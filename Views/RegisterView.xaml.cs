using System.Windows;
using System.Windows.Input;

namespace PRN212_Project.Views
{
    public partial class RegisterView : Window
    {
        private readonly AuthService _authService;
        private readonly DBContext _dbContext;

        public RegisterView()
        {
            InitializeComponent();
            _authService = new AuthService();
            _dbContext = new DBContext();
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
            string enteredName = username.Text;
            string enteredEmail = mail.Text;
            string enteredPassword = password.Password;
            string enteredConfirmPassword = confirmPassword.Password;

            if (string.IsNullOrEmpty(enteredStudentId) ||
                string.IsNullOrWhiteSpace(enteredName) ||
                string.IsNullOrWhiteSpace(enteredEmail) ||
                string.IsNullOrWhiteSpace(enteredPassword) ||
                string.IsNullOrWhiteSpace(enteredConfirmPassword))
            {
                MessageBox.Show("All fields are required.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (enteredPassword != enteredConfirmPassword)
            {
                MessageBox.Show("Passwords do not match!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Check if the email or phone already exists
            if (await _authService.EmailExistsAsync(enteredEmail))
            {
                MessageBox.Show("Email already exists in the system!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int newClubId = _dbContext.GetNextClubId();

            User newUser = new User
            {
                StudentId = enteredStudentId,
                Name = enteredName,
                Email = enteredEmail,
                Password = enteredPassword, // TODO: Hash the password before storing
                UserType = 1, // Default to "Chủ nhiệm"
                ClubId = newClubId
            };

            bool isRegistered = await _authService.RegisterAsync(newUser);

            if (isRegistered)
            {
                MessageBox.Show("Registration successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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
