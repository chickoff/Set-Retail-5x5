using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Set_Retail_5x5.Retail5x5.Model.Data;

namespace Set_Retail_5x5.Retail5x5.Model.SynchroData
{
   public class CardsCompositions
    {
        public CardsCompositions(Set10ExchangeLoyCard masterCard, Set10CardExist cardById, Set10CardExist cardByNumCard)
        {
            MasterCard = masterCard;
            CardById = cardById;
            CardByNumCard = cardByNumCard;
        }

        public Set10ExchangeLoyCard MasterCard { get; set; }
        public Set10CardExist CardById { get; set; }
        public Set10CardExist CardByNumCard { get; set; }
    }
}
