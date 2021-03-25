using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32;
using Set_Retail_5x5.Retail5x5.Common;
using Set_Retail_5x5.Retail5x5.DataAccessLayer;
using Set_Retail_5x5.Retail5x5.Model.ExternalData;

namespace Set_Retail_5x5.Retail5x5.Gui.Cards.ViewModel.TabViewModel
{
    public class ChargeOnBalanceByClientFormFileTabItemViewModel : TabItemViewModel
    {
        private string _selectedFilePath;
        private List<ChargeOnByClient> _chargeOnList;

        public ChargeOnBalanceByClientFormFileTabItemViewModel() : base(string.Empty)
        {
        }
        public ChargeOnBalanceByClientFormFileTabItemViewModel(string nameHeader) : base(nameHeader)
        {
            InitializeCommands();
            SelectedChargeOnDate = DateTime.Now.Date;
        }

        private void InitializeCommands()
        {
            OpenFileCommand = new Command(m => OpenFileCommandMethod());
            LoadListCommand = new Command(m => LoadListCommandMethod());
        }

        private void LoadListCommandMethod()
        {
            DbMs.SaveChargeBonusByClient(ChargeOnList, SelectedChargeOnDate, Note);
        }

        private void OpenFileCommandMethod()
        {
            var openFile = new OpenFileDialog();
            openFile.FileOk += OpenFile_FileOk;
            openFile.Multiselect = false;
            openFile.ShowDialog();
        }

        private void OpenFile_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var dialog = sender as OpenFileDialog;
            if (dialog != null)
            {
                SelectedFilePath = dialog.FileName;
            }
            ChargeOnList = ExternalFilesLa.OpenSourseFile(SelectedFilePath);
        }

        public ICommand OpenFileCommand { get; set; }
        public ICommand LoadListCommand { get; set; }
        public string SelectedFilePath
        {
            get { return _selectedFilePath; }
            set
            {
                _selectedFilePath = value; 
                base.OnPropertyChanged(nameof(SelectedFilePath));
            }
        }
        public DateTime SelectedChargeOnDate { get; set; }
        public string Note { get; set; }
        public List<ChargeOnByClient> ChargeOnList
        {
            get { return _chargeOnList; }
            set
            {
                _chargeOnList = value; 
                base.OnPropertyChanged(nameof(ChargeOnList));
            }
        }
    }
}
