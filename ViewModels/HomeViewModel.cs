using CommunityToolkit.Mvvm.Input;
using PRN212_Project;
using PRN212_Project.Models;
using PRN212_Project.ViewModels;
using PRN212_Project.Views;
using System.ComponentModel;
using System.Windows;

public class HomeViewModel : INotifyPropertyChanged
{
    private readonly User _currentUser;
    private object _currentView;
    private Visibility _reportTabVisibility;
    private string _clubName;
    private Visibility _adminVisibility;
    private Visibility _chatTabVisibility;

    public string ClubName
    {
        get { return _clubName; }
        set { _clubName = value; OnPropertyChanged(nameof(ClubName)); }
    }
    public object CurrentView
    {
        get { return _currentView; }
        set { _currentView = value; OnPropertyChanged(nameof(CurrentView)); }
    }

    public Visibility ReportTabVisibility
    {
        get { return _reportTabVisibility; }
        set { _reportTabVisibility = value; OnPropertyChanged(nameof(ReportTabVisibility)); }
    }
    public Visibility ChatTabVisibility
    {
        get { return _chatTabVisibility; }
        set { _chatTabVisibility = value; OnPropertyChanged(nameof(ChatTabVisibility)); }
    }
    public Visibility AdminVisibility
    {
        get { return _adminVisibility; }
        set { _adminVisibility = value; OnPropertyChanged(nameof(AdminVisibility)); }
    }

    public RelayCommand<string> NavigateCommand { get; }

    public HomeViewModel(User currentUser)
    {
        _currentUser = currentUser;
        ClubName = currentUser.Club.ClubName;
        // Store the current user in the application state
        (Application.Current as App)?.SetCurrentUser(currentUser);
        // Add a debug verification here
        if ((Application.Current as App)?.GetCurrentUser() == null)
        {
            MessageBox.Show("Current user not set properly");
        }
        // Default view
        if (currentUser.RoleId == 5)
        {
            CurrentView = new ClubView { DataContext = new ClubViewModel(_currentUser) };
        }
        else
        {
            CurrentView = new NotificationView { DataContext = new NotificationViewModel(_currentUser) };
        }
        // Only show Report tab for UserType 1, 2, or 3 or 5
        ReportTabVisibility = (currentUser.RoleId != 4)
            ? Visibility.Visible
            : Visibility.Collapsed;
        // Only show Report tab for UserType 1, 2, or 3 or 4
        ChatTabVisibility = (currentUser.RoleId != 5)
            ? Visibility.Visible
            : Visibility.Collapsed;
        // Set admin visibility based on role
        AdminVisibility = (currentUser.RoleId == 5)
            ? Visibility.Visible
            : Visibility.Collapsed;
        NavigateCommand = new RelayCommand<string>(ChangeView);
    }

    private void ChangeView(string parameter)
    {
        switch (parameter)
        {
            case "Member":
                CurrentView = new MemberView { DataContext = new MemberViewModel(_currentUser) };
                break;
            case "Notification":
                CurrentView = new NotificationView { DataContext = new NotificationViewModel(_currentUser) };
                break;
            case "Event":
                CurrentView = new EventView();
                break;
            case "Chat":
                CurrentView = new ChatView { DataContext = new ChatViewModel(_currentUser) };
                break;
            case "Report":
                CurrentView = new ReportView { DataContext = new ReportViewModel(_currentUser) };
                break;
            case "ClubManagement":
                if (_currentUser.RoleId == 5)
                {
                    CurrentView = new ClubView { DataContext = new ClubViewModel(_currentUser) };
                }
                break;
            case "ClubRequests":
                if (_currentUser.RoleId == 5)
                {
                    CurrentView = new ClubRequestsView { DataContext = new ClubRequestsViewModel() };
                }
                break;
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}

