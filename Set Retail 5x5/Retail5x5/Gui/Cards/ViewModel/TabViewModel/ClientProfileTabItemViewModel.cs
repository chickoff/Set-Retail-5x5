using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Set_Retail_5x5.Retail5x5.DataAccessLayer;
using Set_Retail_5x5.Retail5x5.Model;
using Set_Retail_5x5.Retail5x5.Model.Data;

namespace Set_Retail_5x5.Retail5x5.Gui.Cards.ViewModel.TabViewModel
{
    public class ClientProfileTabItemViewModel: TabItemViewModel
    {
        private List<Set10ExchangeLoyClientSexType> _sexTypes;
        private List<Set10ExchangeLoyClientCategory> _clientCategories;


        public ClientProfileTabItemViewModel() : base("ClientProfileTabItemViewModel")
        {
        }
        public ClientProfileTabItemViewModel(ClientKit client, string nameHeader) : base(nameHeader)
        {
            Client = client;
            SexTypes = DbMs.GetSexType();
            ClientCategories = DbMs.GetClientCategory();
        }

        public ClientKit Client { get; set; }

        public List<Set10ExchangeLoyClientSexType> SexTypes
        {
            get => _sexTypes;
            set
            {
                _sexTypes = value; 
                base.OnPropertyChanged(nameof(SexTypes));
            }
        }

        public List<Set10ExchangeLoyClientCategory> ClientCategories
        {
            get => _clientCategories;
            set
            {
                _clientCategories = value; 
                base.OnPropertyChanged(nameof(ClientCategories));
            }
        }
    }
}
