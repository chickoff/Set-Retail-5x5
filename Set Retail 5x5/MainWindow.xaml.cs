using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FluorineFx.ServiceBrowser.Sql;
using Set_Retail_5x5.Retail5x5.DataAccessLayer;
using Set_Retail_5x5.Retail5x5.Global;
using Set_Retail_5x5.Retail5x5.Model.Data;
using Set_Retail_5x5.Retail5x5.Startup;
using WSCardsManagerServiceProxy;

namespace Set_Retail_5x5
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

           var sysTrartup = new SystemSrartup();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //var v = GlobalObjects.Set10AuthToken.JSession;

            //MessageBox.Show(v.Value);//

            var cardNum = "208023083228";


            var sss = new WSInternalCardsProcessingManagerProxy.InternalCardsAccountProsessingManagerService();

            var v1 = sss.getActiveBonusAccounts(cardNum);
            v1.ToString();
            
            var v2 = new WSCardsManagerServiceProxy.CardsManagerService("http://192.168.5.36:8090/SET-Cards/SET/Cards");
            var c = v2.getClientInfoByCardNumber(cardNum);
            
            c.ToString();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //var k = new ClientKit();
            //k.Cards = new ObservableCollection<Set10ExchangeLoyCard> (DbMs.GetCardsByClientUid(Guid.Parse("452F9F71-F20A-4B4B-AA64-D870BBFF83EC")));
        }
    }
}
