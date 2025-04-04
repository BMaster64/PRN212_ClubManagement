﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;

public partial class LoginViewModel : ObservableObject
{
    private readonly AuthService _authService;

    [ObservableProperty] private string username;
    [ObservableProperty] private string password;
    [ObservableProperty] private string message;

    public IRelayCommand LoginCommand { get; }
    public IRelayCommand NavigateToRegisterCommand { get; }

    public event Action? NavigateToRegisterRequested;

    public LoginViewModel(AuthService authService)
    {
        _authService = authService;
        LoginCommand = new RelayCommand(Login);
        NavigateToRegisterCommand = new RelayCommand(() => NavigateToRegisterRequested?.Invoke());
    }

    private void Login()
    {
        var user = _authService.Login(Username, Password);
        if (user != null)
        {
            MessageBox.Show($"Login successful!\nYour role ID: {user.RoleId}");
        }
        else
        {
            Message = "Invalid email or password!";
        }
    }
}

