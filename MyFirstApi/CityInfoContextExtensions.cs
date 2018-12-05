using MyFirstApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstApi
{
    public static class CityInfoContextExtensions
    {
        public static void EnsureSeedDataForContext(this CityInfoContext context)
        {
            if (context.Cities.Any())
            {
                return;
            }

            //init seed data
            var cities = new List<City>()
            {
                new City()
                {
                    Name = "New York City",
                    Description = "Cool place in the NE",
                    PointOfInterests = new List<PointOfInterest>()
                    {
                        new PointOfInterest()
                        {
                            Name = "Central Park",
                            Description = "Famous Park"
                        },
                        new PointOfInterest()
                        {
                            Name = "Empire State Building",
                            Description = "Famous Building"
                        }
                    }

                },
                new City()
                {
                    Name = "Miami",
                    Description = "Place I was born",
                    PointOfInterests = new List<PointOfInterest>()
                    {
                        new PointOfInterest()
                        {
                            Name = "South Beach",
                            Description = "Famous Beach"
                        },
                        new PointOfInterest()
                        {
                            Name = "Hialiah",
                            Description = "Cuban Neighborhood"
                        }
                    }
                },
                new City()
                {
                    Name= "Seattle",
                    Description = "Place where my parents live",
                    PointOfInterests = new List<PointOfInterest>()
                    {
                        new PointOfInterest()
                        {
                            Name = "Space Needle",
                            Description = "Famous Building"
                        },
                        new PointOfInterest()
                        {
                            Name = "Bellvue",
                            Description = "Location of Microsoft Headquarters"
                        }
                    }

                }
            };

            context.Cities.AddRange(cities);
            context.SaveChanges();

        }
    }
}
