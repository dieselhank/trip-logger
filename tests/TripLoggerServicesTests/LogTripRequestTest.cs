using Microsoft.Azure.WebJobs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;
using TripLoggerServices.Models;
using TripLoggerServicesTests.Helpers;
using Newtonsoft.Json;
using TripLoggerServices;
using System.Threading;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace TripLoggerServicesTests
{
    [TestClass]
    public class LogTripRequestTest
    {
        private Mock<IAsyncCollector<TripEntry>> _tripCollector;

        [TestInitialize]
        public void Init()
        {
            _tripCollector = new Mock<IAsyncCollector<TripEntry>>();
        }

        [TestMethod]
        public async Task Run_WithValidRequest_ReturnsIdAndSavesToDb()
        {
            // arrange
            var tripPost = new TripPost
            {
                Date = new DateTime(2020, 1, 1),
                Description = "Bike Commute",
                Distance = new DistanceDetail { Length = 5.6, Units = Units.Miles },
                IsRoundTrip = true,
                TripFrom = "Home",
                TripTo = "SW"
            };
            var req = TestFactory.CreateHttpRequest(JsonConvert.SerializeObject(tripPost));
            var logger = TestFactory.CreateLogger(LoggerTypes.List);

            TripEntry tripEntry = null;
            _tripCollector.Setup(x => x.AddAsync(It.IsAny<TripEntry>(), default(CancellationToken)))
                .Callback((TripEntry entry, CancellationToken token) => tripEntry = entry);

            // act
            var result = await LogTripRequest.Run(req, _tripCollector.Object, logger);

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
