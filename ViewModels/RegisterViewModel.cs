using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;

public partial class RegisterViewModel : ObservableObject
{
    private readonly AuthService _authService;

    [ObservableProperty] private string studentId;
    [ObservableProperty] private string name;
    [ObservableProperty] private string email;
    [ObservableProperty] private string password;
    [ObservableProperty] private string selectedUserType;
    [ObservableProperty] private string message;

    public IRelayCommand RegisterCommand { get; }
    public IRelayCommand NavigateToLoginCommand { get; }

    public event Action? NavigateToLoginRequested;

    public RegisterViewModel(AuthService authService)
    {
        _authService = authService;
        RegisterCommand = new RelayCommand(Register);
        NavigateToLoginCommand = new RelayCommand(() => NavigateToLoginRequested?.Invoke());
    }

    private void Register()
    {
        var newUser = new User
        {
            StudentId = StudentId,
            Name = Name,
            Email = Email,
            Password = Password,
            UserType = 1
        };

        if (_authService.RegisterAsync(newUser).Result)
        {
            MessageBox.Show("Registration successful!");
        }
        else
        {
            Message = "Email already exists!";
        }
    }
}
