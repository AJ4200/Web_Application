using EpsilonDemoWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace EpsilonDemoWebsite.Controllers
{
    public class BinSlotController : Controller
    {
        string baseURL = "http://localhost:7157/api";
        public async Task<IActionResult> Index(int id)
        {
            if (checksessions(HttpContext.Request.Path) == false)
            {
                return RedirectToAction("login");
            }
            ViewData["name"] = HttpContext.Session.GetString("Name");


            var gardenSite = new GardenSite();
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(clientHandler))
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var getdata = await client.GetAsync("api/Locations/" + id);

                if (getdata.IsSuccessStatusCode)
                {
                    string apiResponse = await getdata.Content.ReadAsStringAsync();
                    gardenSite = JsonConvert.DeserializeObject<GardenSite>(apiResponse);
                }

            }


            return View(gardenSite);
        }
        public bool checksessions(string returnUrl)
        {
            bool hasAccess = false;
            string temp = HttpContext.Session.GetString("Name");


            if (returnUrl != null && !temp.Equals(""))
            {
                hasAccess = true;
                return hasAccess;
            }
            else
            {
                return hasAccess;
            }

        }
    }
}