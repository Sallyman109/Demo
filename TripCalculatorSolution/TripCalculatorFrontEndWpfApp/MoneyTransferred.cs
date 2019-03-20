using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TripCalculatorFrontEndWpfApp
{
    public class MoneyTransferred
    {
        public string Payor { get; set; }
        public string Payee { get; set; }
        public double Amount { get; set; }

        public override string ToString()
        {
            return Payor + " pays " + Payee + " $" + Math.Round(Amount, 2);
        }
    }
}