using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace LocatePlate.WebApi.Extensions
{
    public static class FCMHelper
    {
        //content title = message to be shown
        //content type=message type eg. new order
        // content=message to be shown
        //objid= 0 
        //tokenortopic = firebase notification token
        //isAndroid=true
        //isTopic= false
        public static async void SendPushNotification(string contentTitle, string contentType, string content, Guid userId, int restaurantId, int bookingId, string tokenOrTopic = "", bool isAndroid = true, bool isTopic = false)
        {
            var url = new Uri("https://fcm.googleapis.com/fcm/send");

            //locate plate
            string appKey = "AAAAeMU6APY:APA91bGk6KPclCr12Yx1MzAwEin7rjhb7jL9FdQyI3jScCyxfWzAX9NyTGg3My-ZzfyIookOobKpIxQhYCRiD51nuf5wIFiKVV1Hr97GcVrmCx-5viIseZ50Ab0etzwo9BJrMsCd6X3k";

            var dataObj = new
            {
                Title = contentTitle,
                Content = content,
                UserId = userId,
                RestaurantId = restaurantId,
                BookingIdrId = bookingId,
                ContentType = contentType
            };

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.TryAddWithoutValidation(
                        "Authorization", "key=" + appKey);

                    HttpResponseMessage result;
                    if (isTopic)
                    {
                        var MainTopicObj = new
                        {
                            data = dataObj,
                            to = $"/topics/{tokenOrTopic}"
                        };
                        string MainTopicObjJson = JsonConvert.SerializeObject(MainTopicObj);
                        Task.WaitAll(client.PostAsync(url,
                             new StringContent(MainTopicObjJson, Encoding.Default, "application/json")));

                    }
                    else
                    {
                        if (isAndroid)
                        {
                            var androidMainObj = new
                            {
                                data = dataObj,
                                to = tokenOrTopic
                            };
                            string androidJson = JsonConvert.SerializeObject(androidMainObj);
                            Task.WaitAll(client.PostAsync(url,
                             new StringContent(androidJson, Encoding.Default, "application/json")));
                        }
                        else
                        {
                            var notificationObj = new
                            {
                                body = content,
                                title = contentTitle,
                                sound = "default",
                                content_available = true,
                            };
                            var iosMainObj = new
                            {
                                data = dataObj,
                                notification = notificationObj,
                                to = tokenOrTopic
                            };
                            string iosJson = JsonConvert.SerializeObject(iosMainObj);
                            Task.WaitAll(client.PostAsync(url,
                                                new StringContent(iosJson, Encoding.Default, "application/json")));
                        }
                    }


                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to send GCM message:");
                Console.Error.WriteLine(e.StackTrace);
            }

        }
    }
}
