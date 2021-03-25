using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Set_Retail_5x5.Retail5x5.Gui.Cards.View.Converters
{
   

    public class TypeDetailOperationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var v = System.Convert.ToInt32(value);
            switch (v)
            {
                case 0: return "Начисление";
                case 2: return "Активация";
                case 6: return "Списание";
                default: return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
