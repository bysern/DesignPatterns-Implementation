using Npgsql;
using System.Collections.Generic;

namespace ActiveRecord_Implementation
{
    class CopyRecord
    {
        private static readonly string ConnectionString =
            System.Configuration.ConfigurationManager.ConnectionStrings["Rental"].ToString();
        public int ID { get; set; }
        public bool Available { get; set; }
        public int MovieId { get; set; }

        public CopyRecord(int id, bool available, int movieId)
        {
            this.ID = id;
            this.Available = available;
            this.MovieId = movieId;
        }

        public static List<CopyRecord> GetByMovieId(int movieId)
        {
            List<CopyRecord> list = new List<CopyRecord>();

            using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();
                using (var command = new NpgsqlCommand("SELECT * FROM copies WHERE movie_id = @movieID", conn))
                {
                    command.Parameters.AddWithValue("@movieId", movieId);

                    NpgsqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        list.Add(new CopyRecord((int)reader["copy_id"], (bool)reader["available"], (int)reader["movie_id"]));
                    }
                }
            }
            return list;
        }

        public void Save()
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();
                using (var command = new NpgsqlCommand("INSERT INTO copies(copy_id, available, movie_id) " +
                    "VALUES (@ID, @available, @movieId) " +
                    "ON CONFLICT (copy_id) DO UPDATE " +
                    "SET available = @available, movie_id = @movieId", conn))
                {
                    command.Parameters.AddWithValue("@ID", ID);
                    command.Parameters.AddWithValue("@available", Available);
                    command.Parameters.AddWithValue("@movieId", MovieId);
                    command.ExecuteNonQuery();
                }
            }
        }


        public void Remove()
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();
                using (var command = new NpgsqlCommand("DELETE FROM copies WHERE copy_id = @ID", conn))
                {
                    command.Parameters.AddWithValue("@ID", ID);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
