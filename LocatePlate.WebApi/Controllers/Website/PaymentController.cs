using LocatePlate.Model.Payment;
using LocatePlate.Model.RestaurantDomain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace LocatePlate.WebApi.Controllers.Website
{
    [Route("payment")]
    public class PaymentController : BaseController
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _contextAccessor;
        public PaymentController(HttpClient client, IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            this._client = client;
            this._contextAccessor = contextAccessor;
        }

        [HttpGet("open")]
        public async Task<IActionResult> open(Booking Booking)
        {
            //var values = new Dictionary<string, string>
            //{
            //    { "ps_store_id", "CEHT9tore3" },
            //    { "hpp_key", "hpUPPH55ZVJI" },
            //    { "charge_total", $"{Booking.TotalPrice}" },
            //    { "rvar1", $"{System.Guid.NewGuid()}" }
            //};



            ////var content = new FormUrlEncodedContent(values);

            ////var response = await this._client.PostAsync("https://esqa.moneris.com/HPPDP/index.php", content);

            ////var responseString = await response.Content.ReadAsStringAsync();
            ////responseString = responseString.Replace("/HPPDP", "https://esqa.moneris.com/HPPDP");
            ////responseString = responseString.Replace("hprequest.php", "https://esqa.moneris.com/hprequest.php");


            //return View("HostedPaymentPage", values);

            //values.Add("responseString", responseString);
            //return View("HostedPaymentPage", values);

            var values = new Dictionary<string, string>
            {
                { "ssl_merchant_id", "009392" },
                { "ssl_user_id", "webpage" },
                { "ssl_pin", "WZ3B7NG8ISWOXU2XJBFR1X1WT7KFB2YVP6B9ZKGSDLXWIK4XWA8UZ6LP6OQIPF38" },
                { "ssl_transaction_type", "ccgettoken"},
                { "ssl_amount", $"{Booking.TotalPrice}"},
                { "orderid", $"{Booking.Id}"},
                { "billid", $"{Booking.BillId}"}
            };

            var content = new FormUrlEncodedContent(values);

            var response = await this._client.PostAsync("https://api.demo.convergepay.com/hosted-payments/transaction_token", content);

            var responseString = await response.Content.ReadAsStringAsync();
            return Redirect("https://api.demo.convergepay.com/hosted-payments?ssl_txn_auth_token=" + responseString);
        }

        [HttpPost("success")]
        public IActionResult Index(MonerisResponse monerisResponse)
        {
            //save
            return View();
        }

        [HttpGet("PaymentSucess")]
        public IActionResult PaymentSucess()
        {
            return View("PaymentResponse");
        }

        [HttpGet("PaymentFailed")]
        public IActionResult PaymentFailed()
        {
            return View("PaymentResponse", 2);
        }
    }
}