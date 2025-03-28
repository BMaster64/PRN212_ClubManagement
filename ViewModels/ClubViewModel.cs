using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using PRN212_Project.Models;
using PRN212_Project.Views;
using System.Collections.ObjectModel;
using System.Windows;

public partial class ClubViewModel : ObservableObject
{
    private readonly PrnprojectContext _dbContext;
    private readonly User _currentUser;
    public IRelayCommand ExportToExcelCommand { get; }

    [ObservableProperty]
    private ObservableCollection<Club> _clubs;

    [ObservableProperty]
    private bool _showEditClubForm;

    [ObservableProperty]
    private Club _selectedClub;

    public ClubViewModel(User currentUser)
    {
        ExportToExcelCommand = new RelayCommand(ExportToExcel);

        _currentUser = currentUser;
        _dbContext = new PrnprojectContext();
        LoadClubs();
    }

    private void LoadClubs()
    {
        Clubs = new ObservableCollection<Club>(_dbContext.Clubs.ToList());
    }

    [RelayCommand]
    private void EditClub(Club club)
    {
        SelectedClub = club;
        ShowEditClubForm = true;
    }

    [RelayCommand]
    private void SaveEditClub()
    {
        if (SelectedClub == null) return;

        try
        {
            var clubToUpdate = _dbContext.Clubs.Find(SelectedClub.ClubId);
            if (clubToUpdate != null)
            {
                clubToUpdate.ClubName = SelectedClub.ClubName;
                _dbContext.SaveChanges();

                // Refresh the list
                LoadClubs();
                ShowEditClubForm = false;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error updating club: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    private void CancelEdit()
    {
        ShowEditClubForm = false;
        SelectedClub = null;
    }

    [RelayCommand]
    private void DeleteClub(Club club)
    {
        var result = MessageBox.Show("Are you sure you want to delete this club?",
            "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        if (result == MessageBoxResult.Yes)
        {
            try
            {
                var clubToDelete = _dbContext.Clubs.Find(club.ClubId);
                if (clubToDelete != null)
                {
                    _dbContext.Clubs.Remove(clubToDelete);
                    _dbContext.SaveChanges();

                    // Refresh the list
                    LoadClubs();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting club: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    [RelayCommand]
    private void ManageClub(Club club)
    {
        try
        {
            var adminUser = _dbContext.Users
                .Include(u => u.Club)
                .FirstOrDefault(u => u.StudentId == _currentUser.StudentId);
            if (adminUser != null)
            {
                adminUser.ClubId = club.ClubId;
                _dbContext.SaveChanges();

                // Find the current HomeView
                HomeView currentHomeView = Application.Current.Windows
                    .OfType<HomeView>()
                    .FirstOrDefault();

                if (currentHomeView != null)
                {
                    // Update the existing HomeView's DataContext
                    currentHomeView.DataContext = new HomeViewModel(adminUser);

                    // Bring the existing window to front
                    currentHomeView.Activate();
                }
                else
                {
                    // If no HomeView exists, create a new one
                    HomeView newHomeView = new HomeView();
                    newHomeView.DataContext = new HomeViewModel(adminUser);
                    newHomeView.Show();
                }

                MessageBox.Show($"Now managing {club.ClubName}", "Club Management", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error managing club: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void ExportToExcel()
    {
        try
        {
            // Open file dialog to choose save location
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel Files|*.xlsx",
                Title = "Export Clubs to Excel",
                FileName = $"Clubs_{DateTime.Now:yyyyMMdd}.xlsx"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                // Export all clubs in the current collection
                ExcelExportHelper.ExportClubsToExcel(Clubs, saveFileDialog.FileName);

                MessageBox.Show("Clubs exported successfully!",
                                "Export Successful",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error exporting clubs: {ex.Message}",
                            "Export Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
        }
    }

}