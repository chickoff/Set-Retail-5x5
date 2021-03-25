using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Set_Retail_5x5.Retail5x5.Common;
using Set_Retail_5x5.Retail5x5.DataAccessLayer;
using Set_Retail_5x5.Retail5x5.Model;
using Set_Retail_5x5.Retail5x5.Model.Data;


namespace Set_Retail_5x5.Retail5x5.Gui.Cards.ViewModel.TabViewModel
{
    public class FindClientTabItemViewModel : TabItemViewModel
    {
        private List<Set10ExchangeLoyClient> _clientList;
        private QueryForFindClientInfo _queryClientInfo;

        private bool _findByNumCardBit;
        private bool _findByLastNameBit;
        private string _qCardNumber;
        private List<Set10ExchangeLoyClientCategory> _clientCategories;


        public FindClientTabItemViewModel() : base(string.Empty)
        {
        }

        public FindClientTabItemViewModel(string nameHeader) : base(nameHeader)
        {
            QueryClientInfo = new QueryForFindClientInfo();
            InitializeCommands();
            FindByLastNameBit = true;
            ClientCategories = DbMs.GetClientCategory();
        }

        private void InitializeCommands()
        {
            FindNowCommand = new Command(m => FindNowCommandMethod());
            ClearQueryCommand = new Command(m => ClearQueryCommandMethod());
            DblClickDataGridCommand = new Command(DblClickDataGridCommandMethod);
            ClientsToExcelCommand = new Command(m => ClientsToExcelCommandMethod());
        }

        private void ClientsToExcelCommandMethod()
        {
            if (ClientList != null && ClientList.Any())
                ExternalFilesLa.ToExcel(ClientList);
        }

        private void DblClickDataGridCommandMethod(object o)
        {
            if (!(o is Set10ExchangeLoyClient)) return;
            OnSelectedClientInfoEvent(new SelectedClientInfoEventArgs((Set10ExchangeLoyClient) o));
        }

        private void ClearQueryCommandMethod()
        {
            QueryClientInfo = new QueryForFindClientInfo();
            QCardNumber = string.Empty;
        }

        private void FindNowCommandMethod()
        {
            ClientList = FindByLastNameBit ? DbMs.SearchClientInfo(QueryClientInfo) : DbMs.SearchClientInfo(QCardNumber);
        }

        #region MyRegion

        public event EventHandler<SelectedClientInfoEventArgs> SelectedClientInfoEvent;

        public ICommand FindNowCommand { get; set; }

        public ICommand ClientsToExcelCommand { get; set; }

        public ICommand ClearQueryCommand { get; set; }

        public ICommand DblClickDataGridCommand { get; set; }

        public QueryForFindClientInfo QueryClientInfo
        {
            get { return _queryClientInfo; }
            set
            {
                _queryClientInfo = value;
                base.OnPropertyChanged(nameof(QueryClientInfo));
            }
        }

        public List<Set10ExchangeLoyClient> ClientList
        {
            get { return _clientList; }
            set
            {
                _clientList = value;
                base.OnPropertyChanged(nameof(ClientList));
            }
        }

        public bool FindByNumCardBit
        {
            get { return _findByNumCardBit; }
            set
            {
                _findByNumCardBit = value; 
                base.OnPropertyChanged(nameof (FindByNumCardBit) );
            }
        }

        public bool FindByLastNameBit
        {
            get { return _findByLastNameBit; }
            set
            {
                _findByLastNameBit = value;
                base.OnPropertyChanged(nameof(FindByLastNameBit));
            }
        }

        public string QCardNumber
        {
            get { return _qCardNumber; }
            set
            {
                _qCardNumber = value;
                base.OnPropertyChanged(nameof(QCardNumber));
            }
        }


        public List<Set10ExchangeLoyClientCategory> ClientCategories
        {
            get { return _clientCategories; }
            set
            {
                _clientCategories = value;
                base.OnPropertyChanged(nameof(ClientCategories));
            }
        }

        #endregion

        protected virtual void OnSelectedClientInfoEvent(SelectedClientInfoEventArgs e)
        {
            SelectedClientInfoEvent?.Invoke(this, e);
        }
    }
}
