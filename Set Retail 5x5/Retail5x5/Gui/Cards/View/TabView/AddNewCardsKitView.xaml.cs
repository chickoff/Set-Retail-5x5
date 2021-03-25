using System.Windows.Controls;

namespace Set_Retail_5x5.Retail5x5.Gui.Cards.View.TabView
{
    /// <summary>
    /// Логика взаимодействия для AddNewCardsKitView.xaml
    /// </summary>
    public partial class AddNewCardsKitView : UserControl
    {
        public AddNewCardsKitView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            inputTextBox.Focus();
        }
    }
}
