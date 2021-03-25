using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Set_Retail_5x5.Retail5x5.Common;
using Set_Retail_5x5.Retail5x5.Common.Converters;
using Set_Retail_5x5.Retail5x5.Model;
using Set_Retail_5x5.Retail5x5.Model.Data;


namespace Set_Retail_5x5.Retail5x5.Gui.Cards.ViewModel.TabViewModel
{
    public class AddNewCardTabItemViewModel : TabItemViewModel
    {
        private bool _isReadOnly = false;
        private string _cardNumber;
        public ClientKit Client { get; set; }

        public Set10ExchangeLoyCard NewCard { get; set; }
        
        public AddNewCardTabItemViewModel() : base("Н К")
        {
        }

        public AddNewCardTabItemViewModel(ClientKit client, string nameHeader) : base(nameHeader)
        {
            Client = client;
            NewCard = new Set10ExchangeLoyCard(Client.Client.Uid, Client.Client.Set10Guid, Client.Client.BonusAccountId);
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            AddNewCardCommand = new Command(m => AddNewCardCommandMethod(),CanAddNewCardCommand);
        }

        private bool CanAddNewCardCommand(object o)
        {
            return !IsExist();
        }

        private void AddNewCardCommandMethod()
        {
            if (IsExist()) return;
            Client.Cards.Add(NewCard);
            IsReadOnly = IsExist();
        }

        private bool IsExist()
        {
           return Client.Cards.Any(x => x.Uid.Equals(NewCard.Uid));
        }

        public bool IsReadOnly
        {
            get { return _isReadOnly; }
            private set
            {
                _isReadOnly = value; 
                base.OnPropertyChanged(nameof(IsReadOnly));
            }
        }

        public string NameOfButton { get; } = "Добавить";

        public string CardNumber
        {
            get { return _cardNumber; }
            set
            {
                _cardNumber = value;

                if (_cardNumber.Length != 8 && _cardNumber.Length != 12)
                {

                    //return;
                    throw new ArgumentException("not");
                }

                if (CardNumber.Length == 8)
                {
                    NewCard.CardNumber = CardNumber.ToDec();
                    NewCard.CardHexNumber = CardNumber;
                }

                if (CardNumber.Length == 12)
                {
                    NewCard.CardNumber = CardNumber;
                    NewCard.CardHexNumber = CardNumber.ToHex();
                }
            }
        }

        public ICommand AddNewCardCommand { get; set; }
    }
}
