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
            ToTxtBox.Text = System.Security.SecurityElement.Escape(FromTxtBox.Text);
        }
    }
}
