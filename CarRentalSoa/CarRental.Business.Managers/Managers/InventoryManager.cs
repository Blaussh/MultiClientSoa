using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using CarRental.Business.Common;
using CarRental.Business.Contracts;
using CarRental.Business.Entities;
using CarRental.Data.Contracts;
using Core.Common.Contracts;
using Core.Common.Core;
using Core.Common.Exceptions;

namespace CarRental.Business.Managers
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
        ConcurrencyMode = ConcurrencyMode.Multiple,
        ReleaseServiceInstanceOnTransactionComplete = false)]
    public class InventoryManager : ManagerBase, IInventoryService
    {
        public InventoryManager()
        {
        }

        public InventoryManager(IDataRepositoryFactory dataRepositoryFactory)
        {
            _dataRepositoryFactory = dataRepositoryFactory;
        }

        [Import]
        private IDataRepositoryFactory _dataRepositoryFactory;

        [Import] private ICarRentalEngine _iCarRentalEngine;

        public Car GetCar(int carId)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                ICarRepository carRepository = _dataRepositoryFactory.GetDataRepository<ICarRepository>();

                Car carEntity = carRepository.Get(carId);

                if (carEntity == null)
                {
                    NotFoundException ex =
                        new NotFoundException(string.Format("Car with ID of {0} is not in the database", carId));

                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                return carEntity;
            });

        }

        public Car[] GetAllCars() //Also update the returned array if each car rented or not
        {
            return ExecuteFaultHandledOperation(() =>
            {
                ICarRepository carRepository = _dataRepositoryFactory.GetDataRepository<ICarRepository>();
                IRentalRepository rentalRepository = _dataRepositoryFactory.GetDataRepository<IRentalRepository>();

                IEnumerable<Car> cars = carRepository.Get();
                IEnumerable<Rental> rentedCars = rentalRepository.GetCurrentlyRentedCars();

                foreach (Car car in cars)
                {
                    Rental rentedCar = rentedCars.FirstOrDefault(rc => rc.CarId == car.CarId);
                    car.CurrentlyRented = (rentedCar != null);
                }

                return cars.ToArray();
            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public Car UpdateCar(Car car)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                ICarRepository carRepository = _dataRepositoryFactory.GetDataRepository<ICarRepository>();
                Car updateEntity = null;

                if (car.CarId == 0)
                    updateEntity = carRepository.Add(car);
                else
                    updateEntity = carRepository.Update(car);
                return updateEntity;
            });

        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public void DeleteCar(int carId)
        {
            ExecuteFaultHandledOperation(() =>
            {
                ICarRepository carRepository = _dataRepositoryFactory.GetDataRepository<ICarRepository>();
                carRepository.Remove(carId);
            });
        }

        public Car[] GetAvailableCars(DateTime pickupDate, DateTime returnDate)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                ICarRepository carRepository = _dataRepositoryFactory.GetDataRepository<ICarRepository>();
                IRentalRepository rentalRepository = _dataRepositoryFactory.GetDataRepository<IRentalRepository>();
                IReservationRepository reservationRepository = _dataRepositoryFactory.GetDataRepository<IReservationRepository>();

                IEnumerable<Car> cars = carRepository.Get();
                IEnumerable<Rental> rentedCars = rentalRepository.GetCurrentlyRentedCars();
                IEnumerable<Reservation> reservedCars = reservationRepository.Get();

                List<Car> availableCars = new List<Car>();

                foreach (Car car in cars)
                {
                    if(1==1) // carId, pickupDate, returnDate, rentedCars, reservedCars
                        availableCars.Add(car);
                }

                return availableCars.ToArray();
            });
        }
    }
}
