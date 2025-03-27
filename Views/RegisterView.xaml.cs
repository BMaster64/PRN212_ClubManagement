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
            var clubRequest = new ClubRegistrationRequest
            {
                ClubName = enteredClubName,
                PresidentStudentId = enteredStudentId,
                PresidentFullName = enteredFullName,
                PresidentUsername = enteredUsername,
                RequestedAt = DateTime.Now,
                Status = 1
            };

            try
            {
                _dbContext.ClubRegistrationRequests.Add(clubRequest);
                await _dbContext.SaveChangesAsync();

                MessageBox.Show($"Club registration request for {enteredClubName} has been submitted. " +
                    "An admin will review your request.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                LoginView loginView = new LoginView();
                loginView.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to submit registration request: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            LoginView loginView = new LoginView();
            loginView.Show();
            this.Close();
        }
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}