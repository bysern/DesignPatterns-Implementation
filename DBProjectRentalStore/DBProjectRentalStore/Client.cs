using System;

namespace DBProjectRentalStore
{
    class Client
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }

        public Client(int id, string firstName, string lastName, DateTime birthday)
        {
            this.ID = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Birthday = birthday;
        }


        public override string ToString()
        {
            
            return $"Client with {ID} id - {FirstName} {LastName} was born in {Birthday.Year} and has: ";
        }
    }
}
