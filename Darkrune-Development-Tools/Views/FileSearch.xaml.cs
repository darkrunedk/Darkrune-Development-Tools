using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

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
            Loader.Visibility = Visibility.Visible;

            var progress = new Progress<string>(file =>
            {
                SearchResults.Add(file);
            });

            int count = 0;
            var searchCountProgress = new Progress<int>(countProg =>
            {
                count++;
                SearchCount.Text = count.ToString();
            });

            await SearchAsync(SelectedDirTxt.Text, SearchPatternTxtBox.Text, progress, searchCountProgress);

            Loader.Visibility = Visibility.Collapsed;
            ExecuteActionBtn.IsEnabled = true;
        }

        public static async Task SearchAsync(string root, string query, IProgress<string> progress, IProgress<int> searchCountProgress)
        {
            await Task.Run(() =>
            {
                var files = Directory.EnumerateFiles(root, "*", SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    searchCountProgress.Report(1);

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
