using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using PRN212_Project.Models; // Giả sử ở namespace này có PrnprojectContext, Event
using System.Collections.ObjectModel;
using System.Windows;

namespace PRN212_Project.ViewModels
{
    public partial class EventViewModel : ObservableObject
    {
        private readonly PrnprojectContext _db;

        [ObservableProperty]
        private ObservableCollection<Event> events;

        [ObservableProperty]
        private Event selectedEvent;

        // Constructor
        public EventViewModel()
        {
            _db = new PrnprojectContext();

            Events = new ObservableCollection<Event>();

            // Load dữ liệu ban đầu
            LoadEventsAsync();
        }

        // Lệnh load
        [RelayCommand]
        private async Task LoadEventsAsync()
        {
            var list = await _db.Events.ToListAsync();
            Events.Clear();
            foreach (var ev in list)
            {
                Events.Add(ev);
            }
        }

        // Lệnh New (tạo SelectedEvent mới)
        [RelayCommand]
        private void NewEvent()
        {
            // Khởi tạo Event mới => hiển thị ở form
            SelectedEvent = new Event();
        }

        // Lệnh Save (thêm/sửa)
        [RelayCommand]
        private async Task SaveEventAsync()
        {
            if (SelectedEvent == null)
            {
                MessageBox.Show("No event is selected.");
                return;
            }

            try
            {
                // Nếu EventId == 0 => là Event mới => add
                // Nếu >0 => update
                if (SelectedEvent.EventId == 0)
                {
                    _db.Events.Add(SelectedEvent);
                }
                else
                {
                    // EF sẽ theo dõi thay đổi SelectedEvent => update
                    _db.Events.Update(SelectedEvent);
                }

                await _db.SaveChangesAsync();

                MessageBox.Show("Saved successfully!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);

                // Reload list
                await LoadEventsAsync();

                // Chọn lại event vừa save
                var justSaved = Events.FirstOrDefault(e => e.EventId == SelectedEvent.EventId);
                if (justSaved != null)
                {
                    SelectedEvent = justSaved;
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Error saving event: {ex.Message}");
            }
        }
    }
}
