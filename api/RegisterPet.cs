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

public static class RegisterPet
{
    private static readonly string EndpointUri = Environment.GetEnvironmentVariable("CosmosDBEndpointUri");
    private static readonly string PrimaryKey = Environment.GetEnvironmentVariable("CosmosDBPrimaryKey");
    private static readonly string DatabaseId = Environment.GetEnvironmentVariable("CosmosDBDatabaseId");
    private static readonly string ContainerId = "Pets";

    private static CosmosClient cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);
    private static Container container = cosmosClient.GetContainer(DatabaseId, ContainerId);

    [FunctionName("RegisterPet")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "registerPet")] HttpRequest req,
        ILogger log)
    {
        log.LogInformation("RegisterPet function processed a request.");

        try
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            if (string.IsNullOrEmpty(data?.petName) || string.IsNullOrEmpty(data?.petBreed))
            {
                return new BadRequestObjectResult("Invalid input. Both petName and petBreed are required.");
            }

            var pet = new
            {
                id = Guid.NewGuid().ToString(),
                name = data.petName,
                breed = data.petBreed
            };

            await container.CreateItemAsync(pet);

            return new OkObjectResult(new { message = "Pet registered successfully!" });
        }
        catch (Exception ex)
        {
            log.LogError($"An error occurred: {ex.Message}");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}
