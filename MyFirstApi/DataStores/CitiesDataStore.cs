using MyFirstApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstApi.DataStores
{
    public class CitiesDataStore
    {
        public List<CityDto> Cities { get; set; }
        public static CitiesDataStore Current { get; } = new CitiesDataStore();

        public CitiesDataStore()
        {
            Cities = new List<CityDto>()
            {
                new CityDto()
                {
                    Id = 1,
                    Name = "New York City",
                    Description = "Cool place in the NE",
                    PointOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id = 1,
                            Name = "Central Park",
                            Description = "Famous Park"
                        },
                        new PointOfInterestDto()
                        {
                            Id = 2,
                            Name = "Empire State Building",
                            Description = "Famous Building"
                        }
                    }
                    
                },
                new CityDto()
                {
                    Id = 2,
                    Name = "Miami",
                    Description = "Place I was born",
                    PointOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id = 1,
                            Name = "South Beach",
                            Description = "Famous Beach"
                        },
                        new PointOfInterestDto()
                        {
                            Id = 2,
                            Name = "Hialiah",
                            Description = "Cuban Neighborhood"
                        }
                    }
                },
                new CityDto()
                {
                    Id = 3,
                    Name= "Seattle",
                    Description = "Place where my parents live",
                    PointOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id = 1,
                            Name = "Space Needle",
                            Description = "Famous Building"
                        },
                        new PointOfInterestDto()
                        {
                            Id = 1,
                            Name = "Bellvue",
                            Description = "Location of Microsoft Headquarters"
                        }
                    }

                }
            };
        }
    }
}
