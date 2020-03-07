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
    public static class LogTripRequest
    {
        [FunctionName("LogTripRequest")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "trips")] HttpRequest req,
            [CosmosDB(
                databaseName: "TripLog",
                collectionName: "Trips",
                CreateIfNotExists = true,
                ConnectionStringSetting = "CosmosDBConnection")]out TripEntry tripEntry,
            ILogger log)
        {
            tripEntry = null;

            try
            {
                log.LogInformation("Processing trip log entry request.");

                // get raw request body
                var requestBody = new StreamReader(req.Body).ReadToEnd();
                var postRequest = JsonConvert.DeserializeObject<TripPost>(requestBody);

                if (string.IsNullOrWhiteSpace(postRequest.TripFrom)) return new BadRequestObjectResult($"{nameof(postRequest.TripFrom)} missing");
                if (string.IsNullOrWhiteSpace(postRequest.TripTo)) return new BadRequestObjectResult($"{nameof(postRequest.TripTo)} missing");
                if (string.IsNullOrWhiteSpace(postRequest.Description)) return new BadRequestObjectResult($"{nameof(postRequest.Description)} missing");
                if (postRequest.Distance == null) return new BadRequestObjectResult($"{nameof(postRequest.Distance)} missing");

                // create id
                var tripId = Guid.NewGuid();

                // create trip cosmos db model
                tripEntry = new TripEntry
                {
                    Id = tripId,
                    Date = postRequest.Date,
                    Distance = postRequest.Distance,
                    TripFrom = postRequest.TripFrom,
                    TripTo = postRequest.TripTo,
                    Description = postRequest.Description,
                };

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
