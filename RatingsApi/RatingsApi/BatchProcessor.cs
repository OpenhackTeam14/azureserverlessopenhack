using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.ServiceBus;
using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace RatingsApi
{
    public static class BatchProcessor
    {
        [FunctionName("BatchProcessor")]
        public static async Task Run([EventHubTrigger("team14", Connection = "EventHubConnectionAppSetting")]string[] eventHubMessages,
            [DocumentDB(
                databaseName: "ratingsapi",
                collectionName: "salebatches",
                ConnectionStringSetting = "CosmosDb")]IAsyncCollector<dynamic> document,
            TraceWriter log)
        {
            foreach (var item in eventHubMessages)
            {
                log.Info(item);
                await document.AddAsync(item);
            }
        }
    }
}
