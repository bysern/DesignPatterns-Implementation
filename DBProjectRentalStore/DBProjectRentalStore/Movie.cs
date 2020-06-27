using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace DBProjectRentalStore
{
    class Movie
    {


        public int ID { get; private set; }
        public string Title { get; private set; }
        public int Year { get; private set; }
        public double Price { get; private set; }
        public List<Copy> Copies { get; private set; }



        public Movie(int id, string title, int year, double price, List<Copy> copies = null)
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


    }
}
