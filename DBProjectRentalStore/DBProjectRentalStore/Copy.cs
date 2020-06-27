using System;
using System.Collections.Generic;
using Npgsql;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBProjectRentalStore
{
    class Copy
    {
        public int ID { get; private set; }
        public bool Available { get; private set; }
        public int MovieId { get; private set; }


        public Copy(int id, bool available, int movieId)
        {
            this.ID = id;
            this.Available = available;
            this.MovieId = movieId;
        }

        
    }
}
