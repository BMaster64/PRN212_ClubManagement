using PRN212_Project.Models;
using PRN212_Project.ViewModels;
using System.Windows.Controls;

namespace PRN212_Project.Views
{
    /// <summary>
    /// Interaction logic for NotificationView.xaml
    /// </summary>
    public partial class NotificationView : UserControl
    {
        public NotificationView()
        {
            InitializeComponent();

            // Check if we're in design mode
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
                return;

            // Get the current user from the application state
            var currentUser = App.Current.Properties["CurrentUser"] as User;

            // Set the DataContext
            DataContext = new NotificationViewModel(currentUser);
        }
    }
}