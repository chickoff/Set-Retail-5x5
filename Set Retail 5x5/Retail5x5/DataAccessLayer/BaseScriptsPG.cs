using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using Set_Retail_5x5.Retail5x5.Model.Data;
using Set_Retail_5x5.Retail5x5.Model.SynchroData;

namespace Set_Retail_5x5.Retail5x5.DataAccessLayer
{
    public static class BaseScriptsPG
    {
        private static string _cardExist = "SELECT Guid as Set10Guid,numberfield,client_id as clientid FROM card_cards";
        private static string _cardBonusAccounts =
        @"
            SELECT ba.bonusaccountid,ba.enabled,ba.bonusaccounttypeid FROM cards_bonusaccounts ba
            JOIN card_internalcard_bonusaccount c_ba ON c_ba.bonusaccountid = ba.bonusaccountid
            JOIN card_cards c ON c.id = c_ba.cardid
        ";
        private static string _bonusAccounts = @"SELECT ba.bonusaccountid,ba.enabled,ba.bonusaccounttypeid FROM cards_bonusaccounts ba";           
        
        private static string _insertClient =
        @"
            INSERT INTO card_clients(id, auto, birthdate, childrenage, appartment, building, city, district, districtarea, house, other, region, street, zip, deleted, email, firstname, guid, iscompleted, lastchangedate, lastname, marital, middlename, mobileoperator, mobilephone, delivery, deliverydate, passnumber, passserie, phone, bymail, bysms, byphone, byemail, sendcatalog, sex, shopnumber, creationdate, bonusbalance, clienttype, receipt_feedback_means,smartphone_type,wants_e_card)
            VALUES (@id, @auto, @birthdate, @childrenage, @appartment, @building, @city, @district, @districtarea, @house, @other, @region, @street, @zip, @deleted, @email, @firstname, @guid, @iscompleted, @lastchangedate, @lastname, @marital, @middlename, @mobileoperator, @mobilephone, @delivery, @deliverydate, @passnumber, @passserie, @phone, @bymail, @bysms, @byphone, @byemail, @sendcatalog, @sex, @shopnumber, @creationdate, @bonusbalance, @clienttype, @ReceiptFeedbackMeans,@SmartphoneType,@WantsECard);
        ";

        private static string _updateClient = @"UPDATE card_clients SET birthdate=@birthdate, email = @email, firstname = @firstname, lastname = @lastname, middlename = @middlename, mobilephone = @mobilephone, sex = @sex WHERE guid = @guid;";


        //public static string InsertNewClientScript =>
        //    $@"
        //        INSERT INTO card_clients(
        //        id, 
        //        auto, 
        //        birthdate, 
        //        childrenage, 
        //        appartment, 
        //        building, 
        //        city, 
        //        district, 
        //        districtarea, 
        //        house, 
        //        other, 
        //        region, 
        //        street, 
        //        zip, 
        //        deleted, 
        //        email, 
        //        firstname, 
        //        guid, 
        //        iscompleted, 
        //        lastchangedate, 
        //        lastname, 
        //        marital, 
        //        middlename, 
        //        mobileoperator, 
        //        mobilephone, 
        //        delivery, 
        //        deliverydate, 
        //        passnumber, 
        //        phone, 
        //        bymail, 
        //        bysms, 
        //        byphone, 
        //        byemail, 
        //        sendcatalog, 
        //        sex, 
        //        shopnumber, 
        //        creationdate, 
        //        bonusbalance, 
        //        clienttype, 
        //        receipt_feedback_means)            
        //        VALUES(
        //        @Uid, --id, 
        //        false, --auto, 
        //        null, --birthdate, 
        //        null, --childrenage, 
        //        null, --appartment, 
        //        null, --building, 
        //        null, --city, 
        //        null, --district, 
        //        null, --districtarea, 
        //        null, --house, 
        //        null, --other, 
        //        null, --region, 
        //        null, --street, 
        //        null, --zip, 
        //        false, --deleted, 
        //        null, --email, 
        //        @Uid, --firstname, 
        //        @Uid, --guid, 
        //        true, --iscompleted, 
        //        NOW(),--lastchangedate, 
        //        @Uid, --lastname, 
        //        false, --marital, 
        //        null, --middlename, 
        //        null, --mobileoperator, 
        //        null, --mobilephone, 
        //        null, --delivery, 
        //        null, --deliverydate, 
        //        null, --passnumber, 
        //        null, --phone, 
        //        false, --bymail, 
        //        false, --bysms, 
        //        false, --byphone, 
        //        false, --byemail, 
        //        false, --sendcatalog, 
        //        0, --sex, 
        //        NULL, --shopnumber, 
        //        NOW(), --creationdate, 
        //        0, --bonusbalance, 
        //        0, --clienttype, 
        //        0 --receipt_feedback_means);";

        public static string InsertNewCardScript =>
            @"   
            INSERT INTO card_cards(
                id, activationdate, amount, barcode, createdate, deleted, expirationdate, 
                guid, numberfield, status, statusdescription, cardtype_id, client_id, 
                newcardtype_id, id_cardref, counterparty,debitor_type)
            VALUES(
                    @Id, @Activationdate, @Amount, @Barcode, @Createdate, @Deleted, @Expirationdate, 
                    @Guid, @Numberfield, @Status, @Statusdescription, @CardtypeId, @ClientId, 
                    @NewcardtypeId, @IdCardref, @Counterparty,@DebitorType);
            INSERT INTO card_internalcard_bonusaccount(bonusaccountid, cardid)
                    VALUES (@BonusAccountId,@Id);";

