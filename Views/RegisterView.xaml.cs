using PRN212_Project.Models;
using System.Windows;
using System.Windows.Input;

namespace PRN212_Project.Views
{
    public partial class RegisterView : Window
    {
        private readonly AuthService _authService;
        private readonly PrnprojectContext _dbContext;

        public RegisterView()
        {
            InitializeComponent();
            _authService = new AuthService();
            _dbContext = new PrnprojectContext();
        }

        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string enteredStudentId = studentId.Text;
            string enteredFullName = fullname.Text;
            string enteredUsername = username.Text;
            string enteredPassword = password.Password;
            string enteredConfirmPassword = confirmPassword.Password;
            string enteredClubName = clubName.Text;

            if (string.IsNullOrEmpty(enteredStudentId) ||
                string.IsNullOrWhiteSpace(enteredFullName) ||
                string.IsNullOrWhiteSpace(enteredUsername) ||
                string.IsNullOrWhiteSpace(enteredPassword) ||
                string.IsNullOrWhiteSpace(enteredConfirmPassword) ||
                string.IsNullOrWhiteSpace(enteredClubName))
            {
                MessageBox.Show("All required fields must be filled out.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (enteredPassword != enteredConfirmPassword)
            {
                MessageBox.Show("Passwords do not match!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Check if the username or student ID already exists
            if (await _authService.UsernameExistsAsync(enteredUsername))
            {
                MessageBox.Show("Username already exists in the system!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (await _authService.StudentIdExistsAsync(enteredStudentId))
            {
                MessageBox.Show("Student ID already exists in the system!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Create club object
            var newClub = new Club
            {
                ClubName = enteredClubName
            };

            // Create user object
            User newUser = new User
            {
                StudentId = enteredStudentId,
                FullName = enteredFullName,
                Username = enteredUsername,
                Password = enteredPassword,
                RoleId = 1, // Club President
                CreatedAt = DateTime.Now,
                Status = true // Set default status to active
            };

            // Register user with new club
            bool isRegistered = await _authService.RegisterWithClubAsync(newUser, newClub);

            if (isRegistered)
            {
                MessageBox.Show($"Registration successful! You are now the president of {enteredClubName}.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                LoginView loginView = new LoginView();
                loginView.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Registration failed. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            LoginView loginView = new LoginView();
            loginView.Show();
            this.Close();
        }
    }
}