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

public static class GetPets
{
    private static readonly string EndpointUri = Environment.GetEnvironmentVariable("CosmosDBEndpointUri");
    private static readonly string PrimaryKey = Environment.GetEnvironmentVariable("CosmosDBPrimaryKey");
    private static readonly string DatabaseId = Environment.GetEnvironmentVariable("CosmosDBDatabaseId");
    private static CosmosClient cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);
    private static Database database = cosmosClient.GetDatabase(DatabaseId);
    private static Container container = database.GetContainer("Pets");

    [FunctionName("GetPets")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "getPets")] HttpRequest req,
        ILogger log)
    {
        log.LogInformation("GetPets function processed a request.");

        var sqlQueryText = "SELECT * FROM c";
        QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
        FeedIterator<dynamic> queryResultSetIterator = container.GetItemQueryIterator<dynamic>(queryDefinition);

        List<dynamic> pets = new List<dynamic>();

        while (queryResultSetIterator.HasMoreResults)
        {
            FeedResponse<dynamic> currentResultSet = await queryResultSetIterator.ReadNextAsync();
            foreach (var pet in currentResultSet)
            {
                pets.Add(pet);
            }
        }

        return new OkObjectResult(pets);
    }
}
