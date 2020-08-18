using System;
using NUnit.Framework;

namespace NeverBadWeather.DomainModel.UnitTest
{
    class WeatherForecastTest
    {
        [Test]
        public void TestGetStatsWithOneNumber()
        {
            var tempForecast = new TemperatureForecast(20, DateTime.Now, DateTime.MaxValue);
            var weatherForecast = new WeatherForecast(new []{ tempForecast });
            var temperatureStatistics = weatherForecast.GetStats();
            Assert.AreEqual(20, temperatureStatistics.Max);
        }

        [Test]
        public void TestGetStatsWithTwoNumber()
        {
            var tempForecast1 = new TemperatureForecast(10, DateTime.Now, DateTime.MaxValue);
            var tempForecast2 = new TemperatureForecast(15, DateTime.Now, DateTime.MaxValue);
            var weatherForecast = new WeatherForecast(new []{tempForecast1, tempForecast2});
            var temperatureStatistics = weatherForecast.GetStats();

            Assert.AreEqual(10, temperatureStatistics.Min);
            Assert.AreEqual(15, temperatureStatistics.Max);
        }

        [Test]
        public void TestLimitTo()
        {
            var date1From = new DateTime(1991,01,01);
            var date1To = new DateTime(1999,01,01);
            
            var date2From = new DateTime(2010,01,01);
            var date2To = new DateTime(2015,01,01);

            var dateToCheckFrom = new DateTime(1990,01,01);
            var dateToCheckTo = new DateTime(2000,01,01);
           
            var tempForecast = new TemperatureForecast(15, date1From, date1To);
            var tempForecast2 = new TemperatureForecast(25, date2From, date2To);
            var weatherForecast = new WeatherForecast(new []{tempForecast, tempForecast2});
            weatherForecast.LimitTo(dateToCheckFrom, dateToCheckTo);

            Assert.AreEqual(1, weatherForecast.Temperatures.Length);
        }
        
       
    }
}
