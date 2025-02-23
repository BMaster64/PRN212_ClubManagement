using System.Windows;
using System.Windows.Controls;

namespace PRN212_Project.Views
{
    public partial class MemberView : UserControl
    {
        private MemberViewModel _viewModel;

        public MemberView()
        {
            InitializeComponent();

            // Wait for DataContext to be set from HomeViewModel
            this.DataContextChanged += MemberView_DataContextChanged;
        }

        private void MemberView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is MemberViewModel viewModel)
            {
                _viewModel = viewModel;

                // Wire up the PasswordBox
                if (PasswordBox != null)
                {
                    PasswordBox.PasswordChanged += PasswordBox_PasswordChanged;
                }
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (_viewModel != null && sender is PasswordBox passwordBox)
            {
                _viewModel.Password = passwordBox.Password;
            }
        }
    }
}