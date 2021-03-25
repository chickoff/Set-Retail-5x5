using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Set_Retail_5x5.Retail5x5.Common;

namespace Set_Retail_5x5.Retail5x5.Model.SynchroData
{
    public class Set10CardExist:BaseModel
    {
        public long Set10Guid { get; set; }
        public string NumberField { get; set; }

        public long? Clientid { get; set; }
    }
}
