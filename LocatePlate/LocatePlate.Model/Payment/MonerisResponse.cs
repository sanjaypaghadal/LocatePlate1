using Newtonsoft.Json;

namespace LocatePlate.Model.Payment
{
    public class MonerisResponse
    {
        [JsonProperty("response_order_id")]
        public string ResponseOrderId { get; set; }//: mhp21041230717p98
        [JsonProperty("date_stamp")]
        public string DateStamp { get; set; }//: 2021-02-11
        [JsonProperty("time_stamp")]
        public string TimeStamp { get; set; }//: 23:07:33

        [JsonProperty("bank_transaction_id")]
        public string BankTransactionId { get; set; }//: 660109300017800620
        [JsonProperty("charge_total")]
        public string ChargeTotal { get; set; }//: 20
        [JsonProperty("bank_approval_code")]
        public string BankApprovalCode { get; set; }//: 340605
        [JsonProperty("response_code")]
        public string ResponseCode { get; set; }//: 027

        [JsonProperty("iso_code")]
        public string IsoCode { get; set; }//: 01

        [JsonProperty("message")]
        public string Message { get; set; }//:APPROVED*                    =

        [JsonProperty("trans_name")]
        public string TransactionName { get; set; }//:purchase

        [JsonProperty("cardholder")]
        public string Cardholder { get; set; }//:Sarabjeet

        [JsonProperty("f4l4")]
        public string f4l4 { get; set; }//: 4352***3430
        [JsonProperty("card")]
        public string card { get; set; }//: V
        [JsonProperty("expiry_date")]
        public string ExpirDate { get; set; }//: 2212
        [JsonProperty("result")]
        public string Result { get; set; }//: 1

        [JsonProperty("rvar1")]
        public string Rvar1 { get; set; }//: 1
    }
}
