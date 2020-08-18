using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NeverBadWeather.DomainModel.Exception;

namespace NeverBadWeather.DomainModel
{
    public class PlaceList
    {
        private static PlaceList _instance;
        private Place[] _places;
        public static PlaceList Instance => _instance ??= new PlaceList();

        public bool IsLoaded { get; private set; }

        public PlaceList()
        {
        }

        public void Load(IEnumerable<Place> places)
        {
            _places = places.ToArray();
            IsLoaded = true;
        }

        public Place GetClosestPlace(Location location)
        {
            if (!IsLoaded) throw new PlaceListNotLoadedException();
            if (location.Latitude == null && location.Longitude == null || 
                location.Latitude == null && location.Longitude == 0 ||
                location.Latitude == 0 && location.Longitude == null ||
                location.Latitude == 0 && location.Longitude == 0)
            {
                throw new NotAValidLocationException();
            }
            var min = location.CreateWithDelta(-1, -1);
            var max = location.CreateWithDelta(1, 1);
            var minDistance = double.MaxValue;
            Place bestPlace = null;
            foreach (var place in _places)
            {
                if (!place.Location.IsWithin(min, max)) continue;
                var distance = place.Location.GetDistanceFrom(location);
                if (distance > minDistance) continue;
                minDistance = distance;
                bestPlace = place;
            }
            return bestPlace;
        }

    }
}
