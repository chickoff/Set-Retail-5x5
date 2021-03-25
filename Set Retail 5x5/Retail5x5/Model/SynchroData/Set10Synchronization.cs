using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using Set_Retail_5x5.Retail5x5.Common;
using Set_Retail_5x5.Retail5x5.DataAccessLayer;
using Set_Retail_5x5.Retail5x5.Model.Data;

namespace Set_Retail_5x5.Retail5x5.Model.SynchroData
{
    public class Set10Synchronization:BaseModel
    {
        public void Go()
        {
            var dbConnection = DbPg.GetConnection();
            var dbMsConnection = DbMs.GetConnection();
            var clients = DbMs.AllChangedClients();
            foreach (var cli in clients)
            {
                var  kit = DbMs.GetClientFullInfo(cli, dbMsConnection);

                try
                {
                    Sync(kit, dbConnection);
                    DbMs.UpdateClientKitIsLoadToSet10(cli.Uid, true);
                }
                catch (Exception)
                {
                    
                }    
            }
        }

        private void Sync(ClientKit kit,NpgsqlConnection dbConnection)
        {
            var isExistClient = DbPg.GetExistNumClient(kit.Client.Set10Guid);
            var cardComposition = kit.Cards.Select(card => new CardsCompositions(card, DbPg.GetExistCardByGuid(card, dbConnection), DbPg.GetExistCardByNumCard(card, dbConnection))).ToList();
              
            dbConnection = DbPg.GetConnection(dbConnection);
            if (!dbConnection.FullState.Equals(ConnectionState.Open)) dbConnection.Open();
            var transaction = dbConnection.BeginTransaction();
            try
            {
                var bAccounts = DbPg.GetBonusAccounts(kit.Client.BonusAccountId, dbConnection);
                if (bAccounts == null || bAccounts.Count == 0)
                {
                    BonusAccounts(kit.Client, transaction, dbConnection);
                }
                Client(kit.Client, isExistClient, transaction, dbConnection);
                if (cardComposition.Any())
                {
                    Cards(kit.Client,cardComposition, transaction, dbConnection);
                }
                transaction.Commit();
            }
            catch (Exception e)
            {
                transaction.Rollback();
                throw  new Exception("Sync kit exception.", e);
            }
        }

        private void Client(Set10ExchangeLoyClient client,bool isExistClient, IDbTransaction transaction, NpgsqlConnection dbConnection)
        {
            var cli = new CardClients
            {
                Id = client.Set10Guid.Value,
                Auto = false,
                Birthdate = client.BirthDate,
                Childrenage = string.Empty,
                Appartment = string.Empty,
                Building = string.Empty,
                City = string.Empty,
                District = string.Empty,
                Districtarea = string.Empty,
                House = string.Empty,
                Other = string.Empty,
                Region = string.Empty,
                Street = string.Empty,
                Zip = string.Empty,
                Deleted = false,
                Email = client.EMail,
                Firstname = client.FirstName,
                Guid = client.Set10Guid,
                Iscompleted = true,
                Lastchangedate = DateTime.Now,
                Lastname = client.LastName,
                Marital = false,
                Middlename = client.MiddleName,
                Mobileoperator = string.Empty,
                Mobilephone = client.MobilePhone,
                Delivery = string.Empty,
                Deliverydate = null,
                Passnumber = string.Empty,
                Passserie = string.Empty,
                Phone = string.Empty,
                Bymail = false,
                Bysms = false,
                Byphone = false,
                Byemail = false,
                Sendcatalog = false,
                Sex = 0,
                Shopnumber = string.Empty,
                Creationdate = DateTime.Now,
                Bonusbalance = 0,
                Clienttype = 0,
                ReceiptFeedbackMeans = 0,
                SmartphoneType = null,
                WantsECard = false                
            };

            if (isExistClient)
                DbPg.UpdateClient(cli, dbConnection, transaction);
            else
                DbPg.InsertClient(cli, dbConnection, transaction);
        }

        private void Cards(Set10ExchangeLoyClient client, List<CardsCompositions> cardComposition, IDbTransaction transaction, NpgsqlConnection dbConnection)
        {
            foreach (var card in cardComposition)
            {
                var set10Card = new CardCards(card.MasterCard.Set10Guid.Value, DateTime.Now.Date, 0, null, DateTime.Now.Date, false, DateTime.Parse("01-01-2021"), card.MasterCard.Set10Guid.Value, card.MasterCard.CardNumber, (card.MasterCard.IsBlocked == false ? 0 : 2), string.Empty, 2059, client.Set10Guid, null, null, null,String.Empty);
                if (card.CardById == null && card.CardByNumCard != null)
                {
                    DbPg.DeleteCardByNumCard(set10Card, dbConnection, transaction);
                }
                if (card.CardById == null)
                    DbPg.InsertCard(set10Card, card.MasterCard.BonusAccountId,dbConnection, transaction);
                else
                    DbPg.UpdateCard(set10Card, dbConnection, transaction);

                if (card.CardById?.Clientid == null)
                    DbPg.UpdateCardClientIdIEmpty(set10Card, dbConnection, transaction);
            }
        }

        //private bool CheckIdentityCard(CardsCompositions cardCompot)
        //{
        //    if (cardCompot.CardById.)
        //    return false;
        //}

        private void BonusAccounts(Set10ExchangeLoyClient client, IDbTransaction transaction, NpgsqlConnection dbConnection)
        {
            DbPg.InsertBonusAccount(client.BonusAccountId, client.BonusAccountId, dbConnection, transaction);
        }
    }
}
