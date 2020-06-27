using System;

namespace DBProjectRentalStore
{
    class Rental
    {
        public int CopyId { get; set; }
        public int ClientId { get; set; }
        public DateTime DateOfRental { get; set; }
        public DateTime DateOfReturn { get; set; }

        public Rental(int copyId, DateTime dateOfReturn)
        {
            this.CopyId = copyId;
            this.DateOfReturn = dateOfReturn; 
        }

        public Rental(int copyid, int clientid, DateTime dateOfRental)
        {
            this.CopyId = copyid;
            this.ClientId = clientid;
            this.DateOfRental = dateOfRental;

        }

        public Rental(int copyid, int clientid, DateTime dateOfRental, DateTime dateOfReturn)
        {
            this.CopyId = copyid;
            this.ClientId = clientid;
            this.DateOfRental = dateOfRental;
            this.DateOfReturn = dateOfReturn;
        }

        public override string ToString()
        {
            if (DateOfReturn == null)
                return $"Client with {ClientId} id - has {CopyId}, rented in: {DateOfRental} and not yet returned";
            else return $"{CopyId} copy id rented, rented in: {DateOfRental.Date} and returned: {DateOfReturn.Date}";
        }

    }
}
