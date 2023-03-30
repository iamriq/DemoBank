using BankingApp.Models.Database.Tables;
using BankingApp.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Actions
{
    public class CustomerOps
    {
        public string CustomerOp()
        {
            using var db = new BankDbContext();
            db.Database.Migrate();

            var lastAccountNumber = db.Customers.OrderByDescending
                (x => x.AccountNumber).Select(u => u.AccountNumber).FirstOrDefault() ?? "0";

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Would you like to register or login? (r/l)");

                var input = Console.ReadLine();

                if (input.ToLower() == "r")
                {
                    Console.Clear();
                    Console.WriteLine("Please eneter your Firstname: ");
                    var firstname = Console.ReadLine();

                    Console.WriteLine("Please enter your lastname: ");
                    var lastname = Console.ReadLine();

                    Console.WriteLine("Please enter your email: ");
                    var email = Console.ReadLine();

                    Console.WriteLine("Please enter a password: ");
                    var password = Console.ReadLine();

                    Console.WriteLine("Please enter your phone number: ");
                    var phoneNumber = Console.ReadLine();

                    var newAccountNumber = Utilities.GenerateAccountNumber();


                    // Create a new user and add it to the database
                    var customer = new CustomerTb
                    {
                        FirstName = firstname,
                        LastName = lastname,
                        Email = email,
                        Password = password,
                        AccountNumber = newAccountNumber,
                        PhoneNumber = phoneNumber,
                        Balance = 0
                    };

                    db.Customers.Add(customer);
                    db.SaveChanges();

                    
                    Console.WriteLine($"User '{firstname}, {lastname}' registered successfully with account number {newAccountNumber}!");
                    Console.ReadLine();


                    //Call bankOps menu here
                    Console.Clear();
                    var bankOps = new BankOps();
                    bankOps.Menu();

                }
                else if (input.ToLower() == "l")
                {
                    Console.Clear();
                    Console.WriteLine("Please enter your email:");
                    var email = Console.ReadLine();

                    Console.WriteLine("Please enter your password:");
                    var password = Console.ReadLine();

                    // Look up the user in the database by their username and password
                    var user = db.Customers.FirstOrDefault(u => u.Email == email && u.Password == password);

                    if (user == null)
                    {
                        Console.Clear();
                        Console.WriteLine("Login failed. Please try again.");
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine($"Welcome back, {user.FirstName}!, Your account number is {user.AccountNumber}.");
                        //Call bankOps menu here
                        var bankOps = new BankOps();
                        bankOps.Menu();

                        Console.ReadLine();
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please try again.");
                }
            }
        }
    }    
}
