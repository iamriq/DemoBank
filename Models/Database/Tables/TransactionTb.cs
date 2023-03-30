using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Models.Database.Tables
{
    public class TransactionTb
    {
        public int Id { get; set; }
        public string SourceAccount { get; set; }
        public string DestinationAccount { get; set; }
        public decimal Amount { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; internal set; }
        public DateTime Timestamp { get; set; }
        public string TransactionId { get; set; }
    }
}
