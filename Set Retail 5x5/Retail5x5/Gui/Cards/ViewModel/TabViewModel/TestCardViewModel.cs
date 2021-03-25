using Set_Retail_5x5.Retail5x5.Common;
using Set_Retail_5x5.Retail5x5.Common.Converters;
using Set_Retail_5x5.Retail5x5.DataAccessLayer;
using Set_Retail_5x5.Retail5x5.Hardware.SmartCard;
using Set_Retail_5x5.Retail5x5.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Set_Retail_5x5.Retail5x5.Gui.Cards.ViewModel.TabViewModel
{
    class TestCardViewModel : TabItemViewModel
    {
        private Set10ExchangeLoyClient _clientInfo;
        public TestCardViewModel() : base(string.Empty)
        {
        }
        public TestCardViewModel(string nameHeader) : base(nameHeader)
        {
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            ScanCardsCommand = new Command(m => ScanCardsCommandMethod());
        }

        private void ScanCardsCommandMethod()
        {
            var cards = asc122H.ScanCard(1);
            if (cards == null || cards.Count() == 0)
            {
                return;
            }

            string card = cards[0]; 
            var client = DbMs.GetClientByNumCard(card);
            if (client == null || client.Count() == 0)
            {
                return;
            }
            ClientInfo = client.FirstOrDefault();
        }

        public ICommand ScanCardsCommand { get; set; }

        public Set10ExchangeLoyClient ClientInfo
        {
            get{
                return _clientInfo;
            }
            set {
                _clientInfo = value;
                base.OnPropertyChanged(nameof(ClientInfo));
            }
        }

        private void ScanCard()
        {  
            
        }
}
}
