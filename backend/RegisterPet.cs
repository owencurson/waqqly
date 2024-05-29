using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos;
using System.IO;
using System;
using System.Threading.Tasks;

public static class RegisterPet
{
    private static readonly string EndpointUri = Environment.GetEnvironmentVariable("CosmosDBEndpointUri");
    private static readonly string PrimaryKey = Environment.GetEnvironmentVariable("CosmosDBPrimaryKey");
    private static CosmosClient cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);
    private static Database database = cosmosClient.GetDatabase(Environment.GetEnvironmentVariable("CosmosDBDatabaseId"));
    private static Container container = database.GetContainer("Pets");

    [FunctionName("RegisterPet")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "registerPet")] HttpRequest req,
        ILogger log)
    {
        log.LogInformation("RegisterPet function processed a request.");
        
        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        dynamic data = JsonConvert.DeserializeObject(requestBody);
        
        var pet = new { id = Guid.NewGuid().ToString(), name = data.petName, breed = data.petBreed };
        await container.CreateItemAsync(pet);

        return new OkObjectResult(new { message = "Pet registered successfully!" });
    }
}
