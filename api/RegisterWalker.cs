using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos;

public static class RegisterWalker
{
    private static readonly string EndpointUri = Environment.GetEnvironmentVariable("CosmosDBEndpointUri");
    private static readonly string PrimaryKey = Environment.GetEnvironmentVariable("CosmosDBPrimaryKey");
    private static readonly string DatabaseId = Environment.GetEnvironmentVariable("CosmosDBDatabaseId");
    private static readonly string ContainerId = "DogWalkers";

    private static CosmosClient cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);
    private static Container container = cosmosClient.GetContainer(DatabaseId, ContainerId);

    [FunctionName("RegisterWalker")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "registerWalker")] HttpRequest req,
        ILogger log)
    {
        log.LogInformation("RegisterWalker function processed a request.");

        try
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            if (string.IsNullOrEmpty(data?.walkerName) || string.IsNullOrEmpty(data?.walkerLocation))
            {
                return new BadRequestObjectResult("Invalid input. Both walkerName and walkerLocation are required.");
            }

            var walker = new
            {
                id = Guid.NewGuid().ToString(),
                name = data.walkerName,
                location = data.walkerLocation
            };

            await container.CreateItemAsync(walker, new PartitionKey(walker.id));

            return new OkObjectResult(new { message = "Walker registered successfully!" });
        }
        catch (Exception ex)
        {
            log.LogError($"An error occurred: {ex.Message}");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}
