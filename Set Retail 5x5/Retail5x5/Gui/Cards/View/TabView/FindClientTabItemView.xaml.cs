using System.Windows;
using System.Windows.Controls;
using Set_Retail_5x5.Retail5x5.Gui.Cards.ViewModel.TabViewModel;

namespace Set_Retail_5x5.Retail5x5.Gui.Cards.View.TabView
{
    /// <summary>
    /// Логика взаимодействия для FindClientTabItemView.xaml
    /// </summary>
    public partial class FindClientTabItemView : UserControl
    {
        public FindClientTabItemView()
        {
            InitializeComponent();
        }

        private void findByLastNameRbtn_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            findByFullNameGrid.IsEnabled = false;
            findByCardGrid.IsEnabled = true;
            var dc = (this.DataContext as FindClientTabItemViewModel);
            dc.FindByLastNameBit = false;
            dc.FindByNumCardBit = true;

        }

        private void findByLastNameRbtn_Checked(object sender, RoutedEventArgs e)
        {
            findByFullNameGrid.IsEnabled = true;
            findByCardGrid.IsEnabled = false;
            var dc = (this.DataContext as FindClientTabItemViewModel);
            dc.FindByLastNameBit = true;
            dc.FindByNumCardBit = false;
        }
    }
}
