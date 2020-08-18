using NeverBadWeather.DomainModel.Exception;
using NeverBadWeather.Infrastructure.WeatherForecastService;
using NUnit.Framework;

namespace NeverBadWeather.DomainModel.UnitTest
{
    class PlaceListTest
    {
        [Test]
        public void TestGetClosestPlace()
        {
            var location = new Location(59.13118f, 10.21665f);
            var place = new Place("Norway", "Vestfold", "Sandefjord", "Sandefjord", location);
            var placeList = new PlaceList();
            placeList.Load(new[] { place });
            var closestPlace = placeList.GetClosestPlace(location);
            Assert.AreEqual(place, closestPlace);
        }

        [Test]
        public void TestGetClosestPlace3()
        {
            var location1 = new Location(0.5f, 0.5f);
            var place1 = new Place("Norway", "Vestfold", "Sandefjord", "Sandefjord", location1);
            var location2 = new Location(0.6f, 0.6f);
            var place2 = new Place("Norway", "Vestfold", "Sandefjord", "Sandefjord", location2);
            var placeList = new PlaceList();
            placeList.Load(new[] { place1, place2 });
            var location3 = new Location(0.7f, 0.7f);
            var closestPlace = placeList.GetClosestPlace(location2);
            Assert.AreEqual(place2, closestPlace);
        }

        [Test]
        public void TestGetClosestPlace2()
        {
            var location = new Location(59.13118f, 10.21665f);
            var place = new Place("Norway", "Vestfold", "Sandefjord", "Sandefjord", location);
            var placeList = new PlaceList();
            placeList.Load(new[] { place });
            var location2 = new Location(59f, 10f);
            var closestPlace = placeList.GetClosestPlace(location2);
            Assert.AreEqual(place, closestPlace);
        }

        [Test]
        public void TestCheckIfLoaded()
        {
            var weatherForecastService = new WeatherForecastServiceYr();
            var places = weatherForecastService.GetAllPlaces();
            var placeList = PlaceList.Instance;
            Assert.IsFalse(placeList.IsLoaded);
            placeList.Load(places);
            Assert.IsTrue(placeList.IsLoaded);
        }

        [Test]
        public void TestPlaceListNotLoadedException()
        {
            var placeList = new PlaceList();
            var location = new Location(58, 10);
            Assert.Throws<PlaceListNotLoadedException>(() => placeList.GetClosestPlace(location));
        }

   
    }
}
