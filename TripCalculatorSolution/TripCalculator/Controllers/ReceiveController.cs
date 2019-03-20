using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TripCalculator.Models;

namespace TripCalculator.Controllers
{
    public class ReceiveController : ApiController
    {
        [HttpGet]
        public List<MoneyTransferred> WhoOwesWho()
        {
            UsersAndCostStorage usersAndCostStorage = UsersAndCostStorage.GetInstance();
            List<string> ListOfUsers = usersAndCostStorage.GetListOfUsers();

            SortedDictionary<double, List<string>> TotalCostDictionary = new SortedDictionary<double, List<string>>();
            double totalCost = 0.0;
            foreach (string user in ListOfUsers)
            {
                List<string> ListOfCostUsers = new List<string>();
                double totalCostForUser = usersAndCostStorage.GetUserTotalCost(user);
                totalCost += totalCostForUser;
                if (!TotalCostDictionary.TryGetValue(totalCostForUser, out ListOfCostUsers))
                {
                    ListOfCostUsers = new List<string>();
                    ListOfCostUsers.Add(user);
                    TotalCostDictionary.Add(totalCostForUser, ListOfCostUsers);
                }
                else
                {
                    ListOfCostUsers.Add(user);
                }
            }

            double averageCost = totalCost / ListOfUsers.Count;
            
            Stack<string> StackOfUsersWhoPaidMore = new Stack<string>();
            Stack<double> StackOfUsersWhoPaidMoreCorrelatedTotal = new Stack<double>();
            Stack<string> StackOfUsersWhoPaidLess = new Stack<string>();
            Stack<double> StackOfUsersWhoPaidLessCorrelatedTotal = new Stack<double>();
            foreach (KeyValuePair<double, List<string>> pair in TotalCostDictionary.Reverse())
            {
                if (pair.Key > averageCost)
                {
                    foreach (string user in pair.Value)
                    {
                        StackOfUsersWhoPaidMore.Push(user);
                        StackOfUsersWhoPaidMoreCorrelatedTotal.Push(pair.Key);
                    }
                }
                else if (pair.Key < averageCost)
                {
                    foreach (string user in pair.Value)
                    {
                        StackOfUsersWhoPaidLess.Push(user);
                        StackOfUsersWhoPaidLessCorrelatedTotal.Push(pair.Key);
                    }
                }
            }

            List<MoneyTransferred> ListOfMoneyTransferred = new List<MoneyTransferred>();
            while (StackOfUsersWhoPaidMore.Count > 0 && StackOfUsersWhoPaidLess.Count>0)
            {
                string userWhoPaidMore = StackOfUsersWhoPaidMore.Pop();
                double overPaidAmount = StackOfUsersWhoPaidMoreCorrelatedTotal.Pop()-averageCost;
                while (overPaidAmount > 0 && StackOfUsersWhoPaidLess.Count > 0)
                {
                    string userWhoPaidLess = StackOfUsersWhoPaidLess.Pop();
                    double paidAmount = StackOfUsersWhoPaidLessCorrelatedTotal.Pop();
                    double underPaidAmount = averageCost - paidAmount;
                    if (overPaidAmount == underPaidAmount)
                    {
                        MoneyTransferred moneyTransferred = new MoneyTransferred { Payee = userWhoPaidMore, Payor = userWhoPaidLess, Amount = underPaidAmount };
                        ListOfMoneyTransferred.Add(moneyTransferred);
                        overPaidAmount -= underPaidAmount;
                    }
                    else if(overPaidAmount> underPaidAmount)
                    {
                        overPaidAmount -= underPaidAmount;
                        StackOfUsersWhoPaidMore.Push(userWhoPaidMore);
                        StackOfUsersWhoPaidMoreCorrelatedTotal.Push(averageCost + overPaidAmount);

                        MoneyTransferred moneyTransferred = new MoneyTransferred { Payee = userWhoPaidMore, Payor = userWhoPaidLess, Amount = underPaidAmount };
                        ListOfMoneyTransferred.Add(moneyTransferred);
                    }
                    else if (overPaidAmount < underPaidAmount)
                    {
                        paidAmount += overPaidAmount;
                        StackOfUsersWhoPaidLess.Push(userWhoPaidLess);
                        StackOfUsersWhoPaidLessCorrelatedTotal.Push(paidAmount);

                        MoneyTransferred moneyTransferred = new MoneyTransferred { Payee = userWhoPaidMore, Payor = userWhoPaidLess, Amount = overPaidAmount };
                        ListOfMoneyTransferred.Add(moneyTransferred);
                        overPaidAmount = 0;
                    }

                }
            }
            return ListOfMoneyTransferred;
        }
    }
}
