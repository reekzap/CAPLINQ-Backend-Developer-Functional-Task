using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using ShippingRatesAPI.Controllers;
using ShippingRatesAPI.Models;
using ShippingRatesAPI.Repositories.Interfaces;
using Xunit;


namespace ShippingRatesAPI.Test.Controller
{
    public class CarrierControllerTests
    {
        private readonly ICarrierRepository _carrierRepository;
        private readonly CarrierController _carrierController;

        public CarrierControllerTests()
        {
            // Arrange
            _carrierRepository = A.Fake<ICarrierRepository>();
            _carrierController = new CarrierController(_carrierRepository);
        }

        [Fact]
        public async Task GetAllCarriers_ReturnsOkResult_WithListOfCarriers()
        {
            // Arrange
            var carriers = new List<Carrier>
            {
                new Carrier { Id = 1, Name = "Carrier1", ApiEndpoint = "http://endpoint1.com", ApiKey = "key1", HasOngoingShipments = true, HasPendingInvoices = false },
                new Carrier { Id = 2, Name = "Carrier2", ApiEndpoint = "http://endpoint2.com", ApiKey = "key2", HasOngoingShipments = false, HasPendingInvoices = true }
            };

            A.CallTo(() => _carrierRepository.GetAllCarriersAsync()).Returns(Task.FromResult(carriers));


            // Act
            var result = await _carrierController.GetAllCarriers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnCarriers = Assert.IsAssignableFrom<IEnumerable<Carrier>>(okResult.Value);
            Assert.Equal(2, returnCarriers.Count());

            var firstCarrier = returnCarriers.First();
            Assert.Equal(1, firstCarrier.Id);
            Assert.Equal("Carrier1", firstCarrier.Name);
            Assert.Equal("http://endpoint1.com", firstCarrier.ApiEndpoint);
            Assert.Equal("key1", firstCarrier.ApiKey);
            Assert.True(firstCarrier.HasOngoingShipments);
            Assert.False(firstCarrier.HasPendingInvoices);

            var secondCarrier = returnCarriers.Last();
            Assert.Equal(2, secondCarrier.Id);
            Assert.Equal("Carrier2", secondCarrier.Name);
            Assert.Equal("http://endpoint2.com", secondCarrier.ApiEndpoint);
            Assert.Equal("key2", secondCarrier.ApiKey);
            Assert.False(secondCarrier.HasOngoingShipments);
            Assert.True(secondCarrier.HasPendingInvoices);

        }

        [Fact]
        public async Task DisableCarrier_ReturnsOkResult_WhenSuccessful()
        {
            // Arrange
            int carrierId = 1;
            string reason = "Testing";
            string expectedMessage = "Carrier disabled successfully.";

            A.CallTo(() => _carrierRepository.DisableCarrierAsync(carrierId, reason)).Returns(Task.FromResult(expectedMessage));

            // Act
            var result = await _carrierController.DisableCarrier(carrierId, reason);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnMessage = okResult.Value.GetType().GetProperty("message").GetValue(okResult.Value, null);
            Assert.Equal(expectedMessage, returnMessage);
        }

        [Fact]
        public async Task DisableCarrier_ReturnsBadRequest_WhenCarrierNotFound()
        {
            // Arrange
            int carrierId = 1;
            string reason = "Testing";
            string expectedMessage = "Carrier not found.";

            A.CallTo(() => _carrierRepository.DisableCarrierAsync(carrierId, reason)).Throws(new InvalidOperationException(expectedMessage));

            // Act
            var result = await _carrierController.DisableCarrier(carrierId, reason);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnMessage = badRequestResult.Value.GetType().GetProperty("message").GetValue(badRequestResult.Value, null);
            Assert.Equal(expectedMessage, returnMessage);
        }

        [Fact]
        public async Task DisableCarrier_ReturnsBadRequest_WhenCannotDisableCarrier()
        {
            // Arrange
            int carrierId = 1;
            string reason = "Testing";
            string expectedMessage = "Cannot disable a carrier for some reasons.";

            A.CallTo(() => _carrierRepository.DisableCarrierAsync(carrierId, reason)).Throws(new InvalidOperationException(expectedMessage));

            // Act
            var result = await _carrierController.DisableCarrier(carrierId, reason);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnMessage = badRequestResult.Value.GetType().GetProperty("message").GetValue(badRequestResult.Value, null);
            Assert.Equal(expectedMessage, returnMessage);
        }
    }
}
