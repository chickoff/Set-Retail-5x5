
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Metadata.Edm;
using FluorineFx.ServiceBrowser.Sql;
using Set_Retail_5x5.Retail5x5.Common;
using Set_Retail_5x5.Retail5x5.DataAccessLayer;
using Set_Retail_5x5.Retail5x5.Model.Data.IFace;
using Set_Retail_5x5.Retail5x5.Model.SynchroData;
using Set_Retail_5x5.Retail5x5.Common.Converters;

namespace Set_Retail_5x5.Retail5x5.Model.Data
{
   public class ClientKit: BaseModel
    {
        private ObservableCollection<Set10ExchangeLoyCard> _cards;
        private Set10ExchangeLoyClient _client;
       

        public Set10ExchangeLoyClient Client
        {
            get => _client;
            set
            {
                _client = value;
                _client.PropertyChanged += ObjectPropertyChanged;
            }
        }

        public ObservableCollection<Set10ExchangeLoyCard> Cards
        {
            get => _cards;
            set
            {
                _cards = value;
                _cards.CollectionChanged += Cards_CollectionChanged;
                foreach (var card in _cards)
                {
                    card.PropertyChanged += ObjectPropertyChanged;
                }
            }
        }

        public ObservableCollection<Set10ExchangeLoyAmmounts> Ammounts { get; set; }

        public List<CardsBonusaccounts> BonusAccounts { get; set; }

        private void ObjectPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsChanged") return;
            ((BaseModel) sender).IsChanged = true;
            IsChanged = true;
        }

        private void Cards_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (Set10ExchangeLoyCard n in e.NewItems)
            {
                n.PropertyChanged += ObjectPropertyChanged;
            }
            IsChanged = true;
        }

        private void PreSave()
        {
            if (_client.Set10Guid == null) _client.Set10Guid = DbPg.GetHibernateSequenceNextVal();
            if (_client.FirstName == null) _client.FirstName = _client.Set10Guid.ToString();
            if (_client.LastName == null) _client.LastName = _client.Set10Guid.ToString();
            if (_client.MiddleName == null) _client.MiddleName = string.Empty;

            foreach (var card in _cards)
            {
                if (card.Set10ClientGuid == null) card.Set10ClientGuid = _client.Set10Guid;
                if (card.CardHexNumber == null) card.CardHexNumber = card.CardNumber.ToHex();
                if (card.Set10Guid == null) card.Set10Guid = DbPg.GetHibernateSequenceNextVal();
                if (card.ClientUid == null) card.ClientUid = _client.Uid;
            }
        }

        public void Save()
        {
            PreSave();

            DbMs.SaveClientKit(this);
            this.ObjectSaved();
            Client.ObjectSaved();
            foreach (var card in Cards)
            {
                card.ObjectSaved();
            }
        }
    }
}
