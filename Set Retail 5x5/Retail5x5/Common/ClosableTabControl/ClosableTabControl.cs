using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Set_Retail_5x5.Retail5x5.Common.ClosableTabControl
{
   public class ClosableTabControl : TabControl
    {
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ClosableTabItem();
        }
    }
}
