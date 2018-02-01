using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Client.Entities
{
    public class Car
    {
        private int _carId;

        public int CarId
        {
            get { return _carId; }
            set { _carId = value; }
        }

        private string _description;

        public string Description
        {
            get { return _description; }
            set { _description = Description; }
        }

        private string _color;

        public string Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = Color;
            }
        }

        private int _year;

        public int Year
        {
            get { return _year; }
            set { _year = value; }
        }

        private decimal _rentalPrice;

        public decimal RentalPrice
        {
            get { return _rentalPrice; }
            set { _rentalPrice = value; }
        }

        private bool _currentlyRented;

        public bool CurrentlyRented
        {
            get { return _currentlyRented; }
            set { _currentlyRented = value; }
        }
    }
}
