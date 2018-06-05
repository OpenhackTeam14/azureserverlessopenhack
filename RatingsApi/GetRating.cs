using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace RatingsApi
{
    public static class GetRating
    {
        [FunctionName("GetRating")]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
                        HttpRequestMessage req,
                        [DocumentDB(
                        databaseName: "ratingsapi",
                collectionName: "ratings",
                ConnectionStringSetting = "CosmosDb",
                Id = "{Query.id}")] Rating rating)
        {
            // parse query parameter
            var ratingId = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "ratingId", true) == 0)
                .Value;

            if (ratingId == null)
                return req.CreateResponse(HttpStatusCode.BadRequest, "Please pass the rating Id in the query string.");
        
            // get rating from cosmo db

            if (rating == null)
                throw new Exception("Could not find a rating with that Id.");

            var json = JsonConvert.SerializeObject(rating);
            return req.CreateResponse(HttpStatusCode.OK, json);
        }
    }
}
