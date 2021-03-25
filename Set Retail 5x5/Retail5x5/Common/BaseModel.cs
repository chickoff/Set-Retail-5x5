using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Set_Retail_5x5.Retail5x5.Common
{
    public class BaseModel:INotifyPropertyChanged
    {
        private bool _isChanged;
        public bool IsNew { get; set; }

        public bool IsChanged
        {
            get => _isChanged; set
            {
                _isChanged = value;
                OnPropertyChanged(nameof(IsChanged));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void ObjectSaved()
        {
            IsChanged = false;
            IsNew = false;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
             PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
