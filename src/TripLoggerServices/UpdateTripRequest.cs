using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using TripLoggerServices.Models;

namespace TripLoggerServices
{
    public static class UpdateTripRequest
    {
        [FunctionName("UpdateTripRequest")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "trips/{tripId}")] HttpRequest req,
            string tripId,
            [CosmosDB(
            databaseName: "TripLog",
            collectionName: "Trips",
            CreateIfNotExists = true,
            Id = "{tripId}",
            ConnectionStringSetting = "CosmosDBConnection")]TripEntry tripEntry,
            ILogger log)
        {
            try
            {
                log.LogInformation("Processing trip log entry update request.");

                if(tripEntry == null)
                {
                    return new NotFoundObjectResult($"Could not find trip entry with id: {tripId}");
                }

                // get raw request body
                var requestBody = new StreamReader(req.Body).ReadToEnd();
                var putRequest = JsonConvert.DeserializeObject<TripPut>(requestBody);

                if (string.IsNullOrWhiteSpace(putRequest.TripFrom)) return new BadRequestObjectResult($"{nameof(putRequest.TripFrom)} missing");
                if (string.IsNullOrWhiteSpace(putRequest.TripTo)) return new BadRequestObjectResult($"{nameof(putRequest.TripTo)} missing");
                if (string.IsNullOrWhiteSpace(putRequest.Description)) return new BadRequestObjectResult($"{nameof(putRequest.Description)} missing");
                if (putRequest.Distance == null) return new BadRequestObjectResult($"{nameof(putRequest.Distance)} missing");

                tripEntry.Date = putRequest.Date;
                tripEntry.Distance = putRequest.Distance;
                tripEntry.TripFrom = putRequest.TripFrom;
                tripEntry.TripTo = putRequest.TripTo;
                tripEntry.Description = putRequest.Description;
                tripEntry.CreatedOn = DateTime.UtcNow;
                tripEntry.ModifiedOn = DateTime.UtcNow;

                // return results
                return new OkObjectResult($"{tripId}");
            }
            catch (Exception exception)
            {
                // most likely errors to occur
                //  Error communicating with Cosmos DB
                log.LogError(exception, "Error processing request");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
