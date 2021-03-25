using System;
using System.Windows.Input;

namespace Set_Retail_5x5.Retail5x5.Common
{
    public class Command : ICommand
    {
        #region Constructor

        public Command(Action<object> action)
        {
            ExecuteDelegate = action;
        }

        public Command(Action<object> action, Predicate<object> predicate)
        {
            ExecuteDelegate = action;

            CanExecuteDelegate = predicate;
        }

        #endregion


        #region Properties

        public Predicate<object> CanExecuteDelegate { get; set; }
        public Action<object> ExecuteDelegate { get; set; }

        #endregion


        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            if (CanExecuteDelegate != null)
            {
                return CanExecuteDelegate(parameter);
            }

            return true;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            if (ExecuteDelegate != null)
            {
                ExecuteDelegate(parameter);
            }
        }

        #endregion
    }
}
