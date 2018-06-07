using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;

namespace RatingsApi
{
    public static class CreateOrder
    {
        private static CloudStorageAccount account = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["OrderStorage"]);
        private static CloudBlobClient serviceClient = account.CreateCloudBlobClient();
        private static string endpointUrl = ConfigurationManager.AppSettings["cosmosDBAccountEndpoint"];
        private static string authorizationKey = ConfigurationManager.AppSettings["cosmosDBAccountKey"];
        private static DocumentClient client = new DocumentClient(new Uri(endpointUrl), authorizationKey);

        [FunctionName("CreateOrder")]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            var urls = req.Content.ReadAsAsync<List<Url>>().Result;
            dynamic orderHeaderFile = null;
            dynamic orderProductLineFile =null;
            dynamic orderLineItemFile = null;
            List<OrderHeaderDetail> myOrder = new List<OrderHeaderDetail>();

            foreach (var url in urls)
            {
                var fileType = url.url.Split('-')[1];
                if (fileType.Contains("OrderHeaderDetails"))
                {
                    var productCsvReader = GetFileContent(url.url);
                    productCsvReader.Configuration.RegisterClassMap<OrderHeaderDetailMap>();
                    orderHeaderFile = productCsvReader.GetRecords<OrderHeaderDetail>().ToList();
                }
                else if (fileType.Contains("ProductInformation"))
                {
                    var productCsvReader = GetFileContent(url.url);
                    productCsvReader.Configuration.RegisterClassMap<ProductLineMap>();
                    orderProductLineFile = productCsvReader.GetRecords<ProductLine>().ToList();
                }
                else if (fileType.Contains("OrderLine"))
                {
                    var productCsvReader = GetFileContent(url.url);
                    productCsvReader.Configuration.RegisterClassMap<OrderLineItemMap>();
                    orderLineItemFile = productCsvReader.GetRecords<OrderLineItem>().ToList();
                }

            }

            foreach (var detail in orderHeaderFile as IEnumerable<OrderHeaderDetail>)
            {
                myOrder.Add(detail);
                foreach (var order in orderLineItemFile as IEnumerable<OrderLineItem>)
                {
                    if (order.ponumber == detail.ponumber)
                    {
                        detail.lineItems.Add(order);
                        foreach (var product in orderProductLineFile as IEnumerable<ProductLine>)
                        {
                            if (order.productid == product.productid)
                            {
                                order.productname = product.productname;
                                order.productdescription = product.productdescription;
                            }
                        }
                    }
                }
            }

            return req.CreateResponse(HttpStatusCode.OK,  myOrder);
        }

        private static CsvReader GetFileContent(string url)
        {
            string filename = url.Split('/').Last();
            CloudBlobContainer container = serviceClient.GetContainerReference("orders");
            CloudBlob blob = container.GetBlobReference(filename);
            Stream headerStream = new MemoryStream();
            blob.DownloadToStreamAsync(headerStream).Wait();
            StreamReader headerReader = new StreamReader(headerStream);
            headerStream.Seek(0, SeekOrigin.Begin);
            return new CsvReader(headerReader);
        }
        
    }
}
