using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;

public static class GetWalkers
{
    private static readonly string EndpointUri = Environment.GetEnvironmentVariable("CosmosDBEndpointUri");
    private static readonly string PrimaryKey = Environment.GetEnvironmentVariable("CosmosDBPrimaryKey");
    private static readonly string DatabaseId = Environment.GetEnvironmentVariable("CosmosDBDatabaseId");
    private static CosmosClient cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);
    private static Database database = cosmosClient.GetDatabase(DatabaseId);
    private static Container container = database.GetContainer("DogWalkers");

    [FunctionName("GetWalkers")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "getWalkers")] HttpRequest req,
        ILogger log)
    {
        log.LogInformation("GetWalkers function processed a request.");

        var sqlQueryText = "SELECT * FROM c";
        QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
        FeedIterator<dynamic> queryResultSetIterator = container.GetItemQueryIterator<dynamic>(queryDefinition);

        List<dynamic> walkers = new List<dynamic>();

        while (queryResultSetIterator.HasMoreResults)
        {
            FeedResponse<dynamic> currentResultSet = await queryResultSetIterator.ReadNextAsync();
            foreach (var walker in currentResultSet)
            {
                walkers.Add(walker);
            }
        }

        return new OkObjectResult(walkers);
    }
}
