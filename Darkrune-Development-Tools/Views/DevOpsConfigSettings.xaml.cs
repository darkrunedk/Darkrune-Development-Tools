using Darkrune_Development_Tools.Core.ConfigHandlers;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;


namespace Darkrune_Development_Tools.Views
{
    /// <summary>
    /// Interaction logic for DevOpsConfigSettings.xaml
    /// </summary>
    public partial class DevOpsConfigSettings : Page
    {
        public DevOpsConfigSettings()
        {
            InitializeComponent();
        }

        private void OpenCommand_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            Open();
        }

        private void Open()
        {
            OpenFileDialog openFileDialog = new()
            {
                Filter = "Appsettings files|appsettings*.json|Web config files|web*.config"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                var extension = Path.GetExtension(openFileDialog.FileName);

                List<Core.Models.ConfigInfoDto> configSettings = [];

                switch (extension)
                {
                    case ".json":
                        AppsettingsHandler appsettingsHandler = new();
                        configSettings = appsettingsHandler.ProcessFile(openFileDialog.FileName);
                        break;
                    case ".config":
                        WebConfigHandler webconfigHandler = new();
                        configSettings = webconfigHandler.ProcessFile(openFileDialog.FileName);
                        break;
                    default:
                        break;
                }

                ConfigList.ItemsSource = configSettings;
            }
        }

        private void GoHome_Click(object sender, RoutedEventArgs e)
        {
            var home = new Home();
            NavigationService.Navigate(home);
        }

        private void ButtonSetClickStyle_Clicked(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var style = (Style)Application.Current.Resources["ListViewBtn_Clicked"];
            btn.Style = style;
        }
    }
}
