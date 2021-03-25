using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Set_Retail_5x5.Retail5x5.Common;
using Set_Retail_5x5.Retail5x5.DataAccessLayer;
using Set_Retail_5x5.Retail5x5.Model;
using Set_Retail_5x5.Retail5x5.Model.Data;
using Set_Retail_5x5.Retail5x5.Model.SynchroData;

namespace Set_Retail_5x5.Retail5x5.Gui.Cards.ViewModel.TabViewModel
{
    public class CardsBonusDetailTabItemViewModel: TabItemViewModel
    {
        private List<CardsBonusAccountsTrans> _trans;

        public CardsBonusDetailTabItemViewModel() : base(string.Empty)
        {;
        }

        public CardsBonusDetailTabItemViewModel(ClientKit client,string nameHeader) : base(nameHeader)
        {
            Client = client;
            BeginDate = DateTime.Now.Date.AddDays(-30);
            EndDate = DateTime.Now.Date;
            InitializeCommands();
            OkCommandMethod();
        }

        private ClientKit Client { get; set; }

        private void InitializeCommands()
        {
            OkCommand = new Command(m => OkCommandMethod());

            
        }

        private void OkCommandMethod()
        {
            if (BeginDate == null || EndDate == null || BeginDate > EndDate)
            {
                return;
            }

            Trans = DbPg.GetTransactions(Client.Client, BeginDate.Value, EndDate.Value).ToList();
        }

        public ICommand OkCommand { get; set; }

        public List<CardsBonusAccountsTrans> Trans
        {
            get { return _trans; }
            set
            {
                _trans = value; 
                base.OnPropertyChanged(nameof(Trans));
            }
        }

        public DateTime? BeginDate { get; set; }

        public DateTime? EndDate { get; set; }

    }
}
