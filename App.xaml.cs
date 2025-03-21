using PRN212_Project.Models;
using System.Windows;

namespace PRN212_Project
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // Store the current user in the application properties
        public void SetCurrentUser(User user)
        {
            Properties["CurrentUser"] = user;
        }

        // Get the current user from the application properties
        public User GetCurrentUser()
        {
            return Properties["CurrentUser"] as User;
        }
    }
}