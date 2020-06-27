using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace DBProjectRentalStore
{
    class Program
    {
        static void Main(string[] args)
        {
            Movie movie = MovieMapper.Instance.GetByID(2);
            Console.WriteLine(movie);
            Console.WriteLine("Type 1 to display all the movies, 2 to client");
            string answer = Console.ReadLine();

            //Rental rent = RentalMapper.Instance.GetByID(3);
            switch (answer)
            {

                case "1":
                    var mapper = MovieMapper.Instance.GetAllMovies();
                    foreach (var mv in mapper)
                    {
                        Console.WriteLine(mv);
                    }
                    break;


                case "2":
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
                    Console.WriteLine("Type copy id of movie you want to rent");
                    if (!int.TryParse(Console.ReadLine(), out int inputCopyId)) Console.WriteLine("Wrong input");

                    DateTime currentDate = DateTime.Now;
                    Rental rent = new Rental(inputCopyId, 3, currentDate);

                    RentalMapper.Instance.Save(rent);

                    Console.WriteLine("Movie rented");
                    break;

                case "4":
                    Console.WriteLine("Type copy id of movie you want to return");
                    if (!int.TryParse(Console.ReadLine(), out int inputCopyid)) Console.WriteLine("wrong input");


                    DateTime dateNow = DateTime.Now;
                    Rental Rental = new Rental(inputCopyid, dateNow);

                    RentalMapper.Instance.Save(Rental);

                    break;

                case "5":
                    Console.WriteLine("Type your first name");
                    string inputFName = Console.ReadLine();

                    Console.WriteLine("Your last name");
                    string inputLName = Console.ReadLine();

                    Console.WriteLine("Input your birthday in format day/month/year");
                    string inputBirthday = Console.ReadLine();
                    DateTime birthday = Convert.ToDateTime(inputBirthday);


                    Client client = new Client(30, inputFName, inputLName, birthday);
                    ClientMapper.Instance.Save(client);
                    break;

                case "6":
                    Console.WriteLine("Enter title of movie: ");
                    var title = Console.ReadLine();
                    Console.WriteLine("Enter age restriction:");
                    if (!int.TryParse(Console.ReadLine(), out int ageRestriction)) Console.WriteLine("wrong input");
                    Console.WriteLine("Enter price: ");
                    if (!int.TryParse(Console.ReadLine(), out int price)) Console.WriteLine("wrong input");
                    Console.WriteLine("Enter year of production: ");
                    if (!int.TryParse(Console.ReadLine(), out int year)) Console.WriteLine("wrong input");

                    List<Copy> copies = new List<Copy>();
                    

                    Movie movie1 = new Movie(30, title, year, price, copies);
                    copies.Add(new Copy(28, true, 30));

                    MovieMapper.Instance.Save(movie1);

                    break;


                case "7":
                    Console.WriteLine("Rental statistics: ");

                    RentalMapper.Instance.GetAllRentals();

                    Console.WriteLine($"There were {RentalMapper.Instance.Rentals.Count} rentals made in total ");



                   



                    break;





                case "8":
                    Console.WriteLine("Rentals that are overdue by 14days are: ");

                    var rentals = RentalMapper.Instance.GetAllRentals();

                    DateTime nowDate = DateTime.Now;
                    var display = rentals
                        .Where(r => ((nowDate - r.DateOfRental).TotalDays > 14) && r.DateOfReturn != null);


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
