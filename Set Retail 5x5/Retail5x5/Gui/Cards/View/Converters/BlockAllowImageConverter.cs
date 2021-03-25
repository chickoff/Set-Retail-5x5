using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Set_Retail_5x5.Retail5x5.Gui.Cards.View.Converters
{

    public class BlockAllowImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch ((bool?)value)
            {
                case true: return "/Retail5x5/icons/Lock.ico";
                default: return "/Retail5x5/icons/clean.png";

            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }


}
