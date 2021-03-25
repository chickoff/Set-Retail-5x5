using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Set_Retail_5x5.Retail5x5.Common;
using Set_Retail_5x5.Retail5x5.DataAccessLayer;
using Set_Retail_5x5.Retail5x5.Model.Data;
using Set_Retail_5x5.Retail5x5.Model.SynchroData;

namespace Set_Retail_5x5.Retail5x5.Gui.Cards.ViewModel.TabViewModel
{
    public class Set10ManagementTabItemViewModel: TabItemViewModel
    {

        public Set10ManagementTabItemViewModel() : base(string.Empty)
        {
        }
        public Set10ManagementTabItemViewModel(string nameHeader) : base(nameHeader)
        {
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            SyncCommand = new Command(m => SyncCommandMethod());

            ChargeOnNewClientCommand = new Command(m => ChargeOnNewClientCommandMethod());
        }

        private void ChargeOnNewClientCommandMethod()
        {
            DbMs.ChargeOnNewClient();
        }

        private void SyncCommandMethod()
        {
            var sync = new Set10Synchronization();
            sync.Go();           
        }

        public ICommand SyncCommand { get; set; }

        public ICommand ChargeOnNewClientCommand { get; set; }
    }
}
