using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using PRN212_Project.Models;
using PRN212_Project.Views;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;

namespace PRN212_Project.ViewModels
{
    public partial class EventViewModel : ObservableObject
    {
        private readonly PrnprojectContext _context;
        private readonly User _currentUser;

        [ObservableProperty]
        private ObservableCollection<Event> events;

        [ObservableProperty]
        private Event selectedEvent;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private string eventName;

        [ObservableProperty]
        private DateTime? startTime;

        [ObservableProperty]
        private DateTime? endTime;

        [ObservableProperty]
        private string location;

        [ObservableProperty]
        private string status;

        [ObservableProperty]
        private bool showCreateForm;

        [ObservableProperty]
        private bool canManageEvents;

        [ObservableProperty]
        private bool isEditMode;

        // Commands
        public IAsyncRelayCommand LoadEventsCommand { get; }
        public IAsyncRelayCommand RefreshCommand { get; }
        public IRelayCommand ShowCreateFormCommand { get; }
        public IRelayCommand CancelCreateCommand { get; }
        public IAsyncRelayCommand CreateEventCommand { get; }
        public IAsyncRelayCommand UpdateEventCommand { get; }
        public IAsyncRelayCommand DeleteEventCommand { get; }
        public IRelayCommand StartEditCommand { get; }
        public IAsyncRelayCommand RegisterForEventCommand { get; }
        public IAsyncRelayCommand CancelRegistrationCommand { get; }
        public IRelayCommand<Event> ViewRegistrationsCommand { get; }
        public int CurrentUserRoleId { get; set; }

