using Core.Interfaces;
using Core.Models;
using Core.Models.Travelplans;
using Core.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Tests.Services
{
    public class TravelPlanServiceTests
    {
        [Test]
        public async Task CalculateTravelPlanAsync_Returns_And_Stores_UnPersisted_Routes()
        {
            var destination1 = Guid.NewGuid();
            var destination2 = Guid.NewGuid();
            var origin1 = Guid.NewGuid();
            var origin2 = Guid.NewGuid();

            var travelplanRepository = new Mock<ICalculatedRoutesRepository>();

            travelplanRepository.Setup(x => x.GetPersistedRouteAsync(It.IsAny<IEnumerable<Guid>>())).ReturnsAsync(new List<CalculatedRoutes>());

            Dictionary<Guid, HashSet<Guid>> unpersistedRoutes = new();

            travelplanRepository.Setup(x => x.GetAndAddUnpersistedRoutesAsync(It.IsAny<Dictionary<Guid, HashSet<Guid>>>()))
                .Callback((Dictionary<Guid, HashSet<Guid>> routes) =>
                {
                    unpersistedRoutes = routes;
                }).ReturnsAsync(new List<CalculatedRoutes>
                {
                    new ()
                    {
                        Id = Guid.NewGuid(),
                        Origin = GetAddress(1, origin1, true),
                        Destinations = new List<Destinations>
                        {
                            new (){ Address = GetAddress(1, destination1, false) },
                            new (){ Address = GetAddress(2, destination2, false) }
                        }
                    },
                    new ()
                    {
                        Id = Guid.NewGuid(),
                        Origin = GetAddress(2, origin2, true),
                        Destinations = new List<Destinations>
                        {
                            new (){ Address = GetAddress(2, destination2, false)}
                        }
                    }
                });

            var sut = new TravelplanService(travelplanRepository.Object);

            var selectedRoutes = new List<WayPoints>
            {
                new (){ OriginContactId = origin1, DestinationContactId = destination1},
                new (){ OriginContactId = origin1, DestinationContactId = destination2},
                new (){ OriginContactId = origin2, DestinationContactId = destination2}
            };

            await sut.CalculateTravelplanAsync(Guid.NewGuid(), selectedRoutes);

            var expectedUnpersistedRoutes = new Dictionary<Guid, HashSet<Guid>>
            {
                { origin1, new HashSet<Guid> { destination1, destination2} },
                { origin2, new HashSet<Guid> { destination2 } },
            };

            Assert.That(expectedUnpersistedRoutes, Is.EqualTo(unpersistedRoutes));
        }

        [Test]
        public async Task CalculateTravelPlanAsync_Returns_Persisted_Routes()
        {
            var destination1 = Guid.NewGuid();
            var destination2 = Guid.NewGuid();
            var destination3 = Guid.NewGuid();
            var origin1 = Guid.NewGuid();
            var origin2 = Guid.NewGuid();
            var originAddress1 = GetAddress(1, origin1, true);
            var originAddress2 = GetAddress(2, origin2, true);
            var destinationAddress1 = GetAddress(1, destination1, false);
            var destinationAddress2 = GetAddress(2, destination2, false);
            var destinationAddress3 = GetAddress(3, destination3, false);

            var travelplanRepository = new Mock<ICalculatedRoutesRepository>();

            travelplanRepository.Setup(x => x.GetPersistedRouteAsync(It.IsAny<IEnumerable<Guid>>())).ReturnsAsync(new List<CalculatedRoutes>
                {
                    new ()
                    {
                        Id = Guid.NewGuid(),
                        Origin = originAddress1,
                        Destinations = new List<Destinations>
                        {
                            new (){ Address = destinationAddress1, Distance = 1},
                            new (){ Address = destinationAddress2, Distance = 2},
                            new (){ Address = destinationAddress3, Distance = 3},
                        }
                    },
                    new ()
                    {
                        Id = Guid.NewGuid(),
                        Origin = originAddress2,
                        Destinations = new List<Destinations>
                        {
                            new (){ Address = destinationAddress2, Distance = 2},
                            new (){ Address = destinationAddress3, Distance = 3},
                        }
                    }
                });

            travelplanRepository.Setup(x => x.GetAndAddUnpersistedRoutesAsync(It.IsAny<Dictionary<Guid, HashSet<Guid>>>()))
                .ReturnsAsync(new List<CalculatedRoutes>());

            var sut = new TravelplanService(travelplanRepository.Object);

            var selectedRoutes = new List<WayPoints>
            {
                new (){ OriginContactId = origin1, DestinationContactId = destination1},
                new (){ OriginContactId = origin2, DestinationContactId = destination3}
            };

            var result = await sut.CalculateTravelplanAsync(Guid.NewGuid(), selectedRoutes);

            Assert.That(result.Count, Is.EqualTo(2));
            
            var origin1result = result.First(x => x.Origin.Equals(originAddress1));
            Assert.That(origin1result.Destination, Is.EqualTo(destinationAddress1));

            var origin2result = result.First(x => x.Origin.Equals(originAddress2));
            Assert.That(origin2result.Destination, Is.EqualTo(destinationAddress3));
        }

        [Test]
        public async Task CalculateTravelPlanAsync_Returns_Unpersisted_Routes_Only()
        {
            var calculatedRouteId1 = Guid.NewGuid();
            var origin1 = Guid.NewGuid();
            var destination1 = Guid.NewGuid();
            var destination2 = Guid.NewGuid();
            var originAddress1 = GetAddress(1, origin1, true);
            var destinationAddress1 = GetAddress(1, destination1, false);
            var destinationAddress2 = GetAddress(2, destination2, false);

            var travelplanRepository = new Mock<ICalculatedRoutesRepository>();
            travelplanRepository.Setup(x => x.GetPersistedRouteAsync(It.IsAny<IEnumerable<Guid>>())).ReturnsAsync(new List<CalculatedRoutes>
            {
                new()
                {
                    Id = calculatedRouteId1,
                    Origin = originAddress1,
                    Destinations = new List<Destinations>
                    {
                        new(){ Address = destinationAddress1 }
                    }
                }
            });

            travelplanRepository.Setup(x => x.GetAndAddUnpersistedRoutesAsync(It.IsAny<Dictionary<Guid, HashSet<Guid>>>()))
               .ReturnsAsync(new List<CalculatedRoutes>
               {
                   new()
                   {
                       Id = calculatedRouteId1,
                       Origin = originAddress1,
                       Destinations = new List<Destinations>
                       {
                           new() { Address = destinationAddress2 }
                       }
                   }
               });

            var sut = new TravelplanService(travelplanRepository.Object);

            var selectedRoutes = new List<WayPoints>
            {
                new (){ OriginContactId = origin1, DestinationContactId = destination1},
                new (){ OriginContactId = origin1, DestinationContactId = destination2},
            };

            var result = await sut.CalculateTravelplanAsync(Guid.NewGuid(), selectedRoutes);

            Assert.That(result.Count, Is.EqualTo(2));

            var expectedDestinations = new List<Address> { destinationAddress1, destinationAddress2 };

            var origin1result = result.Where(x => x.Origin.Equals(originAddress1));
            Assert.That(origin1result.Select(x => x.Destination), Is.EquivalentTo(expectedDestinations));
        }

        [Test]
        public async Task CalculateTravelPlanAsync_Returns_Merged_Distinct_Combined_Routes()
        {
            var destination1 = Guid.NewGuid();
            var destination2 = Guid.NewGuid();
            var destination3 = Guid.NewGuid();
            var origin1 = Guid.NewGuid();
            var origin2 = Guid.NewGuid();
            var origin3 = Guid.NewGuid();

            var originAddress1 = GetAddress(1, origin1, true);
            var originAddress2 = GetAddress(2, origin2, true);
            var originAddress3 = GetAddress(3, origin3, true);
            var destinationAddress1 = GetAddress(1, destination1, false);
            var destinationAddress2 = GetAddress(2, destination2, false);
            var destinationAddress3 = GetAddress(3, destination3, false);

            var calculatedRouteId1 = Guid.NewGuid();
            var calculatedRouteId2 = Guid.NewGuid();
            var calculatedRouteId3 = Guid.NewGuid();

            var travelplanRepository = new Mock<ICalculatedRoutesRepository>();

            travelplanRepository.Setup(x => x.GetPersistedRouteAsync(It.IsAny<IEnumerable<Guid>>())).ReturnsAsync(new List<CalculatedRoutes>
                {
                    new ()
                    {
                        Id = calculatedRouteId1,
                        Origin = originAddress1,
                        Destinations = new List<Destinations>
                        {
                            new (){ Address = destinationAddress1, Distance = 1},
                        }
                    },
                    new ()
                    {
                        Id = calculatedRouteId2,
                        Origin = originAddress2,
                        Destinations = new List<Destinations>
                        {
                            new (){ Address = destinationAddress2, Distance = 2},
                        }
                    }
                });

            travelplanRepository.Setup(x => x.GetAndAddUnpersistedRoutesAsync(It.IsAny<Dictionary<Guid, HashSet<Guid>>>()))
               .ReturnsAsync(new List<CalculatedRoutes>
                {
                    new ()
                    {
                        Id = calculatedRouteId1,
                        Origin = originAddress1,
                        Destinations = new List<Destinations>
                        {
                            new (){ Address = destinationAddress2, Distance = 2}
                        }
                    },
                    new ()
                    {
                        Id = calculatedRouteId2,
                        Origin = originAddress2,
                        Destinations = new List<Destinations>
                        {
                            new (){ Address = destinationAddress1, Distance = 1}
                        }
                    },
                    new ()
                    {
                        Id = calculatedRouteId3,
                        Origin = originAddress3,
                        Destinations = new List<Destinations>
                        {
                            new (){ Address = destinationAddress1, Distance = 1},
                            new (){ Address = destinationAddress2, Distance = 2}
                        }
                    }
                });

            var sut = new TravelplanService(travelplanRepository.Object);

            var selectedRoutes = new List<WayPoints>
            {
                new (){ OriginContactId = origin1, DestinationContactId = destination1},
                new (){ OriginContactId = origin1, DestinationContactId = destination2},
                new (){ OriginContactId = origin2, DestinationContactId = destination1},
                new (){ OriginContactId = origin2, DestinationContactId = destination2},
                new (){ OriginContactId = origin3, DestinationContactId = destination1},
                new (){ OriginContactId = origin3, DestinationContactId = destination2}
            };

            var result = await sut.CalculateTravelplanAsync(Guid.NewGuid(), selectedRoutes);

            Assert.That(result.Count, Is.EqualTo(6));

            var expectedDestinations = new List<Address> { destinationAddress1, destinationAddress2 };

            var origin1result = result.Where(x => x.Origin.Equals(originAddress1));
            Assert.That(origin1result.Select(x => x.Destination), Is.EquivalentTo(expectedDestinations));

            var origin2result = result.Where(x => x.Origin.Equals(originAddress2));
            Assert.That(origin1result.Select(x => x.Destination), Is.EquivalentTo(expectedDestinations));

            var origin3result = result.Where(x => x.Origin.Equals(originAddress3));
            Assert.That(origin1result.Select(x => x.Destination), Is.EquivalentTo(expectedDestinations));

        }

        private static Address GetAddress(int id, Guid ContactId, bool isOrigin)
        {
            var name = isOrigin ? "origin" : "destination"; 
            return new Address
            {
                Street = $"{name} {id}",
                City = $"{name} {id}",
                ContactId = ContactId,
                HouseNumber = id,
                PostalCode = $"{name} {id}"
            };
        }
    }
}