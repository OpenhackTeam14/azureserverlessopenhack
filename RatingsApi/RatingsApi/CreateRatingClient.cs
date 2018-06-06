using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System.Configuration;
using System;
using System.IO;
using Newtonsoft.Json;
using System.Management.Instrumentation;
using System.Text;

namespace RatingsApi
{

    public static class CreateRatingClient
    {
        private static string endpointUrl = ConfigurationManager.AppSettings["cosmosDBAccountEndpoint"];
        private static string authorizationKey = ConfigurationManager.AppSettings["cosmosDBAccountKey"];
        private static DocumentClient client = new DocumentClient(new Uri(endpointUrl), authorizationKey);

        [FunctionName("CreateRatingClient")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            Rating rating = null;

            try
            {
                rating = await req.Content.ReadAsAsync<Rating>();
            } catch (Exception ex)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, $"Invalid request Body: {ex.Message}");
            }

            try
            {
                GetObject("https://serverlessohlondonuser.azurewebsites.net/api/GetUser?userId=", rating.userId.ToString());

            }
            catch (Exception ex)
            {
                return req.CreateResponse(HttpStatusCode.NotFound, $"No user found with id: {rating.userId} {ex.Message}");

            }
            try
            {
                GetObject("https://serverlessohlondonproduct.azurewebsites.net/api/GetProduct?productId=", rating.productId.ToString());
            }
            catch (Exception ex)
            {
                return req.CreateResponse(HttpStatusCode.NotFound, $"No product found with id: {rating.productId} {ex.Message}");
            }

            try
            {
                await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri("ratingsapi", "ratings"), rating);
            } catch (Exception ex)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, $"Cannot create document in CosmosDB: {ex.Message}");
            }


            return rating == null
                ? req.CreateResponse(HttpStatusCode.BadRequest, "No rating object defined")
                : req.CreateResponse(HttpStatusCode.OK, rating);
        }

        private static void GetObject(string url, string id)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + id);
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
