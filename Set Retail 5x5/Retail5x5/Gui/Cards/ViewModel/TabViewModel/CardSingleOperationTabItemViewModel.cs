using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Set_Retail_5x5.Retail5x5.Model;
using Set_Retail_5x5.Retail5x5.Model.Data;

namespace Set_Retail_5x5.Retail5x5.Gui.Cards.ViewModel.TabViewModel
{
    public class CardSingleOperationTabItemViewModel: TabItemViewModel
    {
        public CardSingleOperationTabItemViewModel(ClientKit client, Set10ExchangeLoyCard card, string nameHeader) : base(nameHeader)
        {
        }
    }
}
