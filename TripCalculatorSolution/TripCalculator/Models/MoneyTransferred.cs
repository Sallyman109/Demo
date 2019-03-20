using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TripCalculator.Models
{
    public class MoneyTransferred
    {
        public string Payor { get; set; }
        public string Payee { get; set; }
        public double Amount { get; set; }
    }
}