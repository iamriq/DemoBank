using BankingApp.Models.Cip.Balance;
using BankingApp.Models.Cip.FundsTransfer;
using BankingApp.Models.Cip.TQuery;
using BankingApp.Models.Database;
using BankingApp.Models.Database.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BankingApp.Actions
{
    public class BankOps
    {
        private readonly BankDbContext db = new();
        
        public void Menu()
        {
            Console.WriteLine("Bank Transactions:");
            Console.WriteLine("------------------");
            Console.WriteLine("1. Fund Transfer");
            Console.WriteLine("2. Transaction Query");
            Console.WriteLine("3. Check Balance");
            Console.WriteLine("4. Logout");

            Console.Write("\nEnter selection: ");
            var selection = Console.ReadLine();

            switch(selection)
            {
                case "1": FundTransfer(); break;
                case "2": TransactionQuery(); break;
                case "3": Balance(); break;
                case "4": Environment.Exit(0); break;
            }
        }

        public FundTransResponse FundTransfer()
        {
            try
            {
                Console.Clear();
                Console.Write("Enter Sender's account number: ");
                var SourceAccount = Console.ReadLine();

                Console.Write("Enter Recipient's account number: ");
                var DestinationAccount = Console.ReadLine();

                Console.Write("Enter Amount: ");
                var Amount = Convert.ToDecimal(Console.ReadLine());

                var sender = db.Customers.Where(u => u.AccountNumber == SourceAccount).FirstOrDefault();
                var receiver = db.Customers.Where(u => u.AccountNumber == DestinationAccount).FirstOrDefault();
                //var receiver = db.Customers.FirstOrDefault(u => u.AccountNumber == DestinationAccount);

                
                if (sender == null || receiver == null)
                {
                    Console.WriteLine("Source account or destination account is invalid.");
                    Console.WriteLine($"Transfer of N{Amount} to {DestinationAccount} Failed.");
                    Console.ReadLine();

                    //return new FundTransResponse
                    //{
                    //    ResponseCode = "01",
                    //    ResponseMessage = "One or more account number is invalid"
                    //};
                }

                if(sender.Balance < Amount || sender.Balance == Amount)
                {
                    Console.WriteLine("Transaction Failed. Insufficient Funds");
                    Console.ReadLine();

                    return new FundTransResponse
                    {
                        ResponseCode = "01",
                        ResponseMessage = "Insufficient Funds",
                    };
                }

                sender.Balance -= Amount;
                db.Customers.Update(sender);

                receiver.Balance += Amount;
                db.Customers.Update(receiver);

                Random generator = new Random();
                var tranId = generator.Next(0, 100000000).ToString("D9");
                 
                var transaction = new TransactionTb
                {
                    SourceAccount = SourceAccount,
                    DestinationAccount = DestinationAccount,
                    Amount = Amount,
                    TransactionId = tranId,

                    Timestamp = DateTime.Now,
                    ResponseCode = "00",
                    ResponseMessage = "Success"
                    
                };
                db.Transactions.Add(transaction);
                db.SaveChanges();

                Console.WriteLine($"\nTransfer of N{Amount} to Account Number: {DestinationAccount} is Successful.");
                Console.ReadLine();

                Console.Clear();
                Menu();

                return new FundTransResponse
                {
                    ResponseCode = "00",
                    ResponseMessage = "Success"
                };


            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
                return new FundTransResponse
                {
                    ResponseCode = "01",
                    ResponseMessage = Ex.Message
                };
                
            }
        }

        public BalanceResponse Balance()
        {
            try
            {
                Console.WriteLine("Enter your account number: ");
                var AccountNumber = Console.ReadLine();
                
                var customer = db.Customers.SingleOrDefault(c => c.AccountNumber == AccountNumber);
                if (customer == null)
                {
                    Console.WriteLine("Error! Invalid Account Number");
                    Console.ReadLine();

                    return new BalanceResponse
                    {
                        ResponseCode = "01",
                        ResponseMessage = "Invalid Account Number"
                    };
                }
                else
                {
                    Console.WriteLine("Balance Enquiry Successful.");
                    Console.WriteLine($"Your Balance is: {customer.Balance}");
                    Console.ReadLine();

                    Console.Clear();
                    Menu();

                    return new BalanceResponse
                    {
                        Balance = customer.Balance,
                        ResponseCode = "00",
                        ResponseMessage = "Balance Enquiry Successful"
                    };
                }

            }
            catch (Exception Ex)
            {

                return new BalanceResponse
                {
                    ResponseCode = "01",
                    ResponseMessage = Ex.Message
                    
                };
            }

        }

        public TranQueryResp TransactionQuery ()
        {
            try
            {
                Console.Write("Enter the transactionId: ");
                var transactionId = Console.ReadLine();

                
                //var customerTransaction = db.Transactions.FirstOrDefault(t => t.TransactionId == transactionId);
                var customerTransaction = db.Transactions.Where(c => c.TransactionId == transactionId).FirstOrDefault();

                if (customerTransaction == null)
                {
                    return new TranQueryResp
                    {
                        ResponseCode = "01",
                        ResponseMessage = "Transaction not found"
                    };
                }

                Console.Clear();
                Console.WriteLine("Transaction Query Successful.");
                Console.WriteLine($"\nThe transaction details is: " +
                    $"\nAmount = {customerTransaction.Amount}" +
                    $"\nSource Account = {customerTransaction.SourceAccount}" +
                    $"\nDestination Account = {customerTransaction.DestinationAccount}" +
                    $"\nTimestamp = {customerTransaction.Timestamp}" +
                    $"\nTransactionId = {customerTransaction.TransactionId}" +
                    $"\n{"ResponseCode = 00"}" +
                    $"\n{"ResponseMessage = Success"}" +
                    $"");

                Console.ReadLine();

                Console.Clear();
                Menu();

                return new TranQueryResp
                {
                    Amount = customerTransaction.Amount,
                    SourceAccount= customerTransaction.SourceAccount,
                    DestinationAccount= customerTransaction.DestinationAccount,
                    Timestamp = customerTransaction.Timestamp,
                    TransactionId= transactionId,
                    ResponseCode = "00",
                    ResponseMessage = "Success",
                };
            }
            catch (Exception Ex)
            {
                return new TranQueryResp
                {
                    ResponseCode = "01",
                    ResponseMessage = Ex.Message
                };
            }
        }
    }
}