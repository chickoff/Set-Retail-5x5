using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Dapper;
using Set_Retail_5x5.Retail5x5.Common.Converters;
using Set_Retail_5x5.Retail5x5.Model;
using Set_Retail_5x5.Retail5x5.Model.Data;
using Set_Retail_5x5.Retail5x5.Model.ExternalData;

namespace Set_Retail_5x5.Retail5x5.DataAccessLayer
{
    public static class DbMs
    {

        public static SqlConnection GetConnection(SqlConnection dbCon = null)
        {
            var dbConnection = dbCon ?? new SqlConnection(ConnectionSettings.ConMsStr);
            if (!dbConnection.State.Equals(ConnectionState.Open)) dbConnection.Open();
            return dbConnection;
        }
        public static List<Set10ExchangeLoyCard> AllNewCards()
        {
            var categoryId = 3;
            var s = BaseScriptsMs.NewCardsScript();
            var dbConnection = new SqlConnection(ConnectionSettings.ConMsStr);
            return dbConnection.Query<Set10ExchangeLoyCard>(s, categoryId).AsEnumerable().ToList();
        }

        public static List<Set10ExchangeLoyClient> AllNewClients()
        {
            var categoryId = 3;
            var s = BaseScriptsMs.NewClientsScript();
            var dbConnection = new SqlConnection(ConnectionSettings.ConMsStr);
            return dbConnection.Query<Set10ExchangeLoyClient>(s, categoryId).AsEnumerable().ToList();
        }

        public static List<Set10ExchangeLoyClient> AllChangedClients(SqlConnection dbConnection = null)
        {
            dbConnection = GetConnection(dbConnection);
            return dbConnection.Query<Set10ExchangeLoyClient>(BaseScriptsMs.ChangesClientsScript()).AsEnumerable().ToList();
        }

        public static IEnumerable<Set10ExchangeLoyCard> GetCardsByClientUid(Guid uid, SqlConnection dbConnection = null)
        {
            dbConnection = GetConnection(dbConnection);
            var cards = dbConnection.Query<Set10ExchangeLoyCard>(BaseScriptsMs.CardsByClientUidScript(), new {uid})
                .AsEnumerable();
            return cards;
        }
        public static IEnumerable<Set10ExchangeLoyClient> GetClientByNumCard(string cardNumber, SqlConnection dbConnection = null)
        {
            dbConnection = GetConnection(dbConnection);
            var client = dbConnection.Query<Set10ExchangeLoyClient>(BaseScriptsMs.ClientByNumCardScript(), new { cardNumber })
                .AsEnumerable();
            return client;
        }

        

        public static List<Set10ExchangeLoyClientSexType> GetSexType()
        {
            var dbConnection = new SqlConnection(ConnectionSettings.ConMsStr);
            var sexTypes = dbConnection.Query<Set10ExchangeLoyClientSexType>(BaseScriptsMs.SexTypeScript).AsEnumerable()
                .ToList();
            return sexTypes;
        }

        public static List<Set10ExchangeLoyClientCategory> GetClientCategory()
        {
            var dbConnection = new SqlConnection(ConnectionSettings.ConMsStr);
            return dbConnection.Query<Set10ExchangeLoyClientCategory>(BaseScriptsMs.ClientCategoryScript).AsEnumerable()
                .ToList();
        }

        public static int ExistNumCard(string numCard)
        {
            var card12 = numCard.Length == 8 ? numCard.ToDec() : numCard;
            var dbConnection = new SqlConnection(ConnectionSettings.ConMsStr);
            return dbConnection.QuerySingle<int>(BaseScriptsMs.ExistNumCardScript, new {card12});
        }

        public static List<Set10ExchangeLoyClient> SearchClientInfo(string numCard)
        {
            var card12 = numCard.Length == 8 ? numCard.ToDec() : numCard;
            var dbConnection = new SqlConnection(ConnectionSettings.ConMsStr);
            return dbConnection.Query<Set10ExchangeLoyClient>(BaseScriptsMs.SearchClientInfoScript(), new {card12})
                .ToList();
        }

