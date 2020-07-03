using System;
using System.Collections.Generic;
using System.Linq;

namespace DBProjectRentalStore
{
    class Program
    {
        static void Main(string[] args)
        {
            Movie movie = MovieMapper.Instance.GetByID(2);
            Console.WriteLine(movie);
            Console.WriteLine("Type:\n 1 to display all the movies \n 2 to see client's rentals \n 3 to rent a copy of a movie \n 4 to return copy of the movie" +
                " \n 5 to create new user \n 6 to create new movie with a copy \n 7 to display statistics \n 8 to display overdue rentals" );
            string answer = Console.ReadLine();

            //Rental rent = RentalMapper.Instance.GetByID(3);
            switch (answer)
            {

                case "1":
                    Console.Clear();
                    var mapper = MovieMapper.Instance.GetAllMovies();
                    foreach (var mv in mapper)
                    {
                        Console.WriteLine(mv);
                    }
                    break;


                case "2":
                    Console.Clear();
                    Console.WriteLine("Type client id");
                    string inputClientId = Console.ReadLine();
                    if (!int.TryParse(inputClientId, out int clientid)) Console.WriteLine("wrong input");

                    Console.WriteLine(ClientMapper.Instance.GetByID(clientid));

                    Console.WriteLine();


                    foreach (var rental in RentalMapper.Instance.GetByClientId(clientid))
                    {
                        Console.WriteLine(rental);
                    }
                    break;

                case "3":
                    Console.Clear();
                    Console.WriteLine("Type copy id of movie you want to rent");
                    if (!int.TryParse(Console.ReadLine(), out int inputCopyId)) Console.WriteLine("Wrong input");

                    DateTime currentDate = DateTime.Now;
                    Rental rent = new Rental(inputCopyId, 3, currentDate);

                    RentalMapper.Instance.Save(rent);

                    Console.WriteLine("Movie rented at " + currentDate);
                    break;

                case "4":
                    Console.Clear();
                    Console.WriteLine("Type copy id of movie you want to return");
                    if (!int.TryParse(Console.ReadLine(), out int inputCopyid)) Console.WriteLine("wrong input");


                    DateTime dateNow = DateTime.Now;
                    Rental Rental = new Rental(inputCopyid, dateNow);

                    RentalMapper.Instance.Save(Rental);

                    Console.WriteLine("Movie returned");

                    break;

                case "5":
                    Console.Clear();
                    Console.WriteLine("Type your first name");
                    string inputFName = Console.ReadLine();

                    Console.WriteLine("Your last name");
                    string inputLName = Console.ReadLine();

                    Console.WriteLine("Input your birthday in format day/month/year");
                    string inputBirthday = Console.ReadLine();
                    DateTime birthday = Convert.ToDateTime(inputBirthday);

                    ClientMapper.Instance.GetAllClients();
                    var nextClientId = ClientMapper.Instance.Clients.Max(x => x.ID) + 1;


                    Client client = new Client(nextClientId, inputFName, inputLName, birthday);
                    ClientMapper.Instance.Save(client);
                    Console.WriteLine("Client user created");
                    break;

                case "6":
                    Console.Clear();
                    Console.WriteLine("Enter title of movie: ");
                    var title = Console.ReadLine();
                    Console.WriteLine("Enter age restriction:");
                    if (!int.TryParse(Console.ReadLine(), out int ageRestriction)) Console.WriteLine("wrong input");
                    Console.WriteLine("Enter price: ");
                    if (!int.TryParse(Console.ReadLine(), out int price)) Console.WriteLine("wrong input");
                    Console.WriteLine("Enter year of production: ");
                    if (!int.TryParse(Console.ReadLine(), out int year)) Console.WriteLine("wrong input");

                    List<Copy> copies = new List<Copy>();

                    MovieMapper.Instance.GetAllMovies();
                    var nextMovieID = MovieMapper.Instance.GetAllMovies().Max(x => x.ID) + 1;
                    var nextCopyID = CopyMapper.Instance.GetByMovieId(nextMovieID).Max(x => x.ID) + 1;



                    Movie movie1 = new Movie(nextMovieID, title, year, price, copies);
                    copies.Add(new Copy(nextCopyID, true, nextMovieID));

                    MovieMapper.Instance.Save(movie1);

                    Console.WriteLine("Movie created");

                    break;

                case "7":
                    Console.Clear();
                    Console.WriteLine("Rental statistics: ");

                    RentalMapper.Instance.GetAllRentals();

                    Console.WriteLine($"There were {RentalMapper.Instance.Rentals.Count} rentals made in total ");




                    break;

                case "8":
                    Console.Clear();
                    Console.WriteLine("Rentals that are overdue by 14days are: ");

                    var rentals = RentalMapper.Instance.GetAllRentals();

                    DateTime nowDate = DateTime.Now;
                    var display = rentals
                        .Where(r => ((nowDate - r.DateOfRental).TotalDays > 14) && r.DateOfReturn == null);


                    foreach (var item in display)
                    {
                        Console.WriteLine(item);
                    }

                    break;



                default:
                    Console.Clear();
                    break;
            }
        }
    }
}
