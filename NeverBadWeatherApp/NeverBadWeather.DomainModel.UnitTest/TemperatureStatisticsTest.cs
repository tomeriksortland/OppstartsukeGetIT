using NeverBadWeather.DomainModel.Exception;
using NUnit.Framework;

namespace NeverBadWeather.DomainModel.UnitTest
{
    class TemperatureStatisticsTest
    {
        [Test]
        public void TestAddOneTemperature()
        {
            var tempStatistics = new TemperatureStatistics();
            tempStatistics.AddTemperature(10);
            tempStatistics.AddTemperature(20);

            Assert.AreEqual(10, tempStatistics.Min);
            Assert.AreEqual(20, tempStatistics.Max);
        }

        [Test]
        public void TestAddMultipleTemperatures()
        {
            var tempStatistics = new TemperatureStatistics();
            tempStatistics.AddTemperature(10);
            tempStatistics.AddTemperature(20);
            tempStatistics.AddTemperature(30);

            Assert.AreEqual(10, tempStatistics.Min);
            Assert.AreEqual(30, tempStatistics.Max);
        }

        [Test]
        public void TestMinHasNoInputException()
        {
            Assert.Throws<CannotGiveMinOrMaxWithNoNumbersException>(() => TestTempStatsWithoutValue());
        }

        public void TestTempStatsWithoutValue()
        {
            var tempStatistics = new TemperatureStatistics();
            Assert.AreEqual(10, tempStatistics.Min);
            Assert.AreEqual(10, tempStatistics.Max);
        }
    }
}
