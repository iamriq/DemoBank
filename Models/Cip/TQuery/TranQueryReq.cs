using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Models.Cip.TQuery
{
    public class TranQueryReq
    {
        public string AccountNumber { get; set; }
        public string TransactionId { get; set;}
    }
}
