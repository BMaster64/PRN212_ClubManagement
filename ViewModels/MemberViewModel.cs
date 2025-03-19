using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using PRN212_Project.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

public partial class MemberViewModel : ObservableObject
{
    private readonly AuthService _authService;
    private readonly PrnprojectContext _dbContext;
    private readonly User _currentUser;

    [ObservableProperty]
    private ObservableCollection<User> members;

    [ObservableProperty]
    private ObservableCollection<int> availableUserTypes;

    [ObservableProperty]
    private bool isEditing;

    [ObservableProperty]
    private User selectedMember;

    [ObservableProperty]
    private string studentId;
    [ObservableProperty]
    private string fullname;
    [ObservableProperty]
    private string username;
    [ObservableProperty]
    private string password;
    [ObservableProperty]
    private int selectedUserType;
    [ObservableProperty]
    private bool showCreateMemberForm;
    [ObservableProperty]
    private bool canAddMembers;
    public IAsyncRelayCommand LoadMembersCommand { get; }
    public IAsyncRelayCommand CreateMemberCommand { get; }
    public IRelayCommand ShowCreateFormCommand { get; }
    public IRelayCommand CancelCreateCommand { get; }
    public IRelayCommand<User> EditMemberCommand { get; }
    public IAsyncRelayCommand<User> DeleteMemberCommand { get; }
    public IAsyncRelayCommand SaveEditCommand { get; }
    public IRelayCommand CancelEditCommand { get; }
    public MemberViewModel(User currentUser)
    {
        _currentUser = currentUser;
        _authService = new AuthService();
        _dbContext = new PrnprojectContext();
        Members = new ObservableCollection<User>();
        AvailableUserTypes = new ObservableCollection<int>();
        CanAddMembers = _currentUser.RoleId <= 3;

        LoadMembersCommand = new AsyncRelayCommand(LoadMembersAsync);
        CreateMemberCommand = new AsyncRelayCommand(CreateMemberAsync);
        ShowCreateFormCommand = new RelayCommand(() =>
        {
            if (CanAddMembers)
            {
                ShowCreateMemberForm = true;
            }
            else
            {
                MessageBox.Show("You don't have permission to add members.",
                    "Permission Denied",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        });
        CancelCreateCommand = new RelayCommand(() => ShowCreateMemberForm = false);
        EditMemberCommand = new RelayCommand<User>(OnEditMember);
        DeleteMemberCommand = new AsyncRelayCommand<User>(DisableMemberAsync);
        SaveEditCommand = new AsyncRelayCommand(SaveEditAsync);
        CancelEditCommand = new RelayCommand(CancelEdit);

        // Initialize by loading members
        LoadMembersAsync();
        SetAvailableUserTypes();
    }

    private async Task LoadMembersAsync()
    {
        var query = _dbContext.Users
            .Where(u => u.ClubId == _currentUser.ClubId && u.Status == true);

        var membersList = await query.ToListAsync();

        Members.Clear();
        foreach (var member in membersList)
        {
            Members.Add(member);
        }
    }

    private void SetAvailableUserTypes()
    {
        AvailableUserTypes.Clear();
        // Add available user types based on current user's type
        for (int i = _currentUser.RoleId + 1; i <= 4; i++)
        {
            AvailableUserTypes.Add(i);
        }
    }

    private async Task CreateMemberAsync()
    {
        if (string.IsNullOrWhiteSpace(StudentId) ||
            string.IsNullOrWhiteSpace(Fullname) ||
            string.IsNullOrWhiteSpace(Username) ||
            string.IsNullOrWhiteSpace(Password))
        {
            MessageBox.Show("All fields are required.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        var newUser = new PRN212_Project.Models.User
        {
            StudentId = StudentId,
            FullName = Fullname,
            Username = Username,
            Password = Password,
            RoleId = SelectedUserType,
            ClubId = _currentUser.ClubId,
            CreatedAt = DateTime.Now,
            Status = true
        };

        try
        {
            // Use await instead of .Wait()
            await _authService.RegisterAsync(newUser);
            MessageBox.Show("Member created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            // Clear form
            StudentId = "";
            Fullname = "";
            Username = "";
            Password = "";
            ShowCreateMemberForm = false;

            // Reload members list
            LoadMembersAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error creating member: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    private void OnEditMember(User member)
    {
        if (member == null) return;

        if (member.RoleId <= _currentUser.RoleId)
        {
            MessageBox.Show("You don't have permission to edit this user.",
                "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        // Set selected member for editing
        SelectedMember = member;

        // Copy member data to form fields
        StudentId = member.StudentId;
        Fullname = member.FullName;
        Username = member.Username;
        Password = ""; // Don't show current password for security
        SelectedUserType = member.RoleId;

        // Show edit form
        IsEditing = true;
        ShowCreateMemberForm = true; // We'll reuse the same form layout
    }
    private void CancelEdit()
    {
        IsEditing = false;
        ShowCreateMemberForm = false;
        SelectedMember = null;

        // Clear form fields
        StudentId = "";
        Fullname = "";
        Username = "";
        Password = "";
    }
    private async Task SaveEditAsync()
    {
        if (string.IsNullOrWhiteSpace(Fullname) ||
            string.IsNullOrWhiteSpace(Username))
        {
            MessageBox.Show("Name and username are required.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        try
        {
            // Update member properties
            SelectedMember.FullName = Fullname;
            SelectedMember.Username = Username;

            // Only update password if provided
            if (!string.IsNullOrWhiteSpace(Password))
            {
                SelectedMember.Password = Password;
            }

            SelectedMember.RoleId = SelectedUserType;

            // Save changes
            await _dbContext.SaveChangesAsync();

            MessageBox.Show("Member updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            // Reset form
            CancelEdit();

            // Refresh the list
            await LoadMembersAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error updating member: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    // Disable member (soft delete)
    private async Task DisableMemberAsync(User member)
    {
        if (member == null) return;

        if (member.RoleId <= _currentUser.RoleId)
        {
            MessageBox.Show("You don't have permission to disable this user.",
                "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        // Confirm deletion
        var result = MessageBox.Show($"Are you sure you want to disable {member.FullName}?",
            "Confirm Disable", MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (result != MessageBoxResult.Yes) return;

        try
        {
            // Soft delete - set status to false
            member.Status = false;
            await _dbContext.SaveChangesAsync();

            MessageBox.Show("Member disabled successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            // Refresh the list
            await LoadMembersAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error disabling member: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    // Add this method to check permissions
    private bool CanModifyUser(User user)
    {
        // Users can only modify users with a higher RoleId (lower rank)
        // e.g., Admin (RoleId=1) can modify Club Leader (RoleId=2)
        return user.RoleId > _currentUser.RoleId;
    }

    // Add this property to be used in the XAML
    public User CurrentUser => _currentUser;
}
