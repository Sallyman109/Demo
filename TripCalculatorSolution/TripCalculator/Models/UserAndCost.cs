using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TripCalculator.Models
{
    public class UserAndCost
    {
        public string Name { get; set; }
        public List<float> CostsList { get; set; }
    }
}