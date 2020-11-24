using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using TripLoggerServices.Models;

namespace TripLoggerServices
{
    public static class GetTripRequest
    {
        [FunctionName("GetTripRequest")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "trips/{tripId}")] HttpRequest req,
            string tripId,
            [CosmosDB(
            databaseName: "TripLog",
            collectionName: "Trips",
            CreateIfNotExists = true,
            Id = "{tripId}",
            ConnectionStringSetting = "CosmosDBConnection")]TripEntry tripEntry,
            ILogger log)
        {
            log.LogInformation("Processing trip log entry get request.");

            if (tripEntry == null)
            {
                return new NotFoundObjectResult($"Could not find trip entry with id: {tripId}");
            }

            return new OkObjectResult(tripEntry);
        }
    }
}
