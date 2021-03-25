using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Set_Retail_5x5.Retail5x5.Common;
using Set_Retail_5x5.Retail5x5.DataAccessLayer;
using Set_Retail_5x5.Retail5x5.Hardware.Printer;
using Set_Retail_5x5.Retail5x5.Hardware.SmartCard;
using Set_Retail_5x5.Retail5x5.Model;
using Set_Retail_5x5.Retail5x5.Model.Data;

namespace Set_Retail_5x5.Retail5x5.Gui.Cards.ViewModel.TabViewModel
{
    public class AddNewCardsKitViewModel : TabItemViewModel
    {
        private int _isScansCount;
        private int _countCardInKit = 2;
        private string _eventMsg;

        public AddNewCardsKitViewModel() : base(string.Empty)
        {
            InitializeCommands();
        }

        public AddNewCardsKitViewModel(string nameHeader) : base(nameHeader)
        {
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            ScanCardsCommand = new Command(m => ScanCardsCommandMethod());
            CalibratePrinterCommand = new Command(m => CalibratePrinterCommandMethod());
        }

        private void CalibratePrinterCommandMethod()
        {
            Printer.Calibration();
        }

        private void ScanCardsCommandMethod()
        {
            EventMsg = string.Empty;
            if (CountCardInKit == 0) return;
            var cards = asc122H.ScanCard(CountCardInKit);
            if (cards == null)
            {
                EventMsg = "Карты не считались";
                return;
            }
            SaveKit(cards);
        }

        private void SaveKit(IEnumerable<string> cards)
        {
            var client = new ClientKit();
            var clientInfo = new Set10ExchangeLoyClient(Guid.NewGuid(), null, DbPg.GetHibernateSequenceNextVal(), null, null, null, null, null, 0, 3, 10,DateTime.Now,string.Empty);
            var cardsList = new ObservableCollection<Set10ExchangeLoyCard>();
            foreach (var card in cards)
            {
                cardsList.Add(new Set10ExchangeLoyCard(clientInfo.Uid, clientInfo.Set10Guid, DbPg.GetHibernateSequenceNextVal(), card, clientInfo.BonusAccountId));
            }
            client.Client = clientInfo;
            client.Cards = cardsList;

            try
            {
                client.Save();
                if (IsPrintLabel)
                    Printer.Print(client.Cards[0]?.CardHexNumber, client.Cards[1]?.CardHexNumber, client.Client.Set10Guid.ToString());
                IsScansCount++;
                EventMsg = $"Комплект с номером {client.Client.Set10Guid} создан";
            }
            catch (Exception ex)
            {
                EventMsg = ex.Message;
            }
        }

        public ICommand ScanCardsCommand { get; set; }

        public ICommand CalibratePrinterCommand { get; set; }

        public int CountCardInKit
        {
            get => _countCardInKit; set
            {
                _countCardInKit = value; 
                if (_countCardInKit > 2) _countCardInKit = 2;
                if (_countCardInKit < 0) _countCardInKit = 0;
            }
        }

        public int CountBonusInCharge { get; set; } = 250;

        public bool IsPrintLabel { get; set; } = true;

        public int IsScansCount
        {
            get => _isScansCount;
            set
            {
                _isScansCount = value; 
                OnPropertyChanged(nameof(IsScansCount));
            }
        }

        public int IsBlockSettings { get; set; }

        public string EventMsg
        {
            get => _eventMsg;
            set
            {
                _eventMsg = value; 
                base.OnPropertyChanged(nameof(EventMsg));
            }
        }
    }
}
