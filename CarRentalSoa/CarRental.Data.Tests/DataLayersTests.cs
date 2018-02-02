using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using CarRental.Business.Bootstrapper;
using CarRental.Business.Entities;
using CarRental.Data.Contracts;
using Core.Common.Contracts;
using Core.Common.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CarRental.Data.Tests
{
    [TestClass]
    public class DataLayersTests
    {
        [TestInitialize]
        public void Initialize()
        {
            ObjectBase.Container = MEFLoader.Init();
        }

        [TestMethod]
        public void test_repository_usage()
        {
            RepositoryTestClass repositoryTest = new RepositoryTestClass();

            IEnumerable<Car> cars = repositoryTest.GetCars();

            Assert.IsTrue(cars != null, "Cars list cant be null");
        }

        [TestMethod]
        public void test_repository_factory_usage()
        {
            RepositoryFactoryTestClass factoryTest = new RepositoryFactoryTestClass();

            IEnumerable<Car> cars = factoryTest.GetCars();

            Assert.IsTrue(cars != null, "Cars list cant be null");
        }

        [TestMethod]
        public void test_repository_mocking()
        {
            List<Car> cars = new List<Car>()
            {
                new Car(){CarId = 1,Description = "Mustang"},
                new Car(){CarId = 2,Description = "Corvette"}
            };

            Mock<ICarRepository> mockCarRepository = new Mock<ICarRepository>();
            mockCarRepository.Setup(obj => obj.Get()).Returns(cars);

            RepositoryTestClass repositoryTest = new RepositoryTestClass(mockCarRepository.Object);

            IEnumerable<Car> ret = repositoryTest.GetCars();

            Assert.IsTrue(ret.ToList()[0].Description == "Mustang");
        }

        [TestMethod]
        public void test_factory_mocking1()
        {
            List<Car> cars = new List<Car>()
            {
                new Car(){CarId = 1,Description = "Mustang"},
                new Car(){CarId = 2,Description = "Corvette"}
            };

            Mock<IDataRepositoryFactory> mockDataRepository = new Mock<IDataRepositoryFactory>();
            mockDataRepository.Setup(obj => obj.GetDataRepository<ICarRepository>().Get()).Returns(cars);

            RepositoryFactoryTestClass factoryTest = new RepositoryFactoryTestClass(mockDataRepository.Object);

            IEnumerable<Car> ret = factoryTest.GetCars();

            Assert.IsTrue(ret==cars);
        }

        [TestMethod]
        public void test_factory_mocking2()
        {
            List<Car> cars = new List<Car>()
            {
                new Car(){CarId = 1,Description = "Mustang"},
                new Car(){CarId = 2,Description = "Corvette"}
            };

            Mock<ICarRepository> mockCarRepository = new Mock<ICarRepository>();
            mockCarRepository.Setup(obj=>obj.Get()).Returns(cars);
            
            Mock<IDataRepositoryFactory> mockDataRepository = new Mock<IDataRepositoryFactory>();
            mockDataRepository.Setup(obj => obj.GetDataRepository<ICarRepository>()).Returns(mockCarRepository.Object);

            RepositoryFactoryTestClass factoryTest = new RepositoryFactoryTestClass(mockDataRepository.Object);

            IEnumerable<Car> ret = factoryTest.GetCars();

            Assert.IsTrue(ret == cars);
        }


    }

    public class RepositoryTestClass
    {
        public RepositoryTestClass(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        public RepositoryTestClass()
        {
            ObjectBase.Container.SatisfyImportsOnce(this);
        }

        [Import]
        private ICarRepository _carRepository;

        public IEnumerable<Car> GetCars()
        {
            IEnumerable<Car> cars = _carRepository.Get();
            return cars;
        }
    }

    public class RepositoryFactoryTestClass
    {
        public RepositoryFactoryTestClass()
        {
            ObjectBase.Container.SatisfyImportsOnce(this);
        }

        [Import]
        private IDataRepositoryFactory _dataRepositoryFactory;

        public RepositoryFactoryTestClass(IDataRepositoryFactory dataRepositoryFactory)
        {
            _dataRepositoryFactory = dataRepositoryFactory;
        }

        public IEnumerable<Car> GetCars()
        {
            ICarRepository carRepository = _dataRepositoryFactory.GetDataRepository<ICarRepository>();
            IEnumerable<Car> cars = carRepository.Get();
            return cars;
        }
    }
}
