using System.ComponentModel.Design;
using NUnit.Framework;

namespace NeverBadWeather.DomainModel.UnitTest
{
    class ClothingRuleTest
    {
        [Test]
        public void TestIfMatch()
        {
            var clothingRule = new ClothingRule(15, 25,null,"Sommertøy");
            var tempStatistics = new TemperatureStatistics();
            tempStatistics.AddTemperature(17);
            tempStatistics.AddTemperature(20);
            var match = clothingRule.Match(tempStatistics);

            Assert.IsTrue(match);
        }

        [Test]
        public void TestIfNoMatch()
        {
            var clothingRule = new ClothingRule(5, 10, null, "Boblejakke");
            var tempStatistics = new TemperatureStatistics();
            tempStatistics.AddTemperature(2);
            tempStatistics.AddTemperature(12);
            var matchResult = clothingRule.Match(tempStatistics);

            Assert.IsFalse(matchResult);
        }

    }

}
