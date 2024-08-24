using System.Windows;

namespace ControllerApplication
{
    public partial class InputDialog : Window
    {
        public string Input { get; private set; }

        public InputDialog(string prompt)
        {
            InitializeComponent();
            PromptTextBlock.Text = prompt;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Input = InputTextBox.Text;
            DialogResult = true;
            Close();
        }
    }
}
