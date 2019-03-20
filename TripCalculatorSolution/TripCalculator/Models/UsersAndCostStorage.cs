using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TripCalculator.Models
{
    public class UsersAndCostStorage
    {
        public UsersAndCostStorage() { }
        private Dictionary<string, List<float>> UsersAndCostDictionary = new Dictionary<string, List<float>>();
        private static UsersAndCostStorage usersAndCostStorage;

        public static UsersAndCostStorage GetInstance()
        {
            if (usersAndCostStorage == null)
            {
                usersAndCostStorage = new UsersAndCostStorage();
                return usersAndCostStorage;
            }
            else
                return usersAndCostStorage;
        }

        public void StoreUser(UserAndCost userAndCost)
        {
            if (string.IsNullOrEmpty(userAndCost.Name) || userAndCost.CostsList == null)
                throw new ApplicationException("Name and CostsLists need to be set");
            List<float> ListOfCosts = new List<float>();
            if (!UsersAndCostDictionary.TryGetValue(userAndCost.Name, out ListOfCosts))
            {
                UsersAndCostDictionary.Add(userAndCost.Name, userAndCost.CostsList);
            }
            else
            {
                foreach (float cost in userAndCost.CostsList)
                {
                    ListOfCosts.Add(cost);
                }
            }
        }

        public List<float> GetUserCosts(string userFullName)
        {
            List<float> ListOfCosts = new List<float>();
            UsersAndCostDictionary.TryGetValue(userFullName, out ListOfCosts);
            return ListOfCosts;
        }

        public double GetUserTotalCost(string userFullName)
        {
            List<float> ListOfCosts = new List<float>();
            UsersAndCostDictionary.TryGetValue(userFullName, out ListOfCosts);
            return Math.Round(ListOfCosts.Sum(), 2);
        }

        public List<string> GetListOfUsers()
        {
            return UsersAndCostDictionary.Keys.ToList();
        }
    }
}