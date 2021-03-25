using System;
using Set_Retail_5x5.Retail5x5.Common;
using Set_Retail_5x5.Retail5x5.Model.Data.IFace;

namespace Set_Retail_5x5.Retail5x5.Model.Data
{
    public class Set10ExchangeLoyCard : BaseModel
    {
        private bool _isBlocked;

        public Set10ExchangeLoyCard()
        {
        }

        public Set10ExchangeLoyCard(Guid? clientUid, string cardNumber)
        {
            Initialize();
            ClientUid = clientUid;
            CardNumber = cardNumber;
        }

        public Set10ExchangeLoyCard(Guid clientUid, long? set10ClientGuid, long bonusAccountId)
        {
            Initialize();
            ClientUid = clientUid;
            Set10ClientGuid = set10ClientGuid;
            BonusAccountId = bonusAccountId;
        }

        public Set10ExchangeLoyCard(Guid clientUid, long? set10ClientGuid, long? set10Guid, string cardNumber, long bonusAccountId)
        {
            Initialize();
            ClientUid = clientUid;
            Set10ClientGuid = set10ClientGuid;
            Set10Guid = set10Guid;
            CardNumber = cardNumber;
            BonusAccountId = bonusAccountId;
        }
        
        private void Initialize()
        {
            Uid = Guid.NewGuid();
            IsBlocked = false;
            IsLoadToSet10 = false;
            IsChanged = true;
            IsNew = true;
        }

        public Guid Uid { get; set; } 

        public Guid? ClientUid { get; set; } 

        public long? Set10Guid { get; set; }

        public long? Set10ClientGuid { get; set; }

        public string CardNumber { get; set; } 

        public string CardHexNumber { get; set; }

        public long? BonusAccountId { get; set; }

        public bool IsBlocked
        {
            get { return _isBlocked; }
            set
            {
                _isBlocked = value;
                base.OnPropertyChanged(nameof(IsBlocked));
        
            }
        }

        public bool IsLoadToSet10 { get; set; }
        
    }
}
