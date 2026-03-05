using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Path = System.IO.Path;

namespace Darkrune_Development_Tools.Views
{
    /// <summary>
    /// Interaction logic for FileContentSearch.xaml
    /// </summary>
    public partial class FileContentSearch : Page
    {
        private string _selectedFile;

        public ObservableCollection<string> SearchResults { get; } = [];

        public FileContentSearch()
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

        private void OpenInExplorerBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_selectedFile) && File.Exists(_selectedFile))
            {
                Process.Start("explorer.exe", $"/select,\"{_selectedFile}\"");
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

            string filePattern = FileType.SelectedIndex switch
            {
                6 => "*.md",
                5 => "*.log",
                4 => "*.csv",
                3 => "*.xml",
                2 => "*.json",
                1 => "*.txt",
                _ => "*",
            };

            switch (FolderSearchMode.SelectedIndex)
            {
                case 0:
                    await SearchAsync(SelectedDirTxt.Text, FileContentTxtBox.Text, SearchOption.TopDirectoryOnly, filePattern, progress);
                    break;
                case 1:
                    await SearchAsync(SelectedDirTxt.Text, FileContentTxtBox.Text, SearchOption.AllDirectories, filePattern, progress);
                    break;
                default:
                    MessageBox.Show("Please select a valid search option.");
                    ExecuteActionBtn.IsEnabled = true;
                    return;
            }

            ExecuteActionBtn.IsEnabled = true;
        }

        public static async Task SearchAsync(string root, string query, SearchOption searchOption, string filePattern, IProgress<string> progress)
        {
            await Task.Run(() =>
            {
                foreach (var file in Directory.EnumerateFiles(root, filePattern, searchOption))
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
    }
}
