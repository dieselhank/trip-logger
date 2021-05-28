using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TripLoggerServices;
using TripLoggerServices.Models;
using TripLoggerServicesTests.Helpers;

namespace TripLoggerServicesTests
{
    [TestClass]
    public class GetTripRequestTests
    {
        [TestMethod]
        public async Task Run_WithValidRequest_ReturnsTripEntry()
        {
            // arrange
            var req = TestFactory.CreateHttpRequest();
            var logger = TestFactory.CreateLogger(LoggerTypes.List);

            var trip = new TripEntry
            {
                Id = Guid.NewGuid(),
                Date = new DateTime(2020, 6, 1),
                Description = "Rec Ride",
                Distance = new DistanceDetail { Length = 26.45, Units = Units.Miles },
                TripFrom = "Home",
                TripTo = "Around"
            };

            // act
            var result = await GetTripRequest.Run(req, trip.Id.ToString(), trip, logger);

            // assert
            using(new AssertionScope())
            {
                result.Should().BeOfType<OkObjectResult>();
                var resultTrip = (TripEntry)((OkObjectResult)result).Value;
                resultTrip.Id.Should().Be(trip.Id);
            }
        }

        [TestMethod]
        public async Task Run_TripEntryNotFound_Returns404()
        {
            // arrange
            var req = TestFactory.CreateHttpRequest();
            var logger = TestFactory.CreateLogger(LoggerTypes.List);

            var tripId = Guid.NewGuid().ToString();
            TripEntry trip = null;

            // act
            var result = await GetTripRequest.Run(req, tripId, trip, logger);

            // assert
            using (new AssertionScope())
            {
                result.Should().BeOfType<NotFoundObjectResult>();
                var resultBody = (string)((NotFoundObjectResult)result).Value;
                resultBody.Should().Contain(tripId.ToString());
            }
        }
    }
}
