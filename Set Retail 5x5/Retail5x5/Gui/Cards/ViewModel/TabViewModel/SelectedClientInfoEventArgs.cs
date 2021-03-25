using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Set_Retail_5x5.Retail5x5.Model;
using Set_Retail_5x5.Retail5x5.Model.Data;

namespace Set_Retail_5x5.Retail5x5.Gui.Cards.ViewModel.TabViewModel
{
    public class SelectedClientInfoEventArgs
    {
        public SelectedClientInfoEventArgs(Set10ExchangeLoyClient clientInfo)
        {
            Client = clientInfo;
        }

        public Set10ExchangeLoyClient Client { get; set; }
    }
}
