using Npgsql;
using System;
using System.Collections.Generic;

namespace DBProjectRentalStore
{
    class RentalMapper : IMapper<Rental>
    {
        private static readonly string ConnectionString =
        System.Configuration.ConfigurationManager.ConnectionStrings["Rental"].ToString();

        public List<Rental> Rentals = new List<Rental>();

        //singleton
        public static RentalMapper Instance { get; } = new RentalMapper();
        private RentalMapper()
        {

        }

        public List<Rental> GetByClientId(int clientId)
        {
            List<Rental> rentals = new List<Rental>();
            using(NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();
                using(var command = new NpgsqlCommand("SELECT * FROM rentals WHERE client_id = @clientID", conn))
                {
                    command.Parameters.AddWithValue("@clientID", clientId);

                    NpgsqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        rentals.Add(new Rental((int)reader["copy_id"], (int)reader["client_id"],
                            (DateTime)reader["date_of_rental"], (DateTime)reader["date_of_return"]));
                    }
                }
            }


            return rentals;
        }


        public List<Rental> GetAllRentals()
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
            {

                conn.Open();
                using (var command = new NpgsqlCommand("SELECT * FROM rentals", conn))
                {
                    NpgsqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {

                        while (reader.Read())
                        {
                            DateTime dateOfRental = Convert.ToDateTime(reader["date_of_rental"]);
                            int id = (int)reader["copy_id"];
                            //var copies = CopyMapper.Instance.GetByMovieId(id);
                            Rentals.Add(new Rental(id, dateOfRental));
                        }
                    }
                }
                return Rentals;
            }
        }





        public void Delete(Rental t)
        {
            throw new NotImplementedException();
        }

        public Rental GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public void Save(Rental rental)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();
                // This is an UPSERT operation - if record doesn't exist in the database it is created, otherwise it is updated
                using (var command = new NpgsqlCommand("INSERT INTO rentals(copy_id, client_id, date_of_rental, date_of_return) " +
                    "VALUES (@copyId, @clientId, @dateOfRental, @dateOfReturn) " +

                    "ON CONFLICT (copy_id, client_id) DO UPDATE " +

                    "SET date_of_rental = @dateOfRental, date_of_return = @dateOfReturn", conn))
                {
                    command.Parameters.AddWithValue("@copyId", rental.CopyId);   ;
                    command.Parameters.AddWithValue("@clientId", rental.ClientId); 
                    command.Parameters.AddWithValue("@dateOfRental", rental.DateOfRental);
                    command.Parameters.AddWithValue("@dateOfReturn", rental.DateOfReturn);

                    command.ExecuteNonQuery();
                }
            }
        }



    }
}
