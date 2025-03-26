using PRN212_Project.Models;
using System.Windows;
using System.Windows.Controls;

namespace PRN212_Project.Views
{
    public partial class EventView : UserControl
    {
        private PrnprojectContext con = new PrnprojectContext();
        private Event selectedEvent;

        public EventView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadEvents();
        }

        private void LoadEvents()
        {
            lvEvents.ItemsSource = con.Events.ToList();
        }

        private void lvEvents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvEvents.SelectedItem is Event ev)
            {
                selectedEvent = ev;
                txtId.Text = ev.EventId.ToString();
                txtName.Text = ev.EventName;
                dpStartTime.SelectedDate = ev.StartTime;
                dpEndTime.SelectedDate = ev.EndTime;
                txtLocation.Text = ev.Location;
                txtStatus.Text = ev.Status;
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            selectedEvent = null;
            lvEvents.SelectedItem = null;
            txtId.Text = "";
            txtName.Text = "";
            dpStartTime.SelectedDate = null;
            dpEndTime.SelectedDate = null;
            txtLocation.Text = "";
            txtStatus.Text = "";
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Event newEvent = new Event
                {
                    EventName = txtName.Text,
                    StartTime = dpStartTime.SelectedDate ?? DateTime.Now,
                    EndTime = dpEndTime.SelectedDate ?? DateTime.Now.AddHours(2),
                    Location = txtLocation.Text,
                    Status = txtStatus.Text,
                    ClubId = 1
                };

                con.Events.Add(newEvent);
                con.SaveChanges();
                LoadEvents();

                MessageBox.Show("Event created!");
            }
            catch (Exception ex)
            {
                string message = ex.InnerException?.InnerException?.Message ?? ex.Message;
                MessageBox.Show("Error creating event:\n" + message);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (selectedEvent == null)
                {
                    MessageBox.Show("Please select an event to update.");
                    return;
                }

                selectedEvent.EventName = txtName.Text;
                selectedEvent.StartTime = dpStartTime.SelectedDate ?? DateTime.Now;
                selectedEvent.EndTime = dpEndTime.SelectedDate ?? DateTime.Now.AddHours(2);
                selectedEvent.Location = txtLocation.Text;
                selectedEvent.Status = txtStatus.Text;

                con.Events.Update(selectedEvent);
                con.SaveChanges();
                LoadEvents();

                MessageBox.Show("Event updated!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating event: " + ex.Message);
            }
        }
    }

}
