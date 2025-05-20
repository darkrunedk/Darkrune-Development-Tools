using Microsoft.Win32;
using System.IO;
using System.Windows;
using Darkrune_Development_Tools.Core.ConfigHandlers;

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

        private void OpenCommand_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            Open();
        }

        private void Open()
        {
            OpenFileDialog openFileDialog = new()
            {
                Filter = "Appsettings files|appsettings*.json"
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
                    default:
                        break;
                }

                ConfigList.ItemsSource = configSettings;
            }
        }
    }
}