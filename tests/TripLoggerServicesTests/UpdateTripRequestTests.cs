using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using TripLoggerServices;
using TripLoggerServices.Models;
using TripLoggerServicesTests.Helpers;

namespace TripLoggerServicesTests
{
    [TestClass]
    public class UpdateTripRequestTests
    {
        [TestMethod]
        public async Task Run_WithValidRequest_ReturnsIdAndUpdatesDb()
        {
            // arrange
            var tripPut = new TripPut
            {
                Date = new DateTime(2020, 1, 1),
                Description = "Bike Commute",
                Distance = new DistanceDetail { Length = 5.6, Units = Units.Miles },
                TripFrom = "Home",
                TripTo = "SW"
            };
            var req = TestFactory.CreateHttpRequest(JsonConvert.SerializeObject(tripPut));
            var logger = TestFactory.CreateLogger(LoggerTypes.List);

            TripEntry tripEntry = new TripEntry
            {
                Id = Guid.NewGuid(),
                Date = new DateTime(2020, 6, 1),
                Description = "Rec Ride",
                Distance = new DistanceDetail { Length = 26.45, Units = Units.Miles },
                TripFrom = "Home",
                TripTo = "Around"
            };

            // act
            var result = await UpdateTripRequest.Run(req, tripEntry.Id.ToString(), tripEntry, logger);

            // assert
            using(new AssertionScope())
            {
                result.Should().BeOfType<OkObjectResult>();
                var resultBody = (string)((OkObjectResult)result).Value;
                resultBody.Should().Be(tripEntry.Id.ToString());
                tripEntry.Should().BeEquivalentTo(new TripEntry
                {
                    Id = tripEntry.Id,
                    Date = new DateTime(2020, 1, 1),
                    Description = "Bike Commute",
                    Distance = new DistanceDetail { Length = 5.6, Units = Units.Miles },
                    TripFrom = "Home",
                    TripTo = "SW"
                },
                config =>
                {
                    config.Excluding(x => x.CreatedOn).Excluding(x => x.ModifiedOn);
                    return config;
                });
            }
        }
    }
}
