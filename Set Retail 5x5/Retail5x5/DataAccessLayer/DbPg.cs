using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using Set_Retail_5x5.Retail5x5.Model.Data;
using Set_Retail_5x5.Retail5x5.Model.SynchroData;


namespace Set_Retail_5x5.Retail5x5.DataAccessLayer
{
    public static class DbPg
    {
        public static NpgsqlConnection GetConnection(NpgsqlConnection dbCon = null)
        {
            return dbCon ?? new NpgsqlConnection(ConnectionSettings.ConPgStrDbSet);
        }

        public static void InsertClient(CardClients client, NpgsqlConnection dbConnection, IDbTransaction t)
        {
            dbConnection.Execute(BaseScriptsPG.InsertNewClientScript(), new { client.Id, client.Auto, client.Birthdate, client.Childrenage, client.Appartment, client.Building, client.City, client.District, client.Districtarea, client.House, client.Other, client.Region, client.Street, client.Zip, client.Deleted, client.Email, client.Firstname, client.Guid, client.Iscompleted, client.Lastchangedate, client.Lastname, client.Marital, client.Middlename, client.Mobileoperator, client.Mobilephone, client.Delivery, client.Deliverydate, client.Passnumber, client.Passserie, client.Phone, client.Bymail, client.Bysms, client.Byphone, client.Byemail, client.Sendcatalog, client.Sex, client.Shopnumber, client.Creationdate, client.Bonusbalance, client.Clienttype, client.ReceiptFeedbackMeans, client.SmartphoneType, client.WantsECard }, t);
        }

        public static void UpdateClient(CardClients client, NpgsqlConnection dbConnection, IDbTransaction t)
        {
            dbConnection.Execute(BaseScriptsPG.UpdateClientScript(), new { client.Id, client.Auto, client.Birthdate, client.Childrenage, client.Appartment, client.Building, client.City, client.District, client.Districtarea, client.House, client.Other, client.Region, client.Street, client.Zip, client.Deleted, client.Email, client.Firstname, client.Guid, client.Iscompleted, client.Lastchangedate, client.Lastname, client.Marital, client.Middlename, client.Mobileoperator, client.Mobilephone, client.Delivery, client.Deliverydate, client.Passnumber, client.Passserie, client.Phone, client.Bymail, client.Bysms, client.Byphone, client.Byemail, client.Sendcatalog, client.Sex, client.Shopnumber, client.Creationdate, client.Bonusbalance, client.Clienttype, client.ReceiptFeedbackMeans }, t);
        }

        public static void InsertCard(CardCards card,long? bonusAccountId, NpgsqlConnection dbConnection, IDbTransaction t)
        {
            dbConnection.Execute(BaseScriptsPG.InsertNewCardScript,
                new
                {
                    card.Id,
                    card.Activationdate,
                    card.Amount,
                    card.Barcode,
                    card.Createdate,
                    card.Deleted,
                    card.Expirationdate,
                    card.Guid,
                    card.Numberfield,
                    card.Status,
                    card.Statusdescription,
                    card.CardtypeId,
                    card.ClientId,
                    card.NewcardtypeId,
                    card.IdCardref,
                    card.Counterparty,
                    card.DebitorType,
                    bonusAccountId
                }, t);
        }

        public static void InsertBonusAccount(long id, long bonusAccountId, NpgsqlConnection dbConnection, IDbTransaction t)
        {
            dbConnection.Execute(BaseScriptsPG.InsertNewBonusAccountScript, new {id,bonusAccountId}, t);
        }

        public static void UpdateCard(CardCards card, NpgsqlConnection dbConnection, IDbTransaction t)
        {
            dbConnection.Execute(BaseScriptsPG.UpdateStatusCardScript, new {card.Guid, card.Status, card.ClientId}, t);
        }

        public static void UpdateCardClientIdIEmpty(CardCards card, NpgsqlConnection dbConnection, IDbTransaction t)
        {
            dbConnection.Execute(BaseScriptsPG.UpdateCardClientIdIEmptyScript, new { card.Guid, card.ClientId }, t);
        }        

        public static void DeleteCardByNumCard(CardCards card, NpgsqlConnection dbConnection, IDbTransaction t)
        {
            dbConnection.Execute(BaseScriptsPG.DeleteCardByNumCardScript, new { Numberfield = card.Numberfield }, t);
        }

        public static IEnumerable<Set10ExchangeLoyAmmounts> GetBonus(Set10ExchangeLoyClient client, NpgsqlConnection dbCon = null)
        {
            var dbConnection = GetConnection(dbCon);
            return dbConnection.Query<Set10ExchangeLoyAmmounts>(BaseScriptsPG.BonusByGuidScript,new { client.Set10Guid });
        }

        public static long GetHibernateSequenceNextVal(NpgsqlConnection dbCon = null)
        {
            var dbConnection = GetConnection(dbCon);
            return Convert.ToInt64(dbConnection.ExecuteScalar("SELECT nextval('hibernate_sequence')"));
        }

        public static IEnumerable<CardsBonusAccountsTrans> GetTransactions(Set10ExchangeLoyClient client, DateTime bDate, DateTime eDate, NpgsqlConnection dbCon = null)
        {
            var dbConnection = GetConnection(dbCon);
            var result = dbConnection.Query<CardsBonusAccountsTrans>(BaseScriptsPG.GetTransactionsScript, new { client.BonusAccountId, bDate, eDate });
            return result;
        }

        public static Set10CardExist GetExistCardByGuid(Set10ExchangeLoyCard card, NpgsqlConnection dbCon = null)
        {
            var dbConnection = GetConnection(dbCon);
            return dbConnection.Query<Set10CardExist>(BaseScriptsPG.GetExistCardByGuidScript(), new {card.Set10Guid}).FirstOrDefault();
        }

        public static Set10CardExist GetExistCardByNumCard(Set10ExchangeLoyCard card, NpgsqlConnection dbCon = null)
        {
            var dbConnection = GetConnection(dbCon);
            return dbConnection.Query<Set10CardExist>(BaseScriptsPG.GetExistCardByNumCardScript(), new { card.CardNumber }).FirstOrDefault();
        }

        public static bool GetExistNumClient(long? numCli, NpgsqlConnection dbCon = null)
        {
            var dbConnection = GetConnection(dbCon);
            var result = dbConnection.Query(BaseScriptsPG.GetExistClientScript(),new { numCli });
            return result != null && result.Any();
        }

        public static List<CardsBonusaccounts> GetBonusAccounts(long? bonusAccountId, NpgsqlConnection dbCon = null)
        {
            var dbConnection = GetConnection(dbCon);
            return dbConnection.Query<CardsBonusaccounts>(BaseScriptsPG.GetBonusAccountsScript(),new { bonusAccountId }).ToList() ;
        }

        public static List<CardsBonusaccounts> GetCardBonusAccounts(long? set10Guid, NpgsqlConnection dbCon = null)
        {
            var dbConnection = GetConnection(dbCon);
            return dbConnection.Query<CardsBonusaccounts>(BaseScriptsPG.GetCardBonusAccountsScript(), new { set10Guid }).ToList();
        }
    }
}