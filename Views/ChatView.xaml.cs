﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PRN212_Project.ViewModels;

namespace PRN212_Project.Views
{
    public partial class ChatView : UserControl
    {
        public ChatView()
        {
            InitializeComponent();
            this.Loaded += ChatView_Loaded;
        }

        private void ChatView_Loaded(object sender, RoutedEventArgs e)
        {
            if (ChatScrollViewer != null && ChatScrollViewer.Content is FrameworkElement content)
            {
                // Scroll to bottom when messages are loaded
                content.SizeChanged += (s, args) =>
                {
                    ChatScrollViewer.ScrollToEnd();
                };
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var vm = DataContext as ChatViewModel;
                var command = vm?.SendMessageCommand;

                if (command != null && command.CanExecute(null))
                {
                    command.Execute(null);
                    e.Handled = true;
                }
            }
        }
    }
}