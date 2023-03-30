using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Actions
{
    public class Utilities
    {
        private static int lastAccountNumber = 0;
        public static string GenerateAccountNumber()
        {
            lastAccountNumber++;
            return "1" + lastAccountNumber.ToString("D9");

        }
    }
}
