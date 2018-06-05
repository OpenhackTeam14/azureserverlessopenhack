using System;
using System.IO;
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
    public static class CreateRating
    {
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
            Rating temp = new Rating();
            //document = temp;
            try
            {
                temp = req.Content.ReadAsAsync<Rating>().Result;
                temp.id = Guid.NewGuid();
            }
            catch (Exception ex)
            {
                response = req.CreateResponse(HttpStatusCode.InternalServerError, "Invalid Request body");
            }


            //Write to Cosmos
            document = temp;
            response = req.CreateResponse(HttpStatusCode.OK, temp);
            return response;
        }
    }
}
