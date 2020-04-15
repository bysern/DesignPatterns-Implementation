using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveRecord_Implementation
{
    class MovieRecord
    {
        private static readonly string Connection_String =
            System.Configuration.ConfigurationManager.ConnectionStrings["Rental"].ToString();
        public int ID { get; private set; }
        public string Title { get; private set; }
        public int Year { get;private set; }
        public double Price { get; private set; }
        public List<CopyRecord> Copies { get; private set; }

        public MovieRecord(int id, string title, int year, double price, List<CopyRecord> copies = null)
        {
            this.ID = id;
            this.Title = title;
            this.Year = year;
            this.Price = price;
            this.Copies = copies;
        }

    }
}
