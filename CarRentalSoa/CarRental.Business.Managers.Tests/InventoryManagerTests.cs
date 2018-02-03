using System;
using System.Security.Principal;
using System.Threading;
using CarRental.Business.Entities;
using CarRental.Data.Contracts;
using Core.Common.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CarRental.Business.Managers.Tests
{
    [TestClass]
    public class InventoryManagerTests
    {
        [TestInitialize]
        public void Initialize()
        {
            GenericPrincipal principal = new GenericPrincipal(
                new GenericIdentity("Shai"), new string[]{"Admin"});

            Thread.CurrentPrincipal = principal;
        }

        [TestMethod]
        public void UpdateCar_add_new()
        {
            Car newCar = new Car();
            Car addedCar = new Car() { CarId = 1 };

            Mock<IDataRepositoryFactory> mockDataRepositoryFactory = new Mock<IDataRepositoryFactory>();
            mockDataRepositoryFactory.Setup(obj => obj.GetDataRepository<ICarRepository>().Add(newCar)).Returns(addedCar);

            InventoryManager manager = new InventoryManager(mockDataRepositoryFactory.Object);

            Car results = manager.UpdateCar(newCar);

            Assert.IsTrue(results == addedCar);
        }

        [TestMethod]
        public void UpdateCar_update_existing()
        {
            Car existingCar = new Car() { CarId = 1 };
            Car updatedCar = new Car() { CarId = 1 };

            Mock<IDataRepositoryFactory> mockDataRepositoryFactory = new Mock<IDataRepositoryFactory>();
            mockDataRepositoryFactory.Setup(obj => obj.GetDataRepository<ICarRepository>().Update(existingCar)).Returns(updatedCar);

            InventoryManager manager = new InventoryManager(mockDataRepositoryFactory.Object);

            Car results = manager.UpdateCar(existingCar);

            Assert.IsTrue(results == updatedCar);

        }
    }
}
