using System;

namespace Set_Retail_5x5.Retail5x5.Model.SynchroData
{
    public class CardCards 
    {

        public CardCards()
        {

        }

        public CardCards(long id, DateTime? activationdate, long? amount, string barcode, DateTime? createdate, bool deleted, DateTime? expirationdate, long? guid, string numberfield, int status, string statusdescription, long? cardtypeId, long? clientId, long? newcardtypeId, long? idCardref, string counterparty,string debitorType)
        {
            Id = id;
            Activationdate = activationdate;
            Amount = amount;
            Barcode = barcode;
            Createdate = createdate;
            Deleted = deleted;
            Expirationdate = expirationdate;
            Guid = guid;
            Numberfield = numberfield;
            Status = status;
            Statusdescription = statusdescription;
            CardtypeId = cardtypeId;
            ClientId = clientId;
            NewcardtypeId = newcardtypeId;
            IdCardref = idCardref;
            Counterparty = counterparty;
            DebitorType = debitorType;


        }

        public long Id { get; set; } 

        public DateTime? Activationdate { get; set; } 

        public long? Amount { get; set; } 

        public string Barcode { get; set; } 

        public DateTime? Createdate { get; set; } 

        public bool Deleted { get; set; } 

        public DateTime? Expirationdate { get; set; } 

        public long? Guid { get; set; } 

        public string Numberfield { get; set; } 

        public int Status { get; set; } 

        public string Statusdescription { get; set; } 

        public long? CardtypeId { get; set; } 

        public long? ClientId { get; set; } 

        public long? NewcardtypeId { get; set; } 

        public long? IdCardref { get; set; } 

        public string Counterparty { get; set; }

        public string DebitorType { get; set; }
        
    }
}
