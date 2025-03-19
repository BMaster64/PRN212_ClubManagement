using CommunityToolkit.Mvvm.Input;
using PRN212_Project.Models;
using PRN212_Project.Views;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

public class HomeViewModel : INotifyPropertyChanged
{
    private readonly User _currentUser;
    private object _currentView;
    private Visibility _reportTabVisibility;

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
        // Default view
        CurrentView = new NotificationView();

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
            case "Notification":
                CurrentView = new NotificationView();
                break;
            case "Member":
                CurrentView = new MemberView { DataContext = new MemberViewModel(_currentUser) };
                break;
            case "Event":
                CurrentView = new EventView();
                break;
            case "Chat":
                CurrentView = new ChatView();
                break;
            case "Report":
                CurrentView = new ReportView();
                break;
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}

