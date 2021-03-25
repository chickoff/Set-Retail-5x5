using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Data;

namespace Set_Retail_5x5.Retail5x5.Gui.Common
{
    public class BaseTabItem : TabItem,INotifyPropertyChanged
    {
        public BaseTabItem()
        {
            var content = new ContentControl();
            content.Content = new CheckBox();
            this.Content = content;
        }
        

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
