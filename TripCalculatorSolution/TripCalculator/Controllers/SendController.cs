using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TripCalculator.Models;

namespace TripCalculator.Controllers
{
    public class SendController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage AddUserAndCosts(UserAndCost userAndCosts)
        {
            UsersAndCostStorage usersAndCostStorage = UsersAndCostStorage.GetInstance();
            usersAndCostStorage.StoreUser(userAndCosts); //} catch (Exception) { return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error Occurred while storing user and cost(s)"); }
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
