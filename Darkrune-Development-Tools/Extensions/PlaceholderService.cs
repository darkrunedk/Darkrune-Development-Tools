using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Darkrune_Development_Tools.Extensions
{
    public static class PlaceholderService
    {
        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.RegisterAttached(
                "Placeholder",
                typeof(string),
                typeof(PlaceholderService),
                new PropertyMetadata(string.Empty, OnPlaceholderTextChanged));

        public static string GetPlaceholderText(UIElement element) => (string)element.GetValue(PlaceholderProperty);

        public static void SetPlaceholderText(UIElement element, string value) => element.SetValue(PlaceholderProperty, value);

        public static void OnPlaceholderTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox textBox)
            {
                textBox.Loaded += (s, args) => ShowPlaceholder(textBox);
                textBox.GotFocus += (s, args) => RemovePlaceholder(textBox);
                textBox.LostFocus += (s, args) => ShowPlaceholder(textBox);
            }
        }

        private static void ShowPlaceholder(TextBox textBox)
        {
            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Foreground = Brushes.Gray;
                textBox.Text = GetPlaceholderText(textBox);
            }
        }

        private static void RemovePlaceholder(TextBox textBox)
        {
            if (textBox.Text == GetPlaceholderText(textBox))
            {
                textBox.Text = string.Empty;
                textBox.Foreground = Brushes.Black;
            }
        }
    }
}
