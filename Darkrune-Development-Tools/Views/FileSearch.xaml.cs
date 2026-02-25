using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Darkrune_Development_Tools.Views
{
    /// <summary>
    /// Interaction logic for FileSearch.xaml
    /// </summary>
    public partial class FileSearch : Page
    {
        private string _selectedFile;

        public ObservableCollection<string> SearchResults { get; } = [];

        public FileSearch()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void SelectDirBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog dialog = new()
            {
                Title = "Select a folder"
            };

            var result = dialog.ShowDialog();

            if (result == true)
            {
                SelectedDirTxt.Text = dialog.FolderName;

                OpenInExplorerBtn.IsEnabled = false;
                _selectedFile = null;

                ExecuteActionBtn.IsEnabled = true;
            }
        }

        private async void ExecuteActionBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SelectedDirTxt.Text) || !Directory.Exists(SelectedDirTxt.Text))
            {
                MessageBox.Show("Please select a valid directory first.");
                return;
            }

            SearchResults.Clear();
            ExecuteActionBtn.IsEnabled = false;

            var progress = new Progress<string>(file =>
            {
                SearchResults.Add(file);
            });

            await SearchAsync(SelectedDirTxt.Text, SearchPatternTxtBox.Text, progress);

            ExecuteActionBtn.IsEnabled = true;
        }

        public static async Task SearchAsync(string root, string query, IProgress<string> progress)
        {
            await Task.Run(() =>
            {
                foreach (var file in Directory.EnumerateFiles(root, "*", SearchOption.AllDirectories))
                {
                    if (Path.GetFileName(file).Contains(query, StringComparison.OrdinalIgnoreCase))
                    {
                        progress.Report(file);
                    }
                }
            });
        }

        private void SearchResultList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SearchResultList.SelectedItem is string filePath)
            {
                _selectedFile = filePath;
            }

            OpenInExplorerBtn.IsEnabled = !string.IsNullOrWhiteSpace(_selectedFile);
        }

        private void OpenInExplorerBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_selectedFile) && File.Exists(_selectedFile))
            {
                Process.Start("explorer.exe", $"/select,\"{_selectedFile}\"");
            }
        }
    }
}
