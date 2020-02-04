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
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "trips")] HttpRequest req,
            [CosmosDB(
                databaseName: "TripLog",
                collectionName: "Trips",
                CreateIfNotExists = true,
                ConnectionStringSetting = "CosmosDBConnection")]IAsyncCollector<TripEntry> trips,
            ILogger log)
        {
            try
            {
                log.LogInformation("Processing mile log entry request.");

                // get raw request body
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var postRequest = JsonConvert.DeserializeObject<TripPost>(requestBody);

                if (string.IsNullOrWhiteSpace(postRequest.TripFrom)) return new BadRequestObjectResult($"{nameof(postRequest.TripFrom)} missing");
                if (string.IsNullOrWhiteSpace(postRequest.TripTo)) return new BadRequestObjectResult($"{nameof(postRequest.TripTo)} missing");
                if (string.IsNullOrWhiteSpace(postRequest.Description)) return new BadRequestObjectResult($"{nameof(postRequest.Description)} missing");
                if (postRequest.Distance == null) return new BadRequestObjectResult($"{nameof(postRequest.Distance)} missing");

                // create id
                var tripId = Guid.NewGuid();

                // create trip cosmosDb model
                var entry = new TripEntry
                {
                    Id = tripId,
                    Date = postRequest.Date,
                    IsRoundTrip = postRequest.IsRoundTrip,
                    Distance = postRequest.Distance,
                    TripFrom = postRequest.TripFrom,
                    TripTo = postRequest.TripTo,
                    Description = postRequest.Description,
                };

                // create in cosmosDb
                await trips.AddAsync(entry);

                // return results
                return new OkObjectResult($"{tripId}");
            }
            catch (Exception exception)
            {
                // most likely errors to occur
                //  Error communicating with 3rd party service
                //  Error communicating with Cosmos DB
                log.LogError(exception, "Error processing request");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}