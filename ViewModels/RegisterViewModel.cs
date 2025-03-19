using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PRN212_Project.Models;
using System.Collections.ObjectModel;
using System.Windows;

public partial class RegisterViewModel : ObservableObject
{
    private readonly AuthService _authService;

    [ObservableProperty] private string studentId;
    [ObservableProperty] private string fullname;
    [ObservableProperty] private string username;
    [ObservableProperty] private string password;
    [ObservableProperty] private string selectedUserType;
    [ObservableProperty] private string message;

    public IRelayCommand RegisterCommand { get; }
    public IRelayCommand NavigateToLoginCommand { get; }

    public event Action? NavigateToLoginRequested;

    public RegisterViewModel(AuthService authService)
    {
        _authService = authService;
        RegisterCommand = new AsyncRelayCommand(Register);
        NavigateToLoginCommand = new RelayCommand(() => NavigateToLoginRequested?.Invoke());
    }

    private async Task Register()
    {
        var newUser = new User
        {
            StudentId = StudentId,
            FullName = Fullname,
            Username = Username,
            Password = Password,
            RoleId = 1
        };

        if (await _authService.RegisterAsync(newUser))
        {
            MessageBox.Show("Registration successful as Club President!");
        }
        else
        {
            Message = "Registration failed. Username or Student ID might already exist.";
        }
    }
}
