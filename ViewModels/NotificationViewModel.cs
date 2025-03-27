using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using PRN212_Project.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PRN212_Project.ViewModels
{
    public partial class NotificationViewModel : ObservableObject
    {
        private readonly PrnprojectContext _context;
        private readonly User _currentUser;

        // Observable properties
        [ObservableProperty]
        private ObservableCollection<UserNotificationDto> notifications;

        [ObservableProperty]
        private UserNotificationDto selectedNotification;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private string notificationContent;

        [ObservableProperty]
        private bool isDetailVisible;

        // Properties for creating notifications
        [ObservableProperty]
        private string notificationTitle;

        [ObservableProperty]
        private bool showCreateForm;

        [ObservableProperty]
        private bool canCreateNotification;
        [ObservableProperty]
        private bool isEditMode;

        [ObservableProperty]
        private int editingNotificationId;
        // Commands
        public IAsyncRelayCommand LoadNotificationsCommand { get; }
        public IAsyncRelayCommand RefreshCommand { get; } // Added missing command
        public IRelayCommand<UserNotificationDto> MarkAsReadCommand { get; }
        public IRelayCommand CloseDetailCommand { get; }
        public IRelayCommand ShowCreateFormCommand { get; }
        public IAsyncRelayCommand CreateNotificationCommand { get; }
        public IRelayCommand CancelCreateCommand { get; }
        public IAsyncRelayCommand DeleteNotificationCommand { get; }
        public IRelayCommand StartEditCommand { get; }
        public IAsyncRelayCommand UpdateNotificationCommand { get; }

        public NotificationViewModel(User currentUser)
        {
            _context = new PrnprojectContext();
            _currentUser = currentUser;
            Notifications = new ObservableCollection<UserNotificationDto>();

            // Set permission based on role ID (1, 2, or 3 can create)
            CanCreateNotification = currentUser.RoleId <= 3 || currentUser.RoleId == 5;

            // Initialize commands
            LoadNotificationsCommand = new AsyncRelayCommand(LoadNotificationsAsync);
            RefreshCommand = new AsyncRelayCommand(LoadNotificationsAsync); // Added missing command

            // Fix notification selection and detail view
            MarkAsReadCommand = new RelayCommand<UserNotificationDto>(notification => {
                SelectedNotification = notification;  // Set the selected item first
                ShowNotificationDetail(notification);  // Then show the detail
            });

            CloseDetailCommand = new RelayCommand(() => IsDetailVisible = false);

            // Commands for notification creation
            ShowCreateFormCommand = new RelayCommand(() =>
            {
                if (CanCreateNotification)
                {
                    ShowCreateForm = true;
                }
                else
                {
                    MessageBox.Show("You don't have permission to create notifications.",
                        "Permission Denied",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                }
            });
            CancelCreateCommand = new RelayCommand(() =>
            {
                ShowCreateForm = false;
                NotificationTitle = string.Empty;
                NotificationContent = string.Empty;
            });
            CreateNotificationCommand = new AsyncRelayCommand(CreateNotificationAsync);
            DeleteNotificationCommand = new AsyncRelayCommand<UserNotificationDto>(DeleteNotificationAsync);
            StartEditCommand = new RelayCommand<UserNotificationDto>(StartEdit);
            UpdateNotificationCommand = new AsyncRelayCommand(UpdateNotificationAsync);
            // Load notifications when the view model is initialized
            LoadNotificationsAsync();
        }

        private async Task LoadNotificationsAsync()
        {
            try
            {
                IsLoading = true;
                Debug.WriteLine("Loading notifications for user: " + _currentUser.StudentId + " in club: " + _currentUser.ClubId);

                // Query notifications for the user's club using async EF methods
                var userNotifications = await _context.UserNotifications
                    .Where(un => un.StudentId == _currentUser.StudentId && un.ClubId == _currentUser.ClubId)
                    .Join(_context.Notifications,
                        un => un.NotificationId,
                        n => n.NotificationId,
                        (un, n) => new { UserNotification = un, Notification = n })
                    .Join(_context.Users,
                        result => result.Notification.SenderId,
                        user => user.StudentId,
                        (result, user) => new UserNotificationDto
                        {
                            UserNotificationId = result.UserNotification.UserNotificationId,
                            NotificationId = result.Notification.NotificationId,
                            Title = result.Notification.Title,
                            Content = result.Notification.Content,
                            CreatedAt = result.Notification.CreatedAt ?? DateTime.Now,
                            SenderName = user.FullName,
                            IsRead = result.UserNotification.IsRead ?? false
                        })
                    .OrderByDescending(x => x.CreatedAt)
                    .ToListAsync();

                Debug.WriteLine($"Found {userNotifications.Count} notifications");

                // Clear and repopulate the collection
                Notifications.Clear();
                foreach (var notification in userNotifications)
                {
                    Notifications.Add(notification);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading notifications: {ex.Message}");
                MessageBox.Show($"Error loading notifications: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        // Made public to allow access from command binding
        public void ShowNotificationDetail(UserNotificationDto notification)
        {
            if (notification == null) return;

            // Set content and selected notification
            SelectedNotification = notification;
            NotificationContent = notification.Content;
            IsDetailVisible = true;

            // Mark as read if not already read
            if (!notification.IsRead)
            {
                MarkAsReadAsync(notification);
            }
        }

        // Changed to async method to properly handle database operations
        private async void MarkAsReadAsync(UserNotificationDto notification)
        {
            if (notification == null) return;

            try
            {
                var userNotification = await _context.UserNotifications
                    .FirstOrDefaultAsync(un => un.UserNotificationId == notification.UserNotificationId);

                if (userNotification != null)
                {
                    userNotification.IsRead = true;
                    await _context.SaveChangesAsync();

                    // Update the UI notification object
                    notification.IsRead = true;

                    // Notify UI of changes
                    OnPropertyChanged(nameof(Notifications));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error marking notification as read: {ex.Message}");
                MessageBox.Show($"Error marking notification as read: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Create notification method
        private async Task CreateNotificationAsync()
        {
            if (string.IsNullOrWhiteSpace(NotificationTitle) ||
                string.IsNullOrWhiteSpace(NotificationContent))
            {
                MessageBox.Show("Title and content are required.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            try
            {
                // Create the notification
                var notification = new Notification
                {
                    Title = NotificationTitle,
                    Content = NotificationContent,
                    CreatedAt = DateTime.Now,
                    SenderId = _currentUser.StudentId
                };

                _context.Notifications.Add(notification);
                await _context.SaveChangesAsync();

                // Get all users in the club to create user notifications for each
                var clubMembers = await _context.Users
                    .Where(u => (u.ClubId == _currentUser.ClubId && u.Status) || u.RoleId == 5)
                    .ToListAsync();

                // Create user notifications for each club member
                foreach (var member in clubMembers)
                {
                    var userNotification = new UserNotification
                    {
                        NotificationId = notification.NotificationId,
                        StudentId = member.StudentId,
                        IsRead = false,
                        ClubId = _currentUser.ClubId
                    };

                    _context.UserNotifications.Add(userNotification);
                }

                await _context.SaveChangesAsync();

                MessageBox.Show("Notification created successfully!",
                    "Success",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                // Clear form and hide it
                NotificationTitle = string.Empty;
                NotificationContent = string.Empty;
                ShowCreateForm = false;

                // Refresh the notification list
                await LoadNotificationsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating notification: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
        private void StartEdit(UserNotificationDto notification)
        {
            // Only allow edit for users with role 1, 2, or 3 and for notifications they created
            if (CanCreateNotification && _context.Notifications
                .Any(n => n.NotificationId == notification.NotificationId && n.SenderId == _currentUser.StudentId))
            {
                IsEditMode = true;
                EditingNotificationId = notification.NotificationId;
                NotificationTitle = notification.Title;
                NotificationContent = notification.Content;
            }
            else
            {
                MessageBox.Show("You don't have permission to edit this notification.",
                    "Permission Denied",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

        private async Task UpdateNotificationAsync()
        {
            if (string.IsNullOrWhiteSpace(NotificationTitle) ||
                string.IsNullOrWhiteSpace(NotificationContent))
            {
                MessageBox.Show("Title and content are required.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            try
            {
                var notification = await _context.Notifications
                    .FirstOrDefaultAsync(n => n.NotificationId == EditingNotificationId);

                if (notification == null)
                {
                    MessageBox.Show("Notification not found.",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

                // Ensure the current user is the sender
                if (notification.SenderId != _currentUser.StudentId)
                {
                    MessageBox.Show("You can only edit notifications you created.",
                        "Permission Denied",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                // Update notification details
                notification.Title = NotificationTitle;
                notification.Content = NotificationContent;

                await _context.SaveChangesAsync();

                MessageBox.Show("Notification updated successfully!",
                    "Success",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                // Reset edit mode and refresh notifications
                IsEditMode = false;
                NotificationTitle = string.Empty;
                NotificationContent = string.Empty;
                await LoadNotificationsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating notification: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private async Task DeleteNotificationAsync(UserNotificationDto notification)
        {
            if (notification == null) return;

            try
            {
                // Check if the current user can delete the notification
                var originalNotification = await _context.Notifications
                    .FirstOrDefaultAsync(n => n.NotificationId == notification.NotificationId);

                if (originalNotification == null)
                {
                    MessageBox.Show("Notification not found.",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

                // Only allow deletion for the sender or users with roles 1-3
                if (originalNotification.SenderId != _currentUser.StudentId && (_currentUser.RoleId > 3 && _currentUser.RoleId != 5))
                {
                    MessageBox.Show("You don't have permission to delete this notification.",
                        "Permission Denied",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                // Confirm deletion
                var result = MessageBox.Show("Are you sure you want to delete this notification?",
                    "Confirm Deletion",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.No) return;

                // Remove associated UserNotifications first
                var userNotifications = await _context.UserNotifications
                    .Where(un => un.NotificationId == notification.NotificationId)
                    .ToListAsync();

                _context.UserNotifications.RemoveRange(userNotifications);

                // Then remove the notification
                _context.Notifications.Remove(originalNotification);

                await _context.SaveChangesAsync();

                MessageBox.Show("Notification deleted successfully!",
                    "Success",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                // Refresh notifications
                await LoadNotificationsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting notification: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }

    public class UserNotificationDto : ObservableObject
    {
        private bool _isRead;

        public int UserNotificationId { get; set; }
        public int NotificationId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string SenderName { get; set; }

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