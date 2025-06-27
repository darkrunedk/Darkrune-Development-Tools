using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Darkrune_Development_Tools
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ViewFrame_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            var frame = sender as Frame;
            var menuItems = ToolsMenuItem.Items;
            foreach (MenuItem item in menuItems)
            {
                if (item.CommandParameter?.ToString() == frame.Source.OriginalString)
                    item.Visibility = Visibility.Collapsed;
                else
                    item.Visibility = Visibility.Visible;
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            if (menuItem == null || string.IsNullOrWhiteSpace(menuItem.CommandParameter.ToString()))
                return;

            ViewFrame.Source = new Uri(menuItem.CommandParameter.ToString(), UriKind.Relative);
        }

        private void CheckGithub_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/darkrunedk/Darkrune-Development-Tools",
                UseShellExecute = true
            });
        }
    }
}