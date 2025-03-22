using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using PRN212_Project.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PRN212_Project.ViewModels
{
    public partial class ReportViewModel : ObservableObject
    {
        private readonly PrnprojectContext _context;
        private readonly User _currentUser;
        public bool HasReports => Reports != null && Reports.Count > 0;
        // Observable properties
        [ObservableProperty]
        private ObservableCollection<UserReportDto> reports;

        [ObservableProperty]
        private UserReportDto selectedReport;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private string reportContent;

        [ObservableProperty]
        private bool isDetailVisible;

        // Properties for creating reports
        [ObservableProperty]
        private string reportTitle;

        [ObservableProperty]
        private bool showCreateForm;

        [ObservableProperty]
        private bool canCreateReport;

        [ObservableProperty]
        private bool canChangeReportStatus;

        [ObservableProperty]
        private ObservableCollection<string> reportStatusOptions;
        // Commands
        public IAsyncRelayCommand LoadReportsCommand { get; }
        public IAsyncRelayCommand RefreshCommand { get; }
        public IRelayCommand<UserReportDto> MarkAsReadCommand { get; }
        public IRelayCommand CloseDetailCommand { get; }
        public IRelayCommand ShowCreateFormCommand { get; }
        public IAsyncRelayCommand CreateReportCommand { get; }
        public IRelayCommand CancelCreateCommand { get; }
        public IAsyncRelayCommand UpdateReportStatusCommand { get; }

        public ReportViewModel(User currentUser)
        {
            _context = new PrnprojectContext();
            _currentUser = currentUser;
            Reports = new ObservableCollection<UserReportDto>();

            // Set permission based on role ID (only role 3 can create reports)
            CanCreateReport = currentUser.RoleId == 3;
            // Set status change permission for roles 1 and 2
            CanChangeReportStatus = currentUser.RoleId == 1 || currentUser.RoleId == 2;
            // Initialize status options
            ReportStatusOptions = new ObservableCollection<string>
            {
                "Pending",
                "Read"
            };
            // Initialize commands
            LoadReportsCommand = new AsyncRelayCommand(LoadReportsAsync);
            RefreshCommand = new AsyncRelayCommand(LoadReportsAsync);
            // Add new command for status update
            UpdateReportStatusCommand = new AsyncRelayCommand(UpdateReportStatusAsync);
            MarkAsReadCommand = new RelayCommand<UserReportDto>(report => {
                SelectedReport = report;  // Set the selected item first
                ShowReportDetail(report);  // Then show the detail
            });

            CloseDetailCommand = new RelayCommand(() => IsDetailVisible = false);

            // Commands for report creation
            ShowCreateFormCommand = new RelayCommand(() =>
            {
                if (CanCreateReport)
                {
                    ShowCreateForm = true;
                }
                else
                {
                    MessageBox.Show("You don't have permission to create reports. Only users with role 3 can create reports.",
                        "Permission Denied",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                }
            });
            CancelCreateCommand = new RelayCommand(() =>
            {
                ShowCreateForm = false;
                ReportTitle = string.Empty;
                ReportContent = string.Empty;
            });
            CreateReportCommand = new AsyncRelayCommand(CreateReportAsync);

            // Load reports when the view model is initialized
            LoadReportsAsync();
        }

        private async Task LoadReportsAsync()
        {
            try
            {
                IsLoading = true;
                Debug.WriteLine("Loading reports for user: " + _currentUser.StudentId + " in club: " + _currentUser.ClubId);

                // Query reports for the user's club using async EF methods
                var userReports = await _context.UserReports
                    .Where(ur => ur.StudentId == _currentUser.StudentId && ur.ClubId == _currentUser.ClubId)
                    .Join(_context.Reports,
                        ur => ur.ReportId,
                        r => r.ReportId,
                        (ur, r) => new { UserReport = ur, Report = r })
                    .Join(_context.Users,
                        result => result.Report.SenderId,
                        user => user.StudentId,
                        (result, user) => new UserReportDto
                        {
                            UserReportId = result.UserReport.UserReportId,
                            ReportId = result.Report.ReportId,
                            Title = result.Report.Title,
                            Content = result.Report.Content,
                            CreatedAt = result.Report.CreatedAt ?? DateTime.Now,
                            SenderName = user.FullName,
                            Status = result.Report.Status,
                            IsRead = result.UserReport.IsRead ?? false
                        })
                    .OrderByDescending(x => x.CreatedAt)
                    .ToListAsync();

                Debug.WriteLine($"Found {userReports.Count} reports");

                // Clear and repopulate the collection
                Reports.Clear();
                foreach (var report in userReports)
                {
                    Reports.Add(report);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading reports: {ex.Message}");
                MessageBox.Show($"Error loading reports: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }
        private async Task UpdateReportStatusAsync()
        {
            if (SelectedReport == null) return;

            try
            {
                // Find the report in the database
                var report = await _context.Reports
                    .FirstOrDefaultAsync(r => r.ReportId == SelectedReport.ReportId);

                if (report != null)
                {
                    // Update the status
                    report.Status = SelectedReport.Status;
                    await _context.SaveChangesAsync();

                    // Show success message
                    MessageBox.Show($"Report status updated to: {SelectedReport.Status}",
                        "Status Updated",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);

                    // Refresh reports to show updated status
                    await LoadReportsAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating report status: {ex.Message}");
                MessageBox.Show($"Error updating report status: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        public void ShowReportDetail(UserReportDto report)
        {
            if (report == null) return;

            // Set content and selected report
            SelectedReport = report;
            ReportContent = report.Content;
            IsDetailVisible = true;

            // Mark as read if not already read
            if (!report.IsRead)
            {
                MarkAsReadAsync(report);
            }
        }

        private async void MarkAsReadAsync(UserReportDto report)
        {
            if (report == null) return;

            try
            {
                var userReport = await _context.UserReports
                    .FirstOrDefaultAsync(ur => ur.UserReportId == report.UserReportId);

                if (userReport != null)
                {
                    userReport.IsRead = true;
                    await _context.SaveChangesAsync();

                    // Update the UI report object
                    report.IsRead = true;

                    // Notify UI of changes
                    OnPropertyChanged(nameof(Reports));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error marking report as read: {ex.Message}");
                MessageBox.Show($"Error marking report as read: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task CreateReportAsync()
        {
            if (string.IsNullOrWhiteSpace(ReportTitle) ||
                string.IsNullOrWhiteSpace(ReportContent))
            {
                MessageBox.Show("Title and content are required.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            try
            {
                // Create the report
                var report = new Report
                {
                    Title = ReportTitle,
                    Content = ReportContent,
                    CreatedAt = DateTime.Now,
                    SenderId = _currentUser.StudentId,
                    Status = "Pending",
                    ClubId = _currentUser.ClubId
                };

                _context.Reports.Add(report);
                await _context.SaveChangesAsync();

                // Get all users in the club with RoleId 1 or 2 to create user reports for each
                var clubAdmins = await _context.Users
                    .Where(u => u.ClubId == _currentUser.ClubId && (u.RoleId == 1 || u.RoleId == 2 || u.RoleId == 3) && u.Status)
                    .ToListAsync();

                // Create user reports for each club admin (RoleId 1 or 2)
                foreach (var admin in clubAdmins)
                {
                    var userReport = new UserReport
                    {
                        ReportId = report.ReportId,
                        StudentId = admin.StudentId,
                        IsRead = false,
                        ClubId = _currentUser.ClubId
                    };

                    _context.UserReports.Add(userReport);
                }

                await _context.SaveChangesAsync();

                MessageBox.Show("Report created and sent to club administrators successfully!",
                    "Success",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                // Clear form and hide it
                ReportTitle = string.Empty;
                ReportContent = string.Empty;
                ShowCreateForm = false;

                // Refresh the report list
                await LoadReportsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating report: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }

    public class UserReportDto : ObservableObject
    {
        private bool _isRead;

        public int UserReportId { get; set; }
        public int ReportId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string SenderName { get; set; }
        public string Status { get; set; }

        public bool IsRead
        {
            get => _isRead;
            set
            {
                SetProperty(ref _isRead, value);
            }
        }
    }
}