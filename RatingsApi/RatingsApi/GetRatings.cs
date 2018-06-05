using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace RatingsApi
{
    public static class GetRatings
    {
        [FunctionName("GetRatings")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get",
            Route = null)]HttpRequestMessage req,
            [DocumentDB(
                databaseName: "ratingsapi",
                collectionName: "ratings",
                ConnectionStringSetting = "CosmosDb")] DocumentClient client,
            TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            string userId = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "userId", true) == 0)
                .Value;

            if (userId == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, "Please provide a userId");
            }

            System.Uri collectionUri = UriFactory.CreateDocumentCollectionUri("ratingsapi", "ratings");
            IDocumentQuery<Rating> query = client.CreateDocumentQuery<Rating>(collectionUri)
                .Where(p => p.userId.ToString() == userId)
                .AsDocumentQuery();

            List<Rating> ratings = new List<Rating>();

            while (query.HasMoreResults)
            {
                foreach (Rating rating in await query.ExecuteNextAsync())
                {
                    ratings.Add(rating);
                }
            }

          

            return ratings.Count() == 0
                ? req.CreateResponse(HttpStatusCode.BadRequest, $"No Ratings for userId {userId}")
                : req.CreateResponse(HttpStatusCode.OK, ratings);
        }
    }
}
