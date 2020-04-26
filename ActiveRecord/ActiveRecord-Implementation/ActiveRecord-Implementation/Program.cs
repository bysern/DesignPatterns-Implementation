using System;

namespace ActiveRecord_Implementation
{
    class Program
    {
        static void Main(string[] args)
        {
            //fetch from db
            MovieRecord mr = MovieRecord.GetByID(2);
            Console.WriteLine(mr.ToString());

            //displaying all movies
            mr.GetAllMovies();
            foreach (var movie in mr.Movies)
            {
                Console.WriteLine(movie);
            }


            // creating and saving new object to database
            mr = new MovieRecord(123, "The Last Samurai", 2003, 10);
            mr.Save();

            Console.WriteLine(MovieRecord.GetByID(123).ToString());

            // We adjust the price
            mr.ChangePrice(4.5);
            Console.WriteLine(MovieRecord.GetByID(123).ToString());



            // Object is removed from the database
            mr.Remove();
            if (MovieRecord.GetByID(123) == null)
            {
                Console.WriteLine("Object is removed from the database");
            }


        }
    }
}
