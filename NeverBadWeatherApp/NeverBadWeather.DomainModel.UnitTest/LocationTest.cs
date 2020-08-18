using System;
using NUnit.Framework;

namespace NeverBadWeather.DomainModel.UnitTest
{
    public class LocationTest
    {
        [Test]
        public void TestIsWithinInside()
        {
            // arrange
            var mainLocation = new Location(0.5f, 0.5f);
            var location1 = new Location(0, 0);
            var location2 = new Location(1, 1);

            // act
            var isWithin = mainLocation.IsWithin(location1, location2);

            // assert
            Assert.IsTrue(isWithin);
        }

        [Test]
        public void TestIsWithinFalse()
        {
            var location1 = new Location(0,0);
            var location2 = new Location(1, 1);
            var mainLocation = new Location(3f, 3f);

            var isWithin = mainLocation.IsWithin(location1, location2);
            Assert.IsFalse(isWithin);
        }


        [Test]
        public void TestGetDistanceFrom()
        {
            var location1 = new Location(5, 5);
            var location2 = new Location(5, 6);
            var distance = location1.GetDistanceFrom(location2);

            Assert.AreEqual(1, distance, 0.0001 );
        }

        [Test]
        public void TestCreateWithDeltaMin()
        {
            var location = new Location(58,10);
            var addMinus1Delta = location.CreateWithDelta(-1, -1);

            var newLat = addMinus1Delta.Latitude;
            var newLong = addMinus1Delta.Longitude;

            Assert.AreEqual(57, newLat);
            Assert.AreEqual(9, newLong);
        }

        [Test]
        public void TestCreateWithDeltaMax()
        {
            var location = new Location(58,10);
            var addPlus1Delta = location.CreateWithDelta(1, 1);

            var newLat = addPlus1Delta.Latitude;
            var newLong = addPlus1Delta.Longitude;

            Assert.AreEqual(59, newLat);
            Assert.AreEqual(11, newLong);
        }

        [Test]
        public void TestToFloat()
        {
            var location = new Location("58","10");

            Assert.AreEqual(58, location.Latitude);

        }

        
    }
}