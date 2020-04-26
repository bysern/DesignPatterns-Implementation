using Npgsql;
using System;
using System.Collections.Generic;

namespace ActiveRecord_Implementation
{
    class MovieRecord
    {
        private static readonly string ConnectionString =
            System.Configuration.ConfigurationManager.ConnectionStrings["Rental"].ToString();
        public int ID { get; private set; }
        public string Title { get; private set; }
        public int Year { get;private set; }
        public double Price { get; private set; }
        public List<CopyRecord> Copies { get; private set; }
        public List<MovieRecord> Movies = new List<MovieRecord>();


        public MovieRecord(int id, string title, int year, double price, List<CopyRecord> copies = null)
        {
            this.ID = id;
            this.Title = title;
            this.Year = year;
            this.Price = price;
            this.Copies = copies;
        }

        public override string ToString()
        {
            return $"Movie {ID}: {Title} produced in {Year} costs {Price} and has {Copies.Count} copies";
        }

        public static MovieRecord GetByID(int id)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();
                using(var command = new NpgsqlCommand("SELECT * FROM movies WHERE movie_id = @ID", conn))
                {
                    command.Parameters.AddWithValue("@ID", id);

                    NpgsqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        double pp = Convert.ToDouble(reader["price"]);
                        var copies = CopyRecord.GetByMovieId(id);
                        return new MovieRecord(id, (string)reader["title"], (int)reader["year"], pp, copies);
                    }

                    else return null;
                }
            }
        }

        public List<MovieRecord> GetAllMovies()
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
            {

                conn.Open();
                using (var command = new NpgsqlCommand("SELECT * FROM movies", conn))
                {
                    NpgsqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        
                        while (reader.Read())
                        {
                            double pp = Convert.ToDouble(reader["price"]);
                            int id = (int)reader["movie_id"];
                            var copies = CopyRecord.GetByMovieId(id);
                            Movies.Add(new MovieRecord(id, (string)reader["title"], (int)reader["year"], pp, copies));
                        }
                    }
                }
                return Movies;
            }
        }

        public void Save()
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();
                // if record doesnt exist its updated
                using (var command = new NpgsqlCommand("INSERT INTO movies(movie_id, title, year, price) " +
                    "VALUES (@ID, @title, @year, @price) " +
                    "ON CONFLICT (movie_id) DO UPDATE " +
                    "SET title = @title, year = @year, price = @price", conn))
                {
                    command.Parameters.AddWithValue("@ID", ID);
                    command.Parameters.AddWithValue("@title", Title);
                    command.Parameters.AddWithValue("@year", Year);
                    command.Parameters.AddWithValue("@price", Price);
                    command.ExecuteNonQuery();
                }

                // saving every copy
                // "?" symbol -  protection from NullReferenceException, copies might be empty
                Copies?.ForEach(obj => obj.Save());
            }
        }



    }
}
