using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Set_Retail_5x5.Retail5x5.Hardware.Printer
{
    public static class Printer
    {
        public static void Calibration()
        {
            TSCLIBClass.openport("TSC TTP-245C");
            TSCLIBClass.clearbuffer();
            TSCLIBClass.sendcommand("GAPDETECT 240, 24");
            TSCLIBClass.sendcommand("FEED 55");
            TSCLIBClass.sendcommand("CUT");
            TSCLIBClass.closeport();
        }

        public static void Print(string firstCard, string secondCard, string idCustomer)
        {
            TSCLIBClass.openport("TSC TTP-245C");
            TSCLIBClass.clearbuffer();
            TSCLIBClass.sendcommand("SIZE 80 mm,30 mm");
            TSCLIBClass.sendcommand("GAP 3 mm,0");
            TSCLIBClass.sendcommand("FEED 180");
            TSCLIBClass.sendcommand($"TEXT 200,0,\"3\",0,1,1,\"{firstCard}\"");
            TSCLIBClass.sendcommand($"TEXT 200,75,\"3\",0,1,1,\"{secondCard}\"");
            TSCLIBClass.sendcommand($"TEXT 210,117,\"3\",0,1,1,\"{"Client " + idCustomer}\"");
            TSCLIBClass.printlabel("1", "1");
            TSCLIBClass.sendcommand("FEED 155");
            TSCLIBClass.sendcommand("CUT");
            TSCLIBClass.closeport();
           
        }
    }
}
