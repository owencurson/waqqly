using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace waqqly.Function
{
    public static class HttpTrigger1
    {
        [FunctionName("HttpTrigger1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "registerPet")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request for pet registration.");

            // Parse the request body
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            // Extract pet name and breed from the request body
            string petName = data?.petName;
            string petBreed = data?.petBreed;

            if (string.IsNullOrEmpty(petName) || string.IsNullOrEmpty(petBreed))
            {
                return new BadRequestObjectResult("Please provide both petName and petBreed in the request body.");
            }

            // Here you can write code to interact with your Cosmos DB to save the pet details
            // For example, you can use Azure Cosmos DB SDK to insert a new document into your Cosmos DB container
            // Remember to replace this placeholder code with actual implementation

            // Return a success message
            return new OkObjectResult("Pet registered successfully!");
        }
    }
}

