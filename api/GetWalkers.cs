using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos;

public static class GetWalkers
{
    private static readonly string EndpointUri = "https://waqqly-dog.documents.azure.com:443/";
    private static readonly string PrimaryKey = "4q30iJfej5fqtVApeUNBLavsRUoRvrxMJcLsDm8ii6CJIDokdOBCqLOk5WiHFIvYieGB9FWrITxQACDbmWDJZQ==";
    private static readonly string DatabaseId = "WaqqlyDB";
    private static readonly string ContainerId = "DogWalkers";

    private static CosmosClient cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);
    private static Container container = cosmosClient.GetContainer(DatabaseId, ContainerId);

    [FunctionName("GetWalkers")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "getWalkers")] HttpRequest req,
        ILogger log)
    {
        log.LogInformation("GetWalkers function processed a request.");

        var query = new QueryDefinition("SELECT * FROM c");
        var iterator = container.GetItemQueryIterator<dynamic>(query);
        var results = new List<dynamic>();

        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            results.AddRange(response.ToList());
        }

        return new OkObjectResult(results);
    }
}
