using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NeverBadWeather.ApplicationServices;
using NeverBadWeather.DomainModel;
using NeverBadWeather.DomainModel.Exception;
using NeverBadWeather.DomainServices;
using NeverBadWeather.Infrastructure.WeatherForecastService;
using NUnit.Framework;

namespace NeverBadWeather.UnitTest
{
    public class ClothingRecommendationServiceTest
    {

        [Test]
        public async Task TestHappyCase()
        {
            // arrange
            var testDate = new DateTime(2000, 1, 1);
            var testPeriod = new TimePeriod(testDate, testDate.AddHours(10));
            var testLocation = new Location(59, 10);

            var mockWeatherForecastService = new Mock<IWeatherForecastService>();
            var mockClothingRuleRepository = new Mock<IClothingRuleRepository>();

            mockClothingRuleRepository
                .Setup(crr => crr.GetRulesByUser(It.IsAny<Guid?>()))
                .ReturnsAsync(new[]
                {
                    new ClothingRule(-20, 10, null, "Bobledress"),
                    new ClothingRule(10, 20, null, "Bukse og genser"),
                    new ClothingRule(20, 40, null, "T-skjore og shorts"),
                });


            mockWeatherForecastService
                .Setup(fs => fs.GetAllPlaces())
                .Returns(new[] { new Place("", "", "", "Andeby", new Location(59.1f, 10.1f)), });

            mockWeatherForecastService
                .Setup(fs => fs.GetWeatherForecast(It.IsAny<Place>()))
                .ReturnsAsync(new WeatherForecast(new[] {
                    new TemperatureForecast(25,testDate.AddHours(2), testDate.AddHours(4)),
                }));

            // act
            var request = new ClothingRecommendationRequest(testPeriod, testLocation);
            var service = new ClothingRecommendationService(
                mockWeatherForecastService.Object,
                mockClothingRuleRepository.Object);
            var recommendation = await service.GetClothingRecommendation(request);

            // assert
            Assert.AreEqual("Andeby",recommendation.Place.Name);
            Assert.That(recommendation.Rules, Has.Exactly(1).Items);
            var rule = recommendation.Rules.First();
            Assert.AreEqual("T-skjore og shorts", rule.Clothes);
        }

        [Test]
        public async Task TestCreateRule()
        {
            var clothingRule = new ClothingRule(10, 20, null, "bukse");

            var mockIWeatherForecastService = new Mock<IWeatherForecastService>();
            var mockIClothingRuleRepository = new Mock<IClothingRuleRepository>();

            mockIClothingRuleRepository
                .Setup(crr => crr.Create(It.IsAny<ClothingRule>()))
                .ReturnsAsync(1);

            
            var clothingrec = new ClothingRecommendationService(mockIWeatherForecastService.Object, mockIClothingRuleRepository.Object);

            var create = await clothingrec.CreateOrUpdateRule(clothingRule);

            Assert.IsTrue(create);
            mockIClothingRuleRepository.Verify(crr=> crr.Create(It.Is<ClothingRule>(cr=>cr.Clothes =="bukse")),Times.AtMost(1));
        }

        [Test]
        public async Task TestUpdateRule()
        {
            var id = "5c49da69-ec97-4882-ba23-7535de7f16a3";
            var clothingRule1 = new ClothingRule(10,20,null,"bukse", new Guid(id));
            var clothingRule2 = new ClothingRule(5, 15, null, "bukse", new Guid(id));
            
            var mockIWeatherForecastService = new Mock<IWeatherForecastService>();
            var mockIClothingRuleRepository = new Mock<IClothingRuleRepository>();

            mockIClothingRuleRepository
                .Setup(crr => crr.Create(It.IsAny<ClothingRule>()))
                .ReturnsAsync(1);

            var ClothingRuleRecService = new ClothingRecommendationService(mockIWeatherForecastService.Object, mockIClothingRuleRepository.Object);
            
            var create = await ClothingRuleRecService.CreateOrUpdateRule(clothingRule1);

            Assert.AreEqual(true, create);
            
            mockIClothingRuleRepository.Verify(c => c.Create(It.Is<ClothingRule>(c => c.FromTemperature == 10)),Times.AtMost(1));


            mockIClothingRuleRepository
                .Setup(u => u.Update(It.IsAny<ClothingRule>()))
                .ReturnsAsync(1);

            var update = await ClothingRuleRecService.CreateOrUpdateRule(clothingRule2);

            Assert.AreEqual(true, update);
            mockIClothingRuleRepository.Verify(u => u.Update(It.Is<ClothingRule>(u => u.FromTemperature == 5)),Times.AtMost(1));
        }

        [Test]
        public async Task TestDeleteRule()
        {
            var guid = "5c49da69-ec97-4882-ba23-7535de7f16a3";
            var clothingRule = new ClothingRule(10, 20, null, "bukse", new Guid(guid));

            var mockIWeatherForecastService = new Mock<IWeatherForecastService>();
            var mockIClothingRuleRepository = new Mock<IClothingRuleRepository>();

            mockIClothingRuleRepository
                .Setup(c => c.Create(It.IsAny<ClothingRule>()))
                .ReturnsAsync(1);

            var clothingRuleService = new ClothingRecommendationService(mockIWeatherForecastService.Object, mockIClothingRuleRepository.Object);

            var create = await clothingRuleService.CreateOrUpdateRule(clothingRule);

            Assert.AreEqual(true, create);
            mockIClothingRuleRepository.Verify(c => c.Create(It.Is<ClothingRule>(c => c.FromTemperature == 10)), Times.AtMost(1));

            mockIClothingRuleRepository
                .Setup(d => d.Delete(It.IsAny<ClothingRule>()))
                .ReturnsAsync(1);

            var delete = await clothingRuleService.DeleteRule(clothingRule);

            Assert.AreEqual(true, delete);
            mockIClothingRuleRepository.Verify(d => d.Delete(It.Is<ClothingRule>(cr=> cr.FromTemperature == 10)), Times.AtMost(1));
        }

        [Test]
        public async Task TestInvalidLatitudePosition()
        {
           var mockIClothingRuleRepository = new Mock<IClothingRuleRepository>();
           var mockIWeatherForecastService = new Mock<IWeatherForecastService>();
            
            var timeFrom = new DateTime(2000,01,01);
            var timeTo = new DateTime(2010, 01, 01);
           
            var timePeriod = new TimePeriod(timeFrom, timeTo);
            var location = new Location(null,null);

            var clothingService = new ClothingRecommendationService(mockIWeatherForecastService.Object, mockIClothingRuleRepository.Object);
            var clothingRequest = new ClothingRecommendationRequest(timePeriod, location);

            Assert.ThrowsAsync<NotAValidLocationException>(async() => await clothingService.GetClothingRecommendation(clothingRequest));

        }

    }
}