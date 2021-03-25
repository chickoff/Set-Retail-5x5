using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Set_Retail_5x5.Retail5x5.Common;
using Set_Retail_5x5.Retail5x5.DataAccessLayer;
using Set_Retail_5x5.Retail5x5.Gui.Cards.View;
using Set_Retail_5x5.Retail5x5.Gui.Cards.ViewModel.TabViewModel;
using Set_Retail_5x5.Retail5x5.Gui.Common;
using Set_Retail_5x5.Retail5x5.Model;
using Set_Retail_5x5.Retail5x5.Model.Data;

namespace Set_Retail_5x5.Retail5x5.Gui.Cards.ViewModel
{
   public class CardsManagementViewModel:BaseViewModel
    {
        private ObservableCollection<BaseViewModel> _tabItems;
        private BaseViewModel _selectedItem;

        public CardsManagementViewModel()
        {
            TabItems = new ObservableCollection<BaseViewModel>();
            InitializeCommands();

            //var c = new FindClientTabItemViewModel("Поиск клиента");
            //c.SelectedClientInfoEvent += C_SelectedClientInfoEvent1; ;
            //; TabItems.Add(c);

            //c = new FindClientTabItemViewModel("Поиск клиента");
            //c.SelectedClientInfoEvent += C_SelectedClientInfoEvent1; ;
            //; TabItems.Add(c);

            //SelectedItem = c;
        }


        #region MyRegion паблики тут

        public ObservableCollection<BaseViewModel> TabItems
        {
            get { return _tabItems; }
            set
            {
                _tabItems = value; 
                base.OnPropertyChanged(nameof(TabItems));
            }
        }

        public BaseViewModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                base.OnPropertyChanged(nameof(SelectedItem));
            }
        }

        public ICommand FindClientCommand { get; set; }

        public ICommand AddNewKitCommand { get; set; }

        public ICommand Set10ManagementClientCommand { get; set; }

        public ICommand CloseClientCommand { get; set; }

        public ICommand ChargeOnByClientFromFlieCommand { get; set; }

        public ICommand TestCardCommand { get; set; }

        #endregion

        private void InitializeCommands()
        {
            Set10ManagementClientCommand = new Command(m => Set10ManagementClientCommandMethod());
            FindClientCommand = new Command(m => FindClientCommandMethod());
            CloseClientCommand = new Command(CloseClientCommandMethod);
            AddNewKitCommand = new Command(m => AddNewKitCommandMethod());
            ChargeOnByClientFromFlieCommand = new Command(m => ChargeOnByClientFromFlieCommandMethod());
            TestCardCommand = new Command(m => TestCardCommandMethod());

        }

        private void TestCardCommandMethod()
        {
            var c = new TestCardViewModel("Тест карты");
            AddItem(c);
        }

        private void ChargeOnByClientFromFlieCommandMethod()
        {
            var c = new ChargeOnBalanceByClientFormFileTabItemViewModel("Начисление из файла");
            AddItem(c);
        }

        private void AddNewKitCommandMethod()
        {
            var c = new AddNewCardsKitViewModel("Новые комплекты");
            AddItem(c);
        }

        private void Set10ManagementClientCommandMethod()
        {
            var c = new Set10ManagementTabItemViewModel("Set 10 управление");
            AddItem(c);
        }

        private void CloseClientCommandMethod(object o)
        {
            TabItems.Remove(SelectedItem);
            GC.Collect();
            GC.WaitForFullGCComplete();
        }

        private void FindClientCommandMethod()
        {
            var c = new FindClientTabItemViewModel("Поиск клиента");
            c.SelectedClientInfoEvent += C_SelectedClientInfoEvent1; ;
;           TabItems.Add(c);
            SelectedItem = c;
        }

        private void C_SelectedClientInfoEvent1(object sender, SelectedClientInfoEventArgs e)
        {
            var fullClient = DbMs.GetClientFullInfo((Set10ExchangeLoyClient)e.Client.Clone());
            fullClient.Ammounts = new ObservableCollection<Set10ExchangeLoyAmmounts>(DbPg.GetBonus(fullClient.Client));
            var currentClient = new ClientInfoTabItemViewModel(fullClient);
            AddItem(currentClient);
        }
        
        private void AddItem(BaseViewModel tab)
        {
            TabItems.Add(tab);
            SelectedItem = tab;
        }
    }
}
