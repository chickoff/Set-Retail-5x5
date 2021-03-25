using System.Runtime.InteropServices;

namespace Set_Retail_5x5.Retail5x5.Hardware.Printer
{


public class TSCLIBClass
{
    //public TSCLIBClass()
    //{
    //}

    [DllImport("TSCLIB.dll")]
    public static extern void openport(string PrinterName);

    [DllImport("TSCLIB.dll")]
    public static extern void closeport();

    [DllImport("TSCLIB.dll")]
    public static extern void sendcommand(string Command);

    [DllImport("TSCLIB.dll")]
    public static extern void setup(string LabelWidth, string LabelHeight,string Speed,string Density,string Sensor,string Vertical,string Offset);

    [DllImport("TSCLIB.dll")]
    public static extern void downloadpcx(string Filename, string ImageName);

    [DllImport("TSCLIB.dll")]
    public static extern void barcode(string X,string Y,string CodeType,string Height,string Readable,string Rotation,string Narrow, string Wide,string Code);

    [DllImport("TSCLIB.dll")]
    public static extern void printerfont(string X,string Y,string FontName,string Rotation,string Xmul,string Ymul,string Content);

    [DllImport("TSCLIB.dll")]
    public static extern void clearbuffer();

    [DllImport("TSCLIB.dll")]
    public static extern void printlabel(string NumberOfSet, string NumberOfCopoy);

    [DllImport("TSCLIB.dll")]
    public static extern void formfeed();

    [DllImport("TSCLIB.dll")]
    public static extern void nobackfeed();

    [DllImport("TSCLIB.dll")]
    public static extern void windowsfont(int X,int Y,int FontHeight,int Rotation,int FontStyle, int FontUnderline,string FaceName,string TextContect);




}


}