using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Set_Retail_5x5.Retail5x5.Model;
using WSInternalCardsProcessingManagerProxy;

namespace Set_Retail_5x5.Retail5x5.DataAccessLayer
{
    public static class BaseScriptsMs
    {
        private static readonly string _cardsScript =
        @"SELECT
            ca.Uid
        ,	ca.ClientUid
        ,	ca.Set10Guid
        ,	ca.Set10ClientGuid
        ,	ca.CardNumber
        ,	ca.CardHexNumber
        ,   ca.BonusAccountId
        ,	ca.IsBlocked
        ,	ca.IsLoadToSet10
        ,	ca.LDM
        FROM AdvancedServices.dbo.Set10Exchange_Loy_Card ca
        JOIN AdvancedServices.dbo.Set10Exchange_Loy_Client cli ON cli.Uid = ca.ClientUid";

        private static readonly string _clientsScript =
        @"SELECT DISTINCT
            cli.Uid,
            cli.Set10Guid,
            cli.BonusAccountId,
            cli.LastName,
            cli.FirstName,
            cli.MiddleName,
            cli.BirthDate,
            cli.MobilePhone,
            cli.SexId,
            cli.CategoryId,
            cli.ChangeTypeId,
            cli.LDM,
            cli.EMail
        FROM AdvancedServices.dbo.Set10Exchange_Loy_Client cli
        JOIN AdvancedServices.dbo.Set10Exchange_Loy_Card ca ON ca.ClientUid = cli.Uid";

        public static string UpdateClientCategoryScript { get; } = @"
            UPDATE AdvancedServices.dbo.Set10Exchange_Loy_Client
            SET 
	            CategoryId = @CategoryId    
            ,	IsLoadToSet10 = 'false'
            ,	LDM = GetDate()
            WHERE Uid = @Uid
        ";
        public static string SexTypeScript { get; } =
            @"SELECT ID,Value,Description FROM AdvancedServices.dbo.Set10Exchange_Loy_ClientSexType";
        public static string ClientCategoryScript { get; } =
            @"SELECT Id, NameLong FROM AdvancedServices.dbo.Set10Exchange_Loy_ClientCategory";
        public static string ExistNumCardScript { get; } =
            @"SELECT TOP 1 COUNT(*) FROM dbo.Set10Exchange_Loy_Card ca WHERE ca.CardNumber = @numCard";
        public static string InsertClientSript { get; } =
            $@"INSERT INTO AdvancedServices.dbo.Set10Exchange_Loy_Client (Uid,Set10Guid,BonusAccountId,LastName,FirstName,MiddleName,BirthDate,MobilePhone,SexId,CategoryId,ChangeTypeId,IsLoadToSet10,LDM,EMail) VALUES (@Uid,@Set10Guid,@BonusAccountId,@LastName,@FirstName,@MiddleName,@BirthDate,@MobilePhone,@SexId,@CategoryId,@ChangeTypeId,'false',@LDM,@EMail)";

        public static string ChargeOnNewClientScript { get; } =
            $@"Set10Exchange_Loy_ChargeOnNewClients";
        public static string UpdateClientSript { get; } =
            $@"
                            UPDATE AdvancedServices.dbo.Set10Exchange_Loy_Client
                               SET 		
		                            LastName = @LastName
	                            ,	FirstName = @FirstName
	                            ,	MiddleName = @MiddleName
	                            ,	BirthDate = @BirthDate
	                            ,	MobilePhone = @MobilePhone
	                            ,	SexId = @SexId
	                            ,	CategoryId = @CategoryId
	                            ,	IsLoadToSet10 = 'FALSE'
	                            ,	LDM = GETDATE()
                                ,   EMail = @EMail
                             WHERE Uid = @Uid";
        public static string InsertCardScript { get; } =
            "INSERT INTO AdvancedServices.dbo.Set10Exchange_Loy_Card(Uid,ClientUid,Set10Guid,Set10ClientGuid,CardNumber,CardHexNumber,BonusAccountId,IsBlocked) VALUES(@Uid,@ClientUid,@Set10Guid,@Set10ClientGuid,@CardNumber,@CardHexNumber,@BonusAccountId, @IsBlocked)";
        public static string UpdateCardScript { get; } =
            "UPDATE AdvancedServices.dbo.Set10Exchange_Loy_Card SET IsBlocked = @IsBlocked,IsLoadToSet10 = 'false',LDM = getdate() WHERE Uid = @Uid";
        public static string UpdateClientKitIsLoadToSet10Script { get; } =
        @"UPDATE ca
        SET ca.IsLoadToSet10 = @IsLoadToSet10
        FROM AdvancedServices.dbo.Set10Exchange_Loy_Client cli
        JOIN AdvancedServices.dbo.Set10Exchange_Loy_Card ca ON ca.ClientUid = cli.Uid
        WHERE cli.Uid = @Uid

