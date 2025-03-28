using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using PRN212_Project.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace PRN212_Project.ViewModels
{
    public partial class EventRegistrationViewModel : ObservableObject
    {
        private readonly PrnprojectContext _context;
        private readonly User _currentUser;

        [ObservableProperty]
        private Event selectedEvent;

        [ObservableProperty]
        private ObservableCollection<UserRegistrationInfo> registeredMembers;

        [ObservableProperty]
        private bool canViewRegistrations;

        public EventRegistrationViewModel(User currentUser, Event selectedEvent)
        {
            _context = new PrnprojectContext();
            _currentUser = currentUser;
            SelectedEvent = selectedEvent;
            RegisteredMembers = new ObservableCollection<UserRegistrationInfo>();

            // Allow view for roles 1, 2, and 3
            CanViewRegistrations = currentUser.RoleId <= 3;

            // Load registrations if allowed
            if (CanViewRegistrations)
            {
                LoadRegisteredMembersAsync();
            }
        }

        private async void LoadRegisteredMembersAsync()
        {
            try
            {
                var registrations = await _context.EventRegistrations
                    .Where(er => er.EventId == SelectedEvent.EventId)
                    .Include(er => er.Student)
                    .Select(er => new UserRegistrationInfo
                    {
                        StudentId = er.StudentId,
                        FullName = er.Student.FullName,
                        Username = er.Student.Username,
                        RegistrationStatus = er.Status,
                        RegistrationTime = er.Student.CreatedAt ?? DateTime.MinValue
                    })
                    .ToListAsync();

                RegisteredMembers.Clear();
                foreach (var registration in registrations)
                {
                    RegisteredMembers.Add(registration);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading registered members: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private void CloseView()
        {
            // This method can be used to close the registration view
        }
    }

    // DTO to represent registration information
    public class UserRegistrationInfo
    {
        public string StudentId { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string RegistrationStatus { get; set; }
        public DateTime RegistrationTime { get; set; }
    }
}