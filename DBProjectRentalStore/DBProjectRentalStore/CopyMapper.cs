using Npgsql;
using System;
using System.Collections.Generic;

namespace DBProjectRentalStore
{
    class CopyMapper : IMapper<Copy>
    {
        private static readonly string ConnectionString =
        System.Configuration.ConfigurationManager.ConnectionStrings["Rental"].ToString();

        //singleton
        public static CopyMapper Instance { get; } = new CopyMapper();
        private CopyMapper()
        {

        }


        public List<Copy> GetByMovieId(int movieId)
        {
            List<Copy> copies = new List<Copy>();

            using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();
                using (var command = new NpgsqlCommand("SELECT * FROM copies WHERE movie_id = @movieID", conn))
                {
                    command.Parameters.AddWithValue("@movieId", movieId);

                    NpgsqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        copies.Add(new Copy((int)reader["copy_id"], (bool)reader["available"], (int)reader["movie_id"]));
                    }
                }
            }
            return copies;
        }


        public Copy GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public void Save(Copy copy)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();
                // This is an UPSERT operation - if record doesn't exist in the database it is created, otherwise it is updated
                using (var command = new NpgsqlCommand("INSERT INTO copies(copy_id, available, movie_id) " +
                    "VALUES (@ID, @available, @movieId) " +
                    "ON CONFLICT (copy_id) DO UPDATE " +
                    "SET available = @available, movie_id = @movieId", conn))
                {
                    command.Parameters.AddWithValue("@ID", copy.ID);
                    command.Parameters.AddWithValue("@available", copy.Available);
                    command.Parameters.AddWithValue("@movieId", copy.MovieId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(Copy t)
        {
            throw new NotImplementedException();
        }
    }
}
