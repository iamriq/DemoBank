using BankingApp.Actions;
using BankingApp.Models.Database;
using BankingApp.Models.Database.Tables;
using Microsoft.EntityFrameworkCore;

namespace BankingApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to EA Bank");
            var customerOp = new CustomerOps();
            customerOp.CustomerOp();
            //var bankOp = new BankOps();
            //bankOp.Menu();
        }
    }
}