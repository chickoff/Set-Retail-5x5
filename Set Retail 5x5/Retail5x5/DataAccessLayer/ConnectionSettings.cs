using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Set_Retail_5x5.Retail5x5.DataAccessLayer
{
    public static class ConnectionSettings
    {
        public static string ConMsStr { get; } =
            "Server=192.168.5.9;User Id=sa;Password=cgjhnkjnj80$;Database=AdvancedServices;";

        public static string ConPgStrDbSet { get; } =
            "Server=192.168.5.36;Port=5432;User Id=postgres;Password=postgres;Database=set;";
    }
}
