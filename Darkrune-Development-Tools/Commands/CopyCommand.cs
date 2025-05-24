using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Darkrune_Development_Tools.Commands
{
    public class CopyCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return parameter != null;
        }

        public void Execute(object parameter)
        {
            string textToCopy = "";
            if (parameter is TextBox textBox)
            {
                textToCopy = textBox.Text;
            }

            if (!string.IsNullOrWhiteSpace(textToCopy))
            {
                Clipboard.SetText(textToCopy);
            }
        }
    }
}
