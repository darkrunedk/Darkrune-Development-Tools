using System.Windows;
using System.Windows.Controls;

namespace Darkrune_Development_Tools.Views
{
    /// <summary>
    /// Interaction logic for XmlValueEncoder.xaml
    /// </summary>
    public partial class XmlValueEncoder : Page
    {
        public XmlValueEncoder()
        {
            InitializeComponent();
        }

        private void ExecuteActionBtn_Click(object sender, RoutedEventArgs e)
        {
            ToTxtBox.Text = ActionComboBox.SelectedIndex switch
            {
                1 => UnescapeSecurityElement(FromTxtBox.Text),
                _ => System.Security.SecurityElement.Escape(FromTxtBox.Text),
            };
        }

        public static string UnescapeSecurityElement(string input)
        {
            if (input == null)
                return null;

            return input
                .Replace("&lt;", "<")
                .Replace("&gt;", ">")
                .Replace("&quot;", "\"")
                .Replace("&apos;", "'")
                .Replace("&amp;", "&");
        }

    }
}
