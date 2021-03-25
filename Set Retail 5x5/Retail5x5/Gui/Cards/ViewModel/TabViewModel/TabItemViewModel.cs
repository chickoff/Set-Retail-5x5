using System;
using Set_Retail_5x5.Retail5x5.Gui.Common;

namespace Set_Retail_5x5.Retail5x5.Gui.Cards.ViewModel.TabViewModel
{
    public class TabItemViewModel:BaseViewModel
    {
        public TabItemViewModel(string nameHeader)
        {
            NameHeader = nameHeader;
        }

        private string _nameHeader;
        public string NameHeader
        {
            get { return _nameHeader; }
            set
            {
                _nameHeader = value; 
                base.OnPropertyChanged(propertyName: nameof(NameHeader));
            }
        }

        
    }
}
