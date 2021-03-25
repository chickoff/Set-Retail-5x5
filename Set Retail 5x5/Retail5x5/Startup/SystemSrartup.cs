using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Set_Retail_5x5.Retail5x5.Global;
using Set_Retail_5x5.Set10.FlexMod;

namespace Set_Retail_5x5.Retail5x5.Startup
{
    public class SystemSrartup
    {
        public SystemSrartup()
        {
            Set10AuthStart();
        }


        public void Set10AuthStart()
        {
            GlobalObjects.Set10AuthToken = new Set10CentrumAuth("192.168.5.213", "8090", "/SetXRMI/messagebroker/amf");

            GlobalObjects.Set10AuthToken.Connect();
        }
    }
}