        public EventViewModel(User currentUser)
        {
            _context = new PrnprojectContext();
            _currentUser = currentUser;
            Events = new ObservableCollection<Event>();

            // Set permissions based on user role
            CanManageEvents = currentUser.RoleId <= 3 || currentUser.RoleId == 5;

            // Initialize commands
            LoadEventsCommand = new AsyncRelayCommand(LoadEventsAsync);
            RefreshCommand = new AsyncRelayCommand(LoadEventsAsync);
            RegisterForEventCommand = new AsyncRelayCommand<Event>(RegisterForEventAsync);
            CancelRegistrationCommand = new AsyncRelayCommand<Event>(CancelRegistrationAsync);
            ViewRegistrationsCommand = new RelayCommand<Event>(ViewRegistrations);

            ShowCreateFormCommand = new RelayCommand(() =>
            {
                if (CanManageEvents)
                {
                    ShowCreateForm = true;
                }
                else
                {
                    MessageBox.Show("You don't have permission to create events.",
                        "Permission Denied",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                }
            });
            CancelCreateCommand = new RelayCommand(() =>
            {
                ResetForm();
            });
            CreateEventCommand = new AsyncRelayCommand(async () =>
            {
                try
                {
                    await CreateEventAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            });
            UpdateEventCommand = new AsyncRelayCommand(UpdateEventAsync);
            StartEditCommand = new RelayCommand<Event>(StartEdit);

            // Load events on initialization
            LoadEventsAsync();
        }

        private void ResetForm()
        {
            ShowCreateForm = false;
            IsEditMode = false;
            EventName = string.Empty;
            StartTime = null;
            EndTime = null;
            Location = string.Empty;
            Status = string.Empty;
        }

        private async Task LoadEventsAsync()
        {
            try
            {
                IsLoading = true;
                var clubEvents = await _context.Events
                    .Include(e => e.EventRegistrations)
                    .Where(e => e.ClubId == _currentUser.ClubId)
                    .OrderByDescending(e => e.StartTime)
                    .ToListAsync();

                Events.Clear();
                foreach (var evt in clubEvents)
                {
                    var extendedEvent = new ExtendedEvent(evt, _currentUser);
                    Events.Add(extendedEvent);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading events: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task CreateEventAsync()
        {
            // Add detailed logging
            Debug.WriteLine("CreateEventAsync method started");

            try
            {
                // Log each input value
                Debug.WriteLine($"Event Name: {EventName}");
                Debug.WriteLine($"Start Time: {StartTime}");
                Debug.WriteLine($"End Time: {EndTime}");
                Debug.WriteLine($"Location: {Location}");
                Debug.WriteLine($"Status: {Status}");
                Debug.WriteLine($"Club ID: {_currentUser.ClubId}");

                if (string.IsNullOrWhiteSpace(EventName))
                {
                    Debug.WriteLine("Event name is empty");
                    MessageBox.Show("Event name is required.",
                        "Validation Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

                var newEvent = new Event
                {
                    EventName = EventName,
                    StartTime = StartTime ?? DateTime.Now,
                    EndTime = EndTime ?? DateTime.Now.AddHours(2),
                    Location = Location ?? string.Empty,
                    Status = Status ?? "Upcoming",
                    ClubId = _currentUser.ClubId
                };

                Debug.WriteLine("Attempting to add event to context");
                _context.Events.Add(newEvent);

                Debug.WriteLine("Attempting to save changes");
                int savedChanges = await _context.SaveChangesAsync();

                Debug.WriteLine($"Saved changes: {savedChanges}");

                MessageBox.Show("Event created successfully!",
                    "Success",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                ResetForm();
                await LoadEventsAsync();
            }
            catch (Exception ex)
            {
                // Capture full exception details
                Debug.WriteLine($"Exception in CreateEventAsync: {ex}");
                MessageBox.Show($"Error creating event: {ex.Message}\n\nDetails: {ex.StackTrace}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void StartEdit(Event selectedEvent)
        {
            if (selectedEvent == null) return;

            if (CanManageEvents)
            {
                IsEditMode = true;
                SelectedEvent = selectedEvent;
                EventName = selectedEvent.EventName;
                StartTime = selectedEvent.StartTime;
                EndTime = selectedEvent.EndTime;
                Location = selectedEvent.Location;
                Status = selectedEvent.Status;
                ShowCreateForm = true;
            }
            else
            {
                MessageBox.Show("You don't have permission to edit events.",
                    "Permission Denied",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

        private async Task UpdateEventAsync()
        {
            if (string.IsNullOrWhiteSpace(EventName))
            {
                MessageBox.Show("Event name is required.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            try
            {
                var eventToUpdate = await _context.Events
                    .FirstOrDefaultAsync(e => e.EventId == SelectedEvent.EventId);

                if (eventToUpdate == null)
                {
                    MessageBox.Show("Event not found.",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

                eventToUpdate.EventName = EventName;
                eventToUpdate.StartTime = StartTime ?? DateTime.Now;
                eventToUpdate.EndTime = EndTime ?? DateTime.Now.AddHours(2);
                eventToUpdate.Location = Location;
                eventToUpdate.Status = Status;

                await _context.SaveChangesAsync();

                MessageBox.Show("Event updated successfully!",
                    "Success",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                ResetForm();
                await LoadEventsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating event: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        
        private async Task RegisterForEventAsync(Event selectedEvent)
        {
            if (selectedEvent == null) return;

            try
            {
                // Check if user is already registered
                var existingRegistration = await _context.EventRegistrations
                    .FirstOrDefaultAsync(er =>
                        er.EventId == selectedEvent.EventId &&
                        er.StudentId == _currentUser.StudentId);

                if (existingRegistration != null)
                {
                    MessageBox.Show("You are already registered for this event.",
                        "Registration",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    return;
                }

                // Create new registration
                var newRegistration = new EventRegistration
                {
                    EventId = selectedEvent.EventId,
                    StudentId = _currentUser.StudentId,
                    Status = "Registered" // You can customize this status
                };

                _context.EventRegistrations.Add(newRegistration);
                await _context.SaveChangesAsync();

                MessageBox.Show("Successfully registered for the event!",
                    "Registration",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                // Refresh events to update registration status
                await LoadEventsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error registering for event: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private async Task CancelRegistrationAsync(Event selectedEvent)
        {
            if (selectedEvent == null) return;

            try
            {
                var existingRegistration = await _context.EventRegistrations
                    .FirstOrDefaultAsync(er =>
                        er.EventId == selectedEvent.EventId &&
                        er.StudentId == _currentUser.StudentId);

                if (existingRegistration == null)
                {
                    MessageBox.Show("You are not registered for this event.",
                        "Registration",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    return;
                }

                _context.EventRegistrations.Remove(existingRegistration);
                await _context.SaveChangesAsync();

                MessageBox.Show("Registration cancelled successfully.",
                    "Registration",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                // Refresh events to update registration status
                await LoadEventsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error cancelling registration: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
        private void ViewRegistrations(Event selectedEvent)
        {
            // Only allow roles 1, 2, and 3 to view registrations
            if (_currentUser.RoleId > 3 || _currentUser.RoleId != 5)
            {
                MessageBox.Show("You are not authorized to view event registrations.",
                    "Unauthorized",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            var registrationsView = new EventRegistrationsView
            {
                DataContext = new EventRegistrationViewModel(_currentUser, selectedEvent)
            };
            registrationsView.Show();
        }
    }
    public class ExtendedEvent : Event
    {
        public bool IsRegistered { get; }
        public int RegisteredUsersCount { get; }

        public ExtendedEvent(Event baseEvent, User currentUser)
        {
            // Copy all properties from the base event
            EventId = baseEvent.EventId;
            EventName = baseEvent.EventName;
            StartTime = baseEvent.StartTime;
            EndTime = baseEvent.EndTime;
            Location = baseEvent.Location;
            Description = baseEvent.Description;
            Status = baseEvent.Status;
            ClubId = baseEvent.ClubId;
            Club = baseEvent.Club;
            EventRegistrations = baseEvent.EventRegistrations;

            // Add new properties
            IsRegistered = EventRegistrations
                .Any(er => er.StudentId == currentUser.StudentId);
            RegisteredUsersCount = EventRegistrations.Count;
        }
    }

}