using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ScenarioManager.Services
{
    public static class ControllerInfoSender
    {
        public static async Task UpdateAsync(string adress, long scenarioId)
        {
            using (var client = new HttpClient())
            {
                await client.PostAsync(adress + "/api/Update",
                    new StringContent(JsonConvert.SerializeObject(scenarioId)));
            }
        }

        public static async Task ListUpdateAsync(string adress)
        {
            using (var client = new HttpClient())
            {
                await client.GetAsync(adress + "/api/ListUpdate");
            }
        }
        public static async Task DeleteAsync(string adress, long scenarioId)
        {
            using (var client = new HttpClient())
            {
                await client.PostAsync(adress + "/api/Delete",
                    new StringContent(JsonConvert.SerializeObject(scenarioId)));
            }
        }
    }
}
