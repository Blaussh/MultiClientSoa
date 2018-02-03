using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRental.Business.Common;
using CarRental.Business.Entities;

namespace CarRental.Business.Business_Engines
{
    [Export(typeof(ICarRentalEngine))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class CarRentalEngine: ICarRentalEngine
    {
        public bool IsCarAvailableForRental(int carId, DateTime pickupDate, DateTime returnDate,
            IEnumerable<Rental> rentedCars, IEnumerable<Reservation> reservedCars)
        {
            bool available = true;

            Reservation reservation = reservedCars.FirstOrDefault(item => item.CarId == carId);
            if (reservation != null && (
                    (pickupDate >= reservation.RentalDate && pickupDate <= reservation.ReturnDate) ||
                    (returnDate >= reservation.RentalDate && returnDate <= reservation.ReturnDate)))
            {
                available = false;
            }

            if (available)
            {
                Rental rental = rentedCars.FirstOrDefault(item => item.CarId == carId);
                if (rental != null && (pickupDate <= rental.DateDue))
                    available = false;
            }
            return available;
        }
    }
}
