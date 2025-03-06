using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
    private readonly DBContext _dbContext;
    private readonly User _currentUser;

    [ObservableProperty]
    private ObservableCollection<User> members;

    [ObservableProperty]
    private ObservableCollection<int> availableUserTypes;

    [ObservableProperty]
    private string studentId;
    [ObservableProperty]
    private string name;
    [ObservableProperty]
    private string email;
    [ObservableProperty]
    private string password;
    [ObservableProperty]
    private int selectedUserType;
    [ObservableProperty]
    private bool showCreateMemberForm;

    public IRelayCommand LoadMembersCommand { get; }
    public IRelayCommand CreateMemberCommand { get; }
    public IRelayCommand ShowCreateFormCommand { get; }
    public IRelayCommand CancelCreateCommand { get; }

    public MemberViewModel(User currentUser)
    {
        _currentUser = currentUser;
        _authService = new AuthService();
        _dbContext = new DBContext();
        Members = new ObservableCollection<User>();
        AvailableUserTypes = new ObservableCollection<int>();

        LoadMembersCommand = new RelayCommand(LoadMembers);
        CreateMemberCommand = new RelayCommand(CreateMember);
        ShowCreateFormCommand = new RelayCommand(() => ShowCreateMemberForm = true);
        CancelCreateCommand = new RelayCommand(() => ShowCreateMemberForm = false);

        // Initialize by loading members
        LoadMembers();
        SetAvailableUserTypes();
    }

    private void LoadMembers()
    {
        var query = _dbContext.Users.Where(u => u.ClubId == _currentUser.ClubId);

        // If not admin (usertype 1), only show members of lower rank
        if (_currentUser.UserType > 1)
        {
            query = query.Where(u => u.UserType > _currentUser.UserType);
        }

        var membersList = query.ToList();

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
        for (int i = _currentUser.UserType + 1; i <= 4; i++)
        {
            AvailableUserTypes.Add(i);
        }
    }

    private void CreateMember()
    {
        if (string.IsNullOrWhiteSpace(Name) ||
            string.IsNullOrWhiteSpace(Email) ||
            string.IsNullOrWhiteSpace(Password))
        {
            MessageBox.Show("All fields are required.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        var newUser = new User
        {
            Name = Name,
            Email = Email,
            Password = Password,
            UserType = SelectedUserType,
            ClubId = _currentUser.ClubId
        };

        try
        {
            _authService.RegisterAsync(newUser).Wait();
            MessageBox.Show("Member created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            // Clear form
            Name = "";
            Email = "";
            Password = "";
            ShowCreateMemberForm = false;

            // Reload members list
            LoadMembers();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error creating member: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
