using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using PRN212_Project.Models;
using System.Collections.ObjectModel;
using System.Windows;

public partial class ClubRequestsViewModel : ObservableObject
{
    private readonly PrnprojectContext _dbContext;
    private readonly AuthService _authService;

    [ObservableProperty]
    private ObservableCollection<ClubRegistrationRequest> _clubRequests;

    public ClubRequestsViewModel()
    {
        _dbContext = new PrnprojectContext();
        _authService = new AuthService();
        LoadClubRequests();
    }

    private void LoadClubRequests()
    {
        ClubRequests = new ObservableCollection<ClubRegistrationRequest>(
            _dbContext.ClubRegistrationRequests
                .Where(r => r.Status == 1)
                .ToList()
        );
    }

    [RelayCommand]
    private async Task ApproveRequest(ClubRegistrationRequest request)
    {
        try
        {
            // Create a new club
            var newClub = new Club
            {
                ClubName = request.ClubName
            };

            // Create a new user as the club president
            var newUser = new User
            {
                StudentId = request.PresidentStudentId,
                FullName = request.PresidentFullName,
                Username = request.PresidentUsername,
                Password = GenerateTemporaryPassword(), // Generate a temporary password
                RoleId = 1, // Club President
                CreatedAt = DateTime.Now,
                Status = true
            };

            // Use the existing registration method
            bool isRegistered = await _authService.RegisterWithClubAsync(newUser, newClub);

            if (isRegistered)
            {
                // Update the request status
                var existingRequest = _dbContext.ClubRegistrationRequests.Find(request.RequestId);
                if (existingRequest != null)
                {
                    existingRequest.Status = 2;
                    await _dbContext.SaveChangesAsync();
                }

                // Refresh the list
                LoadClubRequests();

                MessageBox.Show($"Club {request.ClubName} has been approved!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Failed to register the club and president.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error approving request: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    private async Task DeclineRequest(ClubRegistrationRequest request)
    {
        try
        {
            var existingRequest = _dbContext.ClubRegistrationRequests.Find(request.RequestId);
            if (existingRequest != null)
            {
                existingRequest.Status = 3;
                await _dbContext.SaveChangesAsync();

                // Refresh the list
                LoadClubRequests();

                MessageBox.Show($"Club {request.ClubName} request has been declined.", "Declined", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error declining request: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private string GenerateTemporaryPassword()
    {
        // Generate a random temporary password
        return Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 10);
    }
}