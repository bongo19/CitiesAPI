using MyFirstApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstApi.Services
{
    public interface ICityInfoRepository
    {
        IEnumerable<City>GetCities();

        City GetCity(int cityId, bool includePointsOfInterest);

        IEnumerable<PointOfInterest> GetPointOfInterestsForCity(int cityId);

        PointOfInterest GetPointOfInterestForCity(int cityId, int pointOfInterestId);

        bool CheckIfCityExists(int cityId);

        void AddPointOfInterestForCity(PointOfInterest pointOfInterest, int cityId);

        bool SavePointOfInterest();

        void DeletePointOfInterest(PointOfInterest pointOfInterest);
    }
}
