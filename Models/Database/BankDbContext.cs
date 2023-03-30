using BankingApp.Models.Database.Tables;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Models.Database
{
    public class BankDbContext : DbContext
    {
        public DbSet<CustomerTb> Customers { get; set; }
        public DbSet<TransactionTb> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=.;Initial catalog=BankAppDb;Integrated Security=true;TrustServerCertificate=True;");
        }
    }
}