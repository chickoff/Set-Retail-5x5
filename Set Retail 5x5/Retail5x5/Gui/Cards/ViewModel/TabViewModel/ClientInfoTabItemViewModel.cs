using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Set_Retail_5x5.Retail5x5.Common;
using Set_Retail_5x5.Retail5x5.Gui.Common;
using Set_Retail_5x5.Retail5x5.Model;
using Set_Retail_5x5.Retail5x5.Model.Data;

namespace Set_Retail_5x5.Retail5x5.Gui.Cards.ViewModel.TabViewModel
{
    public class ClientInfoTabItemViewModel : TabItemViewModel, IDisposable
    {
        private ClientKit _currentClient;
        private ObservableCollection<TabItemViewModel> _detailTabItems;
        private TabItemViewModel _currentDetailTabItem;
        private Set10ExchangeLoyCard _selectedCard;
        //private decimal _activeBonus;

        public ClientInfoTabItemViewModel()
            : base(string.Empty)
        {
        }

        public ClientInfoTabItemViewModel(ClientKit client)
            : base(
                $"{client.Client?.LastName?.ToUpper()} {client.Client?.FirstName?.ToUpper()} {client.Client?.MiddleName?.ToUpper()}"
            )
        {
            CurrentClient = client;
            DetailTabItems = new ObservableCollection<TabItemViewModel>();
            MustHaveTabItems();
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            CopyDecToClipBoardPopupCommand = new Command(CopyDecToClipBoardPopupCommandMethod);
            CopyHexToClipBoardPopupCommand = new Command(CopyHexToClipBoardPopupCommandMethod);
            NewCardPopupCommand = new Command(m => NewCardPopupCommandMethod());
            BlockCardPopupCommand = new Command(BlockCardPopupCommandMethod, BlockCardCommandCanExecutePredicate);
            UnBlockCardPopupCommand = new Command(m => UnBlockCardPopupCommandMethod(),
                UnblockCardCommandCanExecutePredicate);
            ReplaceCardPopupCommand = new Command(m => ReplaceCardPopupCommandMethod());
            DeleteCardPopupCommand = new Command(m => DeleteCardPopupCommandMethod(), DeleteCardCanExecutePredicate);
            CloseClientCommand = new Command(CloseClientCommandMethod);
            OpenClientProfileCommand = new Command(m => OpenClientProfileCommandMethod());
            SaveChangesCommand = new Command(m => SaveChangesCommandMethod(), SaveChangesCanExecutePredicate);
            OpenClientHistoryCommand = new Command(m=> OpenClientHistoryCommandMethod());
        }

        private void OpenClientHistoryCommandMethod()
        {
            var bonusDetail = new CardsBonusDetailTabItemViewModel(CurrentClient, "История клиента");
            AddItem(bonusDetail);
        }

        private void CopyHexToClipBoardPopupCommandMethod(object obj)
        {
            if (obj == null) return;
            Clipboard.Clear();
            Clipboard.SetText(((Set10ExchangeLoyCard)obj).CardHexNumber);
        }

        private void CopyDecToClipBoardPopupCommandMethod(object obj)
        {
            if (obj == null) return;
            Clipboard.Clear();
            Clipboard.SetText(((Set10ExchangeLoyCard)obj).CardNumber);
        }
        
        private void OpenClientProfileCommandMethod()
        {
            var r = new ClientProfileTabItemViewModel(CurrentClient,"АНКЕТА");
            AddItem(r);
        }

        private void CloseClientCommandMethod(object obj)
        {
            if (CurrentDetailTabItem != null)
            {
                _detailTabItems.Remove(CurrentDetailTabItem);
            }
        }

        private bool DeleteCardCanExecutePredicate(object o)
        {
            return SelectedCard?.IsNew ?? false;
        }

        private bool SaveChangesCanExecutePredicate(object o)
        {
            return CurrentClient.IsChanged; 
        }

        private bool UnblockCardCommandCanExecutePredicate(object o)
        {
            if (SelectedCard?.IsBlocked != null)
                return SelectedCard.IsBlocked;
            return false;
        }

        private bool BlockCardCommandCanExecutePredicate(object o)
        {
            if (SelectedCard?.IsBlocked != null)
                return !SelectedCard.IsBlocked;
            return false;
        }

        private void SaveChangesCommandMethod()
        {
            CurrentClient.Save();
        }

        private void NewCardPopupCommandMethod()
        {
            var cardOp = new AddNewCardTabItemViewModel(CurrentClient, $"Новая карта");
            AddItem(cardOp);
        }

        private void DeleteCardPopupCommandMethod()
        {
            CurrentClient.Cards.Remove(SelectedCard);
        }

        private void ReplaceCardPopupCommandMethod()
        {
            throw new NotImplementedException();
        }

        private void UnBlockCardPopupCommandMethod()
        {
            SelectedCard.IsBlocked = false;
        }

        private void BlockCardPopupCommandMethod(object o)
        {
            SelectedCard.IsBlocked = true;
        }

        private void AddItem(TabItemViewModel tab)
        {
            DetailTabItems.Add(tab);
            CurrentDetailTabItem = tab;
        }

        public void MustHaveTabItems()
        {
            OpenClientHistoryCommandMethod();
        }

        public ClientKit CurrentClient
        {
            get { return _currentClient; }
            set
            {
                _currentClient = value; 
                base.OnPropertyChanged(nameof(CurrentClient));
            }
        }

        public ObservableCollection<TabItemViewModel> DetailTabItems
        {
            get { return _detailTabItems; }
            set
            {
                _detailTabItems = value; 
                base.OnPropertyChanged("DetailTabItems");
            }
        }

        public TabItemViewModel CurrentDetailTabItem
        {
            get { return _currentDetailTabItem; }
            set
            {
                _currentDetailTabItem = value; 
                base.OnPropertyChanged(nameof(CurrentDetailTabItem));
            }
        }

        public Set10ExchangeLoyCard SelectedCard
        {
            get => _selectedCard;
            set
            {
                _selectedCard = value;
                base.OnPropertyChanged(nameof(SelectedCard));
            }
        }

        public ICommand CopyDecToClipBoardPopupCommand { get; set; }

        public ICommand CopyHexToClipBoardPopupCommand { get; set; }

        public ICommand NewCardPopupCommand { get; set; }
        public ICommand BlockCardPopupCommand { get; set; }
        public ICommand UnBlockCardPopupCommand { get; set; }
        public ICommand ReplaceCardPopupCommand { get; set; }
        public ICommand DeleteCardPopupCommand { get; set; }
        public ICommand OpenClientProfileCommand { get; set; }
        public ICommand OpenClientHistoryCommand { get; set; }
        public ICommand CloseClientCommand { get; set; }
        public ICommand SaveChangesCommand { get; set; }

        public decimal ActiveBonus
        {
            get
            {
                var set10ExchangeLoyAmmounts = CurrentClient.Ammounts.FirstOrDefault(x => x.BalanceType.Equals(0));
                if (set10ExchangeLoyAmmounts != null)
                    return Convert.ToDecimal(set10ExchangeLoyAmmounts.AmmountComposition / 100);
                else
                {
                    return 0;
                }
            }
        }

        public override void Dispose()
        {
            CurrentClient = null;
        }
    }
}
