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
        //CurrentView = new NotificationView();

        // Only show Report tab for UserType 1, 2, or 3
        ReportTabVisibility = (currentUser.RoleId >= 1 && currentUser.RoleId <= 3)
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
                CurrentView = new EventView { DataContext = new NotificationViewModel(_currentUser) };
                break;
            case "Chat":
                CurrentView = new ChatView();
                break;
            case "Report":
                CurrentView = new ReportView() { DataContext = new ReportViewModel(_currentUser) };
                break;
            default:
                CurrentView = new NotificationView { DataContext = new NotificationViewModel(_currentUser) };
                break;
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}

