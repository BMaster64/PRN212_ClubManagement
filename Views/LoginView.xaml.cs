using System.Windows;
using System.Windows.Input;

namespace PRN212_Project.Views
{
    public partial class LoginView : Window
    {
        private readonly AuthService _authService;

        public LoginView()
        {
            InitializeComponent();
            _authService = new AuthService();
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

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string enteredUsername = username.Text;
            string enteredPassword = password.Password;

            if (string.IsNullOrWhiteSpace(enteredUsername) || string.IsNullOrWhiteSpace(enteredPassword))
            {
                MessageBox.Show("Username and password are required", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var user = _authService.Login(enteredUsername, enteredPassword);

            if (user != null)
            {
                string role = user.RoleId switch
                {
                    1 => "Chủ nhiệm",
                    2 => "Phó chủ nhiệm",
                    3 => "Trưởng ban",
                    4 => "Thành viên",
                    _ => "Unknown"
                };

                HomeView home = new HomeView();
                home.DataContext = new HomeViewModel(user);
                home.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new RegisterView().Show();
            this.Close();
        }
    }
}