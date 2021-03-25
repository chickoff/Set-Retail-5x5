using System;
using Set_Retail_5x5.Retail5x5.Common;
using Set_Retail_5x5.Retail5x5.Model.Data.IFace;

namespace Set_Retail_5x5.Retail5x5.Model.Data
{
    public class Set10ExchangeLoyClient : BaseModel,ICloneable
    {
        private string _lastName;
        private string _firstName;
        private string _middleName;
        private DateTime? _birthDate;
        private string _mobilePhone;
        private int _sexId;
        private int _categoryId;
        private string _eMail;

        public Set10ExchangeLoyClient()
        {
        }

        public Set10ExchangeLoyClient(Guid uid,long? set10Guid, long bonusAccountId, string lastName, string firstName, string middleName, DateTime? birthDate, string mobilePhone, int sexId, int categoryId, int changeTypeId, DateTime ldm,string email)
        {
            Initialize();
            Uid = uid;
            Set10Guid = set10Guid;
            BonusAccountId = bonusAccountId;
            LastName = lastName;
            FirstName = firstName;
            MiddleName = middleName;
            BirthDate = birthDate;
            MobilePhone = mobilePhone;
            SexId = sexId;
            CategoryId = categoryId;
            ChangeTypeId = changeTypeId;
            Ldm = ldm;
            EMail = email;
        }

        private void Initialize()
        {
            IsChanged = true;
            IsNew = true;
        }

        public Guid Uid { get; set; } 

        public long? Set10Guid { get; set; } 

        public long BonusAccountId { get; set; }

        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value; 
                base.OnPropertyChanged(nameof(LastName));
            }
        }

        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                base.OnPropertyChanged(nameof(FirstName));

            }
        }

        public string MiddleName
        {
            get => _middleName;
            set
            {
                _middleName = value;
                base.OnPropertyChanged(nameof(MiddleName));
            }
        }

        public DateTime? BirthDate
        {
            get { return _birthDate; }
            set
            {
                _birthDate = value;
                base.OnPropertyChanged(nameof(BirthDate));
            }
        }

        public string MobilePhone
        {
            get { return _mobilePhone; }
            set
            {
                _mobilePhone = value;
                base.OnPropertyChanged(nameof(MobilePhone));
            }
        }

        public int SexId
        {
            get => _sexId;
            set
            {
                _sexId = value;
                base.OnPropertyChanged(nameof(SexId));
            }
        }

        public int CategoryId
        {
            get { return _categoryId; }
            set
            {
                _categoryId = value;
                base.OnPropertyChanged(nameof(CategoryId));
            }
        }

        public int ChangeTypeId { get; set; } 

        public DateTime Ldm { get; set; }

        public string EMail
        {
            get
            {
                return _eMail;
            }
            set
            {
                _eMail = value;
                base.OnPropertyChanged(nameof(EMail));
            }
        }

        public object Clone()
        {
            return new Set10ExchangeLoyClient
            {
                Uid = Uid,
                Set10Guid = Set10Guid,
                BonusAccountId = BonusAccountId,
                LastName = LastName,
                FirstName = FirstName,
                MiddleName = MiddleName,
                BirthDate = BirthDate,
                MobilePhone = MobilePhone,
                SexId = SexId,
                CategoryId = CategoryId,
                ChangeTypeId = ChangeTypeId,
                Ldm = Ldm,
                IsChanged = IsChanged,
                IsNew = IsNew,
                EMail = EMail
            };
        }
    }
}
