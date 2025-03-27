using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using PRN212_Project.Models;
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

        public int CurrentUserRoleId { get; set; }

        public EventViewModel(User currentUser)
        {
            _context = new PrnprojectContext();
            _currentUser = currentUser;
            Events = new ObservableCollection<Event>();

            // Set permissions based on user role
            CanManageEvents = currentUser.RoleId <= 3;

            // Initialize commands
            LoadEventsCommand = new AsyncRelayCommand(LoadEventsAsync);
            RefreshCommand = new AsyncRelayCommand(LoadEventsAsync);
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
            DeleteEventCommand = new AsyncRelayCommand(DeleteEventAsync);
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
                    .Where(e => e.ClubId == _currentUser.ClubId)
                    .OrderByDescending(e => e.StartTime)
                    .ToListAsync();

                Events.Clear();
                foreach (var evt in clubEvents)
                {
                    Events.Add(evt);
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

        private async Task DeleteEventAsync()
        {
            if (SelectedEvent == null) return;

            try
            {
                var result = MessageBox.Show("Are you sure you want to delete this event?",
                    "Confirm Deletion",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.No) return;

                var eventToDelete = await _context.Events
                    .FirstOrDefaultAsync(e => e.EventId == SelectedEvent.EventId);

                if (eventToDelete == null)
                {
                    MessageBox.Show("Event not found.",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

                _context.Events.Remove(eventToDelete);
                await _context.SaveChangesAsync();

                MessageBox.Show("Event deleted successfully!",
                    "Success",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                ResetForm();
                await LoadEventsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting event: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}