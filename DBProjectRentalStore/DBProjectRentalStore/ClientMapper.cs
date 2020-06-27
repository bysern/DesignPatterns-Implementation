using Npgsql;
using System;

namespace DBProjectRentalStore
{
    class ClientMapper : IMapper<Client>
    {
        private static readonly string ConnectionString =
        System.Configuration.ConfigurationManager.ConnectionStrings["Rental"].ToString();

        //singleton
        public static ClientMapper Instance { get; } = new ClientMapper();
        private ClientMapper()
        {

        }




        public void Delete(Client t)
        {
            throw new NotImplementedException();
        }

        public Client GetByID(int id)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString)) 
            {
                conn.Open();
                using (var command = new NpgsqlCommand("SELECT * FROM clients WHERE client_id = @ID", conn))
                {
                    command.Parameters.AddWithValue("@ID", id);

                    NpgsqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        return new Client(id, (string)reader["first_name"], (string)reader["last_name"], (DateTime)reader["birthday"]);
                    }
                }
            }
            return null;
        }

        public void Save(Client client)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))

            {

                conn.Open();

                // This is an UPSERT operation - if record doesn't exist in the database it is created, otherwise it is updated

                using (var command = new NpgsqlCommand("INSERT INTO clients(client_id, first_name, last_name, birthday) " +

                    "VALUES (@ID, @firstname, @lastname, @birthday) " +

                    "ON CONFLICT (client_id) DO UPDATE " +

                    "SET first_name = @firstname, last_name = @lastname, birthday = @birthday", conn))

                {

                    command.Parameters.AddWithValue("@ID", client.ID);

                    command.Parameters.AddWithValue("@firstname", client.FirstName);

                    command.Parameters.AddWithValue("@lastname", client.LastName);

                    command.Parameters.AddWithValue("@birthday", client.Birthday);

                    command.ExecuteNonQuery();

                }

            }
        }
    }
}
