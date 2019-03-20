using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TripCalculatorFrontEndWpfApp
{
    class WebServiceClient
    {
        string PostURLString = ConfigurationManager.AppSettings["PostURL"];
        string GetURLString = ConfigurationManager.AppSettings["GetURLBase"];

        public async Task<bool> PostCost(string user, float cost)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(PostURLString);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string jsonString = "{\"Name\":\"" + user + "\",\"CostsList\":[\"" + cost + "\"]}";
            StringContent jsonContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = await client.PostAsync(PostURLString, jsonContent);
            if (responseMessage.IsSuccessStatusCode)
                return true;
            else
                return false;
        }

        public async Task<List<MoneyTransferred>> GetWhoOwesWhoAsync()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(GetURLString);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage responseMessage = await client.GetAsync(GetURLString);
            string result = await responseMessage.Content.ReadAsStringAsync();
            List<MoneyTransferred> ListOfMoneyTransferred = new List<MoneyTransferred>();
            if (result == null || result.Equals("null"))
                return ListOfMoneyTransferred;
            JArray jArray = JArray.Parse(result);
            foreach(JToken token in jArray)
            {
                ListOfMoneyTransferred.Add(token.ToObject<MoneyTransferred>());
            }
            return ListOfMoneyTransferred;
        }
    }
}
