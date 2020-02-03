using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using TripLoggerServices.Models;

namespace MileLoggerServices
{
    public static class LogTripRequest
    {
        [FunctionName("LogTripRequest")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "miles")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Processing mile log entry request.");

            // get raw request body
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var postRequest = JsonConvert.DeserializeObject<TripPost>(requestBody);

            if (string.IsNullOrWhiteSpace(postRequest.TripFrom)) return new BadRequestObjectResult($"{nameof(postRequest.TripFrom)} missing");
            if (string.IsNullOrWhiteSpace(postRequest.TripTo)) return new BadRequestObjectResult($"{nameof(postRequest.TripTo)} missing");
            if (string.IsNullOrWhiteSpace(postRequest.Description)) return new BadRequestObjectResult($"{nameof(postRequest.Description)} missing");
            if (postRequest.Distance == null) return new BadRequestObjectResult($"{nameof(postRequest.Distance)} missing");

            // create id
            var tripId = Guid.NewGuid();

            return new OkObjectResult($"{tripId}");
        }
    }
}