        UPDATE cli
        SET cli.IsLoadToSet10 = @IsLoadToSet10
        FROM AdvancedServices.dbo.Set10Exchange_Loy_Client cli
        WHERE cli.Uid = @Uid";
        public static string ChangesCardsScript()
        {
            var sb = new StringBuilder();
            sb.AppendLine(_cardsScript);
            sb.AppendLine(@"WHERE ca.IsLoadToSet10 = 'FALSE'");
            return sb.ToString();
        }

        public static string ClientByNumCardScript()
        {
            var sb = new StringBuilder();
            sb.AppendLine(_clientsScript);            
            sb.AppendLine(@"WHERE ca.CardNumber = @CardNumber");
            return sb.ToString();

        }

        public static string ChangesClientsScript()
        {
            var sb = new StringBuilder();
            sb.AppendLine(_clientsScript);
            //sb.AppendLine(@"WHERE (ca.CardNumber='118073071171')");           
            sb.AppendLine(@"WHERE (cli.IsLoadToSet10 = 'FALSE' OR ca.IsLoadToSet10 = 'FALSE')");
            return sb.ToString();
        }
        public static string NewCardsScript()
        {
            var sb = new StringBuilder();
            sb.AppendLine(_cardsScript);
            sb.AppendLine(@"WHERE cli.CategoryId = @CategoryId");
            return sb.ToString();
        }
        public static string NewClientsScript()
        {
            var sb = new StringBuilder();
            sb.AppendLine(_cardsScript);
            sb.AppendLine(@"WHERE cli.CategoryId = 3");
            return sb.ToString();
        }
        public static string CardsByClientUidScript()
        {
            var sb = new StringBuilder();
            sb.AppendLine(_cardsScript);
            sb.AppendLine(@"WHERE cli.Uid = @Uid");
            return sb.ToString();
        }
        public static string SearchClientInfoScript()
        {
            var sb = new StringBuilder();
            sb.AppendLine(_clientsScript);
            sb.AppendLine(@"WHERE ca.CardNumber = @card12");
            return sb.ToString();
        }
        public static string SearchClientInfoScript(Dictionary<string, object> queryDictionary)
        {
            var sb = new StringBuilder();
            sb.AppendLine(_clientsScript);
            if (queryDictionary.Any())
            {
                sb.Append("WHERE ");

                foreach (var d in queryDictionary)
                {
                    if (!Equals(queryDictionary.First(), d)) sb.Append("AND ");

                    sb.Append($"{d.Key} {ConditionByType(d.Value)} ");
                }
            }
            return sb.ToString();
        }
        private static string ConditionByType(object o)
        {
            var sb = new StringBuilder();

            if (o is string) return $"LIKE '%{o}%' ";

            if (o is DateTime?) return $"= '{(o as DateTime?).Value:yyyyMMdd}' ";

            if (o is int) return $"= {(int)o}";

            return string.Empty;
        }
        public static string ChargeBonusScript { get; } = "INSERT INTO #TmpChargeBonus(Id,Bonus) VALUES (@Id,@Bonus)";
        public static string AddBonusPackageByClientProc { get; } = "Set10Exchange_Loy_AddBonusPackageByClient";

        

    }
}
