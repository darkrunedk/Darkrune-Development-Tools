using System.Windows;
using System.Windows.Controls;

namespace Darkrune_Development_Tools.Views
{
    /// <summary>
    /// Interaction logic for StringManipulation.xaml
    /// </summary>
    public partial class StringManipulation : Page
    {
        public StringManipulation()
        {
            InitializeComponent();
        }

        private void ExecuteActionBtn_Click(object sender, RoutedEventArgs e)
        {
            string toText = FromTxtBox.Text;
            string seperator = SeperatorSeletor.Text;
            if (seperator.Equals("Linebreak", StringComparison.OrdinalIgnoreCase))
                seperator = DetectLinebreakType(toText);

            string[] toTextParts = toText.Split(seperator, StringSplitOptions.RemoveEmptyEntries);

            string joiner = JoinSelector.Text;
            if (joiner.Equals("Linebreak", StringComparison.OrdinalIgnoreCase))
                joiner = Environment.NewLine;

            toText = string.Join(joiner, toTextParts);
            ToTxtBox.Text = toText;
        }

        private static string DetectLinebreakType(string text)
        {
            int lineFeedCariageReturnCount = text.Split("\r\n", StringSplitOptions.RemoveEmptyEntries).Length;
            int lineFeedCount = text.Split("\n", StringSplitOptions.RemoveEmptyEntries).Length;
            int cariageReturnCount = text.Split("\r", StringSplitOptions.RemoveEmptyEntries).Length;

            if (lineFeedCariageReturnCount >= lineFeedCount && lineFeedCariageReturnCount >= cariageReturnCount)
                return "\r\n";
            else if (lineFeedCount > lineFeedCariageReturnCount && lineFeedCount > cariageReturnCount)
                return "\n";
            else
                return "\r";
        }
    }
}