        public static string UpdateStatusCardScript => $@"UPDATE card_cards SET status = @status WHERE guid = @guid";

        public static string UpdateCardClientIdIEmptyScript => $@"UPDATE card_cards SET client_id = @clientid WHERE guid = @guid";

        public static string DeleteCardByNumCardScript => @"DELETE FROM card_internalcard_bonusaccount WHERE CardId = (SELECT ID FROM  card_cards WHERE NumberField = @Numberfield LIMIT 1); DELETE FROM card_cards WHERE NumberField = @Numberfield;";        

        public static string InsertNewBonusAccountScript =>
            @"
            INSERT INTO cards_bonusaccounts(bonusaccountid,enabled,bonusaccounttypeid) VALUES (@bonusaccountid, CAST(1 as BOOLEAN), CAST(2058 as BIGINT));
            INSERT INTO cards_bonusaccounts_balance_composition(id, ammount, finishdate, startdate, bonusaccounts_id, balancetype,lasttransactionid)
            VALUES
                (nextval('hibernate_sequence'), 0, NULL, NULL, @bonusaccountid, 0, NULL),
                (nextval('hibernate_sequence'), 0, NULL, NULL, @bonusaccountid, 1, NULL),
                (nextval('hibernate_sequence'), 0, NULL, NULL, @bonusaccountid, 2, NULL),
                (nextval('hibernate_sequence'), 0, NULL, NULL, @bonusaccountid, 3, NULL),
                (nextval('hibernate_sequence'), 0, NULL, NULL, @bonusaccountid, 4, NULL);";

        //INSERT INTO cards_bonusaccounts_balance(id,ammount,balancetype,lasttransactionid,bonusaccounts_id)
        //VALUES
        //    (nextval('hibernate_sequence'),0,0,NULL ,@bonusaccountid),
        //    (nextval('hibernate_sequence'),0,1,NULL ,@bonusaccountid),
        //    (nextval('hibernate_sequence'),0,2,NULL ,@bonusaccountid),
        //    (nextval('hibernate_sequence'),0,3,NULL ,@bonusaccountid),
        //    (nextval('hibernate_sequence'),0,4,NULL ,@bonusaccountid);"; 

        public static string InsertNewClientScript()
        {
            return _insertClient;
        }

        public static string UpdateClientScript()
        {
            return _updateClient;
        }

        public static string BonusByGuidScript { get; } =
            $@"
                SELECT
                X.clientGuid,
                X.bonusaccountid,
                bBalanceComp.balancetype,
                SUM(COALESCE(bBalanceComp.ammount,0)) as AmmountComposition
                FROM
                (
                SELECT cli.guid as clientGuid,ba.bonusaccountid
                FROM card_clients cli
                JOIN card_cards ca ON ca.client_id = cli.id
                JOIN card_internalcard_bonusaccount ba ON ba.cardid = ca.id
                WHERE cli.guid =  @Set10Guid
                GROUP BY cli.id,ba.bonusaccountid
                )X
                LEFT JOIN cards_bonusaccounts_balance_composition bBalanceComp ON bBalanceComp.bonusaccounts_id = X.bonusaccountid
                GROUP BY
                X.clientGuid,
                X.bonusaccountid,
                bBalanceComp.balancetype";

        public static string GetTransactionsScript { get; } =
            $@"
                    SELECT bonusaccounttransid, advertactguid, datefinishaction, datestartaction, 
                           operationdate, cashnum, checknum, createdate, shiftnum, shopnum, 
                           receiptsum, transactiontype, bonusaccountsid
                    FROM cards_bonusaccountstrans
                    WHERE bonusaccountsid = @BonusAccountId AND
                    CAST(operationdate AS date) BETWEEN @bDate AND @eDate
                    ORDER BY operationdate DESC;";

        public static string GetExistCardByGuidScript()
        {
            var sb = new StringBuilder();
            sb.AppendLine(_cardExist);
            sb.Append("WHERE Guid = @Set10Guid");
            return sb.ToString();
        }

        public static string GetExistCardByNumCardScript()
        {
            var sb = new StringBuilder();
            sb.AppendLine(_cardExist);
            sb.Append("WHERE numberfield = @CardNumber");
            return sb.ToString();
        }

        public static string GetCardBonusAccountsScript()
        {
            var sb = new StringBuilder();
            sb.AppendLine(_bonusAccounts);
            sb.Append("JOIN card_internalcard_bonusaccount c_ba ON c_ba.bonusaccountid = ba.bonusaccountid WHERE c_ba.cardid = @Set10Guid");
            return sb.ToString();
        }

        public static string GetBonusAccountsScript()
        {
            var sb = new StringBuilder();
            sb.AppendLine(_bonusAccounts);
            sb.Append("WHERE ba.bonusaccountid = @bonusaccountid");
            return sb.ToString();
        }

        public static string GetClientBonusAccountScript()
        {
            var sb = new StringBuilder();
            sb.AppendLine(_bonusAccounts);
            sb.Append("WHERE c.Guid = @Set10Guid");
            return sb.ToString();
        }

        public static string GetExistClientScript()
        {
            var sb = new StringBuilder();
            sb.AppendLine($@"SELECT id FROM card_clients WHERE guid = @numCli");
            return sb.ToString();
        }
    }
}
