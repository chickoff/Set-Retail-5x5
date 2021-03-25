using System;
using Set_Retail_5x5.Retail5x5.Common;
using Set_Retail_5x5.Retail5x5.Gui.Common;

namespace Set_Retail_5x5.Retail5x5.Model
{
    public class QueryForFindClientInfo : BaseModel
    {
        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public DateTime? BirthDate { get; set; }

        public string MobilePhone { get; set; }

        public int CategoryId { get; set; }

        public bool IsLastNameEnabled { get; set; } = true;

        public bool IsFirstNameEnabled { get; set; } = true;

        public bool IsMiddleNameEnabled { get; set; } = false;

        public bool IsMobilePhoneEnabled { get; set; } = false;

        public bool IsDateBirthEnabled { get; set; } = true;

        public bool IsCategoryIdEnabled { get; set; } = false;
    }
}