        public static List<Set10ExchangeLoyClient> SearchClientInfo(QueryForFindClientInfo query)
        {
            var queryDictionary = new Dictionary<string, object>();
            if (query.IsLastNameEnabled && !string.IsNullOrEmpty(query.LastName))
                queryDictionary.Add(nameof(query.LastName), query.LastName);
            if (query.IsFirstNameEnabled && !string.IsNullOrEmpty(query.FirstName))
                queryDictionary.Add(nameof(query.FirstName), query.FirstName);
            if (query.IsMiddleNameEnabled && !string.IsNullOrEmpty(query.MiddleName))
                queryDictionary.Add(nameof(query.MiddleName), query.MiddleName);
            if (query.IsMobilePhoneEnabled && !string.IsNullOrEmpty(query.FirstName))
                queryDictionary.Add(nameof(query.MobilePhone), query.MobilePhone);
            if (query.IsDateBirthEnabled && query.BirthDate != null)
                queryDictionary.Add(nameof(query.BirthDate), query.BirthDate);
            if (query.IsCategoryIdEnabled) queryDictionary.Add(nameof(query.CategoryId), query.CategoryId);
            var dbConnection = new SqlConnection(ConnectionSettings.ConMsStr);
            return dbConnection.Query<Set10ExchangeLoyClient>(BaseScriptsMs.SearchClientInfoScript(queryDictionary))
                .ToList();
        }

        public static ClientKit GetClientFullInfo(Set10ExchangeLoyClient client, SqlConnection dbConnection = null)
        {
            var clientFull = new ClientKit();
            clientFull.Client = client;
            clientFull.Cards = new ObservableCollection<Set10ExchangeLoyCard>(DbMs.GetCardsByClientUid(client.Uid, dbConnection));
           return clientFull;
        }

        public static void SaveChargeBonusByClient(List<ChargeOnByClient> bonus, DateTime chargeOnDate, string note)
        {
            using (var dbConnection = new SqlConnection(ConnectionSettings.ConMsStr))
            {
                dbConnection.Open();
                using (var dbTransaction = dbConnection.BeginTransaction())
                {
                    var sql = TableByClass(bonus.First().GetType(), "#TmpChargeBonus");
                    dbConnection.Execute(sql, transaction: dbTransaction);
                    foreach (var b in bonus)
                    {
                        dbConnection.Execute(BaseScriptsMs.ChargeBonusScript,
                            new {b.Id, b.Bonus}, dbTransaction);
                    }
                    try
                    {
                        dbTransaction.Commit();
                        dbConnection.Execute(BaseScriptsMs.AddBonusPackageByClientProc, new {date = chargeOnDate, note},
                            commandType: CommandType.StoredProcedure);
                    }
                    catch (Exception ex)
                    {
                        dbTransaction.Rollback();
                        throw new Exception("SaveChargeBonusByClient error!", ex);
                    }
                    finally
                    {
                        dbConnection.Close();
                    }
                }
            }
        }

        public static void SaveClientInfo(Set10ExchangeLoyClient client){
            using (var dbConnection = new SqlConnection(ConnectionSettings.ConMsStr))
            {
                    dbConnection.Open();
                    using (var tran = dbConnection.BeginTransaction())
                    {
                        if (client.IsNew)
                        {
                            dbConnection.Execute(BaseScriptsMs.InsertClientSript,
                                new
                                {
                                    client.Uid,
                                    client.Set10Guid,
                                    client.BonusAccountId,
                                    client.LastName,
                                    client.FirstName,
                                    client.MiddleName,
                                    client.BirthDate,
                                    client.MobilePhone,
                                    client.SexId,
                                    client.CategoryId,
                                    client.ChangeTypeId,
                                    client.Ldm
                                });
                        }
                        else if (!client.IsNew)
                        {
                            dbConnection.Execute(BaseScriptsMs.UpdateClientSript,
                                new
                                {
                                    client.Uid,
                                    client.LastName,
                                    client.FirstName,
                                    client.MiddleName,
                                    client.BirthDate,
                                    client.MobilePhone,
                                    client.SexId,
                                    client.CategoryId
                                });
                        }

                        try
                        {
                            tran.Commit();
                        }
                        catch (Exception e)
                        {
                            tran.Rollback();
                            
                            throw new Exception("SaveClientInfo error!", e);
                        }
                    }
            }
        }

        public static void SaveCard(Set10ExchangeLoyCard card)
        {
            var dbConnection = new SqlConnection(ConnectionSettings.ConMsStr);
            if (card.IsNew)
            {
                dbConnection.Execute(BaseScriptsMs.InsertCardScript,
                    new
                    {
                        card.Uid,
                        card.ClientUid,
                        card.Set10Guid,
                        card.Set10ClientGuid,
                        card.CardNumber,
                        card.CardHexNumber,
                        card.BonusAccountId,
                        card.IsBlocked
                    });
            }
            else if (!card.IsNew)
            {
                dbConnection.Execute(BaseScriptsMs.UpdateCardScript, new {card.Uid, card.IsBlocked});
            }
        }

        public static void UpdateClientKitIsLoadToSet10(Guid uid,bool isLoadToSet10)
        {
            using (var dbConnection = GetConnection())
            {
                using (var transaction = dbConnection.BeginTransaction())
                {
                    dbConnection.Execute(BaseScriptsMs.UpdateClientKitIsLoadToSet10Script, new { uid, isLoadToSet10 }, transaction);
                    transaction.Commit();
                }
            }
        }

