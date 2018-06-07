using System;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Microsoft.ApplicationInsights;

namespace RatingsApi
{
    public static class CreateRating
    {

        private static readonly HttpClient client = new HttpClient();
        private static TelemetryClient telemetry = new TelemetryClient();


        [FunctionName("CreateRating")]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequestMessage req,
            [DocumentDB(
                databaseName: "ratingsapi",
                collectionName: "ratings",
                ConnectionStringSetting = "CosmosDb")]out dynamic document,
            TraceWriter log)
        {
            //Validation
            HttpResponseMessage response = null;
            document = null;
            Rating temp = new Rating();
            //document = temp;
            try
            {
                temp = req.Content.ReadAsAsync<Rating>().Result;
                temp.id = Guid.NewGuid();
            }
            catch (Exception ex)
            {
                return req.CreateResponse(HttpStatusCode.InternalServerError, "Invalid Request body");
            }

            try
            {
                GetObject("https://serverlessohlondonuser.azurewebsites.net/api/GetUser?userId=", temp.userId.ToString());

            }
            catch (Exception ex)
            {
                return req.CreateResponse(HttpStatusCode.NotFound, $"No user found with id: {temp.userId}");

            }
            try
            {
                GetObject("https://serverlessohlondonproduct.azurewebsites.net/api/GetProduct?productId=", temp.productId.ToString());
            }
            catch (Exception ex)
            {
                return req.CreateResponse(HttpStatusCode.NotFound, $"No product found with id: {temp.productId}");
            }

            //Get Sentiment Score
            var sentimentRequestBody = new { documents = new[] { new { language = "en", id = temp.id, text = temp.userNotes } } };
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("ContentType", "application/json");
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", System.Configuration.ConfigurationManager.AppSettings["CognitiveApiKey"]);

            var sentimentResponse = client.PostAsJsonAsync("https://northeurope.api.cognitive.microsoft.com/text/analytics/v2.0/sentiment", sentimentRequestBody).Result;
            var sentiments = JsonConvert.DeserializeObject<SentimentScores>(sentimentResponse.Content.ReadAsStringAsync().Result);

            temp.sentimentScore = sentiments.documents[0].score;
            var sample = new Microsoft.ApplicationInsights.DataContracts.MetricTelemetry();
            sample.Name = "SentimentScore";
            sample.Sum = sentiments.documents[0].score;
            telemetry.TrackMetric(sample);


            //Write to Cosmos
            document = temp;
            response = req.CreateResponse(HttpStatusCode.OK, temp);
            return response;
        }

        private static void GetObject(string url, string id)
        {
            url = url + id;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                WebResponse userResponse = request.GetResponse();
                using (Stream responseStream = userResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    var result = JsonConvert.DeserializeObject(reader.ReadToEnd());
                    if (result == null)
                    {
                        throw new InstanceNotFoundException();
                    }
                }
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                }
                throw;
            }
        }
    }
}
