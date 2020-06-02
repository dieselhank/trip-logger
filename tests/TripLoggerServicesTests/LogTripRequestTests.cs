using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using TripLoggerServices;
using TripLoggerServices.Models;
using TripLoggerServicesTests.Helpers;

namespace TripLoggerServicesTests
{
    [TestClass]
    public class LogTripRequestTests
    {
        [TestMethod]
        public void Run_WithValidRequest_ReturnsIdAndSavesToDb()
        {
            // arrange
            var tripPost = new TripPost
            {
                Date = new DateTime(2020, 1, 1),
                Description = "Bike Commute",
                Distance = new DistanceDetail { Length = 5.6, Units = Units.Miles },
                TripFrom = "Home",
                TripTo = "SW"
            };
            var req = TestFactory.CreateHttpRequest(JsonConvert.SerializeObject(tripPost));
            var logger = TestFactory.CreateLogger(LoggerTypes.List);

            TripEntry tripEntry = null;

            // act
            var result = LogTripRequest.Run(req, out tripEntry, logger);

            // assert
            result.Should().BeOfType<OkObjectResult>();
            var resultBody = (string)((OkObjectResult)result).Value;
            resultBody.Should().NotBeNullOrEmpty();
            tripEntry.Should().NotBeNull();
            tripEntry.Id.Should().Be(resultBody);
            tripEntry.Description.Should().Be("Bike Commute");
        }
    }
}
