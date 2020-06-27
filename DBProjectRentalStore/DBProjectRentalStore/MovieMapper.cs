using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBProjectRentalStore
{
    class MovieMapper : IMapper<Movie>
    {
        private static readonly string ConnectionString =
        System.Configuration.ConfigurationManager.ConnectionStrings["Rental"].ToString();
        public List<Movie> Movies = new List<Movie>();
        private readonly Dictionary<int, Movie> _cache = new Dictionary<int, Movie>();

        //singleton
        public static MovieMapper Instance { get; } = new MovieMapper();
        private MovieMapper()
        {

        }




        public List<Movie> GetAllMovies()
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
                            var copies = CopyMapper.Instance.GetByMovieId(id);
                            Movies.Add(new Movie(id, (string)reader["title"], (int)reader["year"], pp, copies));
                        }
                    }
                }
                return Movies;
            }
        }



        public Movie GetByID(int id)
        {
            if (_cache.ContainsKey(id))
            {
                return _cache[id];
            }
            Movie movie = GetByIDFromDB(id);
            _cache.Add(movie.ID, movie);
            return movie;
        }

        private Movie GetByIDFromDB(int id)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();
                using (var command = new NpgsqlCommand("SELECT * FROM movies WHERE movie_id = @ID", conn))
                {
                    command.Parameters.AddWithValue("@ID", id);

                    NpgsqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        double pp = Convert.ToDouble(reader["price"]);
                        var copies = CopyMapper.Instance.GetByMovieId(id);
                        return new Movie(id, (string)reader["title"], (int)reader["year"], pp, copies);
                    }
                }
            }
            return null;
        }



        public void Save(Movie movie)
        {
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    var transaction = conn.BeginTransaction();

                    // This is an UPSERT operation - if record doesn't exist in the database it is created, otherwise it is updated
                    using (var command = new NpgsqlCommand("INSERT INTO movies(movie_id, title, year, price) " +
                        "VALUES (@ID, @title, @year, @price) " +
                        "ON CONFLICT (movie_id) DO UPDATE " +
                        "SET title = @title, year = @year, price = @price", conn))
                    {
                        command.Parameters.AddWithValue("@ID", movie.ID);
                        command.Parameters.AddWithValue("@title", movie.Title);
                        command.Parameters.AddWithValue("@year", movie.Year);
                        command.Parameters.AddWithValue("@price", movie.Price);
                        command.ExecuteNonQuery();
                    }
                    // We need to save every copy in our list. 
                    // ="?" symbol - protection from NullReferenceException
                    movie.Copies?.ForEach(obj => CopyMapper.Instance.Save(obj));



                    transaction.Commit();

                    
                }
                _cache[movie.ID] = movie;
            }
        }

            public void Delete(Movie t)
            {
                throw new NotImplementedException();
            }
        }
    } 