        public static void SaveClientKit(ClientKit clientKit)
        {
            var xd = new XDocument(
                    new XElement("clientKit",
                        new XElement("Client",
                            new XElement(nameof(clientKit.Client.Uid), clientKit.Client.Uid),
                            new XElement(nameof(clientKit.Client.Set10Guid), clientKit.Client.Set10Guid),
                            new XElement(nameof(clientKit.Client.BonusAccountId), clientKit.Client.BonusAccountId),
                            new XElement(nameof(clientKit.Client.LastName), clientKit.Client.LastName),
                            new XElement(nameof(clientKit.Client.FirstName), clientKit.Client.FirstName),
                            new XElement(nameof(clientKit.Client.MiddleName), clientKit.Client.MiddleName),
                            new XElement(nameof(clientKit.Client.BirthDate), clientKit.Client.BirthDate),
                            new XElement(nameof(clientKit.Client.MobilePhone), clientKit.Client.MobilePhone),
                            new XElement(nameof(clientKit.Client.SexId), clientKit.Client.SexId),
                            new XElement(nameof(clientKit.Client.CategoryId), clientKit.Client.CategoryId),
                            new XElement(nameof(clientKit.Client.ChangeTypeId), clientKit.Client.ChangeTypeId),
                            new XElement(nameof(clientKit.Client.IsChanged), clientKit.Client.IsChanged),
                            new XElement(nameof(clientKit.Client.IsNew), clientKit.Client.IsNew),
                            new XElement(nameof(clientKit.Client.EMail), clientKit.Client.EMail)),
                            
                        new XElement("Cards",
                            clientKit.Cards.Select(card =>
                                new XElement("Card",
                                    new XElement(nameof(card.Uid), card.Uid),
                                    new XElement(nameof(card.ClientUid), card.ClientUid),
                                    new XElement(nameof(card.Set10Guid), card.Set10Guid),
                                    new XElement(nameof(card.Set10ClientGuid), card.Set10ClientGuid),
                                    new XElement(nameof(card.CardNumber), card.CardNumber),
                                    new XElement(nameof(card.CardHexNumber), card.CardHexNumber),
                                    new XElement(nameof(card.BonusAccountId), card.BonusAccountId),
                                    new XElement(nameof(card.IsBlocked), card.IsBlocked),
                                    new XElement(nameof(card.IsChanged), card.IsChanged),
                                    new XElement(nameof(card.IsNew), card.IsNew))))));
            xd.ToString();
            using (var dbConnection = GetConnection())
            {
                var p = new DynamicParameters();
                p.Add("@kit", xd.ToString());
                dbConnection.Execute("Set10Exchange_Loy_SaveClientKit", p, commandType: CommandType.StoredProcedure);
            }
        }

        public static void ChargeOnNewClient()
        {
            var dbConnection = new SqlConnection(ConnectionSettings.ConMsStr);
            dbConnection.Execute(BaseScriptsMs.ChargeOnNewClientScript,commandType:CommandType.StoredProcedure);
        }

        private static string TableByClass(Type type, string nameTable)
        {
            var fields = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var sb = new StringBuilder();
            sb.Append($"CREATE TABLE {nameTable} (");
            foreach (var f in fields)
            {
                var z = fields.First().Equals(f) ? string.Empty : ",";
                sb.Append($"{z}{f.Name} {GetSqlType(f.PropertyType)}");
            }
            sb.Append(")");
            return sb.ToString();
        }

        private static string GetSqlType(Type type)
        {
            if (type == typeof(string))
                return string.Format("{0}(max)", SqlDbType.NVarChar);
            else if (type == typeof(int))
                return SqlDbType.Int.ToString();
            else if (type == typeof(Guid))
                return SqlDbType.UniqueIdentifier.ToString();
            else if (type == typeof(Int16))
                return SqlDbType.SmallInt.ToString();
            else if (type == typeof(double))
                return SqlDbType.Float.ToString();
            else if (type == typeof(decimal))
                return SqlDbType.Money.ToString();
            else if (type == typeof(bool))
                return SqlDbType.Bit.ToString();
            else if (type == typeof(DateTime))
                return SqlDbType.DateTime.ToString();
            else if (type == typeof(Single))
                return SqlDbType.Float.ToString();
            else if (type == typeof(object))
                return string.Format("{0}(max)", SqlDbType.NVarChar);
            else if (type == typeof(long))
                return SqlDbType.BigInt.ToString();
            else throw new NotImplementedException();
        }
    }
}
