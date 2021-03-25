using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Set_Retail_5x5.Retail5x5.Common.ClosableTabControl
{
   public class ClosableTabItem : TabItem
    {
        public static readonly DependencyProperty CloseItCommandProperty = DependencyProperty.Register("CloseItCommand", typeof(ICommand), typeof(ClosableTabItem), new PropertyMetadata(null));
        public ICommand CloseItCommand
        {
            get { return (ICommand)GetValue(CloseItCommandProperty); }
            set
            {
                SetValue(CloseItCommandProperty, value); 
                
            }
        }
    }
}
