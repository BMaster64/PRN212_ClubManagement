using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using PRN212_Project.Models;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace PRN212_Project.ViewModels
{
    public partial class ChatViewModel : INotifyPropertyChanged
    {
        private readonly User _currentUser;
        private readonly PrnprojectContext _context;
        private string _messageText = string.Empty;
        private ObservableCollection<ChatMessage> _messages = new ObservableCollection<ChatMessage>();
        private readonly DispatcherTimer _refreshTimer;

        public ObservableCollection<ChatMessage> Messages
        {
            get { return _messages; }
            set { _messages = value; OnPropertyChanged(nameof(Messages)); }
        }

        public string MessageText
        {
            get { return _messageText; }
            set
            {
                _messageText = value;
                OnPropertyChanged(nameof(MessageText));
                // This line is important to re-evaluate if the button should be enabled
                (SendMessageCommand as RelayCommand)?.NotifyCanExecuteChanged();
            }
        }

        public ICommand SendMessageCommand { get; }
        public ICommand RefreshCommand { get; }

        public ChatViewModel(User currentUser)
        {
            _currentUser = currentUser;
            _context = new PrnprojectContext();

            SendMessageCommand = new RelayCommand(SendMessage, () => CanSendMessage());
            RefreshCommand = new RelayCommand(LoadMessages);

            // Set up auto-refresh timer
            _refreshTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(5)
            };
            _refreshTimer.Tick += (s, e) => LoadMessages();
            _refreshTimer.Start();

            LoadMessages();
        }

        private bool CanSendMessage()
        {
            return !string.IsNullOrWhiteSpace(MessageText) && _currentUser != null;
        }

        private void SendMessage()
        {
            if (_currentUser == null || !CanSendMessage()) return;

            try
            {
                var channel = _context.ChatChannels
                    .FirstOrDefault(c => c.ClubId == _currentUser.ClubId);

                if (channel == null)
                {
                    // Create a default channel if it doesn't exist
                    var club = _context.Clubs.Find(_currentUser.ClubId);
                    channel = new ChatChannel
                    {
                        ChannelName = $"{club.ClubName} General",
                        ClubId = _currentUser.ClubId,
                        CreatedAt = DateTime.Now
                    };
                    _context.ChatChannels.Add(channel);
                    _context.SaveChanges();
                }

                var message = new ChatMessage
                {
                    Content = MessageText,
                    SenderId = _currentUser.StudentId,
                    ChannelId = channel.ChannelId,
                    SentAt = DateTime.Now
                };

                _context.ChatMessages.Add(message);
                _context.SaveChanges();

                MessageText = string.Empty;
                LoadMessages();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending message: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadMessages()
        {
            if (_currentUser == null) return;

            try
            {
                var messages = _context.ChatMessages
                    .Include(m => m.Sender)
                    .Include(m => m.Channel)
                    .Where(m => m.Channel.ClubId == _currentUser.ClubId)
                    .OrderBy(m => m.SentAt)
                    .ToList();

                Application.Current.Dispatcher.Invoke(() =>
                {
                    Messages.Clear();
                    foreach (var message in messages)
                    {
                        Messages.Add(message);
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading messages: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void Dispose()
        {
            _refreshTimer?.Stop();
            _context?.Dispose();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}