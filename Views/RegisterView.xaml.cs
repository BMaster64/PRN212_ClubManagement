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
            string enteredName = username.Text;
            string enteredEmail = mail.Text;
            string enteredPhone = phone.Text;
            string enteredPassword = password.Password;
            string enteredConfirmPassword = confirmPassword.Password;

            if (string.IsNullOrWhiteSpace(enteredName) ||
                string.IsNullOrWhiteSpace(enteredEmail) ||
                string.IsNullOrWhiteSpace(enteredPhone) ||
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
            if (await _authService.EmailExistsAsync(enteredEmail) || await _authService.PhoneExistsAsync(enteredPhone))
            {
                MessageBox.Show("Email or Phone Number already exists in the system!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int newGroupId = _dbContext.GetNextGroupId();

            User newUser = new User
            {
                Name = enteredName,
                Email = enteredEmail,
                Phone = enteredPhone,
                Password = enteredPassword, // TODO: Hash the password before storing
                UserType = 1, // Default to "Chủ nhiệm"
                GroupId = newGroupId
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
