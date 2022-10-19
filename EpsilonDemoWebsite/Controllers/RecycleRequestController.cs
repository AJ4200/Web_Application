using EpsilonDemoWebsite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace EpsilonDemoWebsite.Controllers
{
    public class RecycleRequestController : Controller
    {
        private readonly string BASE_URL = "http://localhost:7157/api";

        public async Task<IActionResult> Index()
        {
            if (checksessions(HttpContext.Request.Path) == false)
            {
                return RedirectToAction("login");
            }
            ViewData["name"] = HttpContext.Session.GetString("Name");

            List<RecycleRequestView> requests = new List<RecycleRequestView>();
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(clientHandler))
            {
                client.BaseAddress = new Uri(BASE_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getdata = await client.GetAsync("api/Logs/RecycleRequestView");

                if (getdata.IsSuccessStatusCode)
                {
                    string response = await getdata.Content.ReadAsStringAsync();
                    requests = JsonConvert.DeserializeObject<List<RecycleRequestView>>(response);
                }

            }
            return View(requests);
        }

        [HttpPost]
        public async Task<IActionResult> Complete(IFormCollection form)
        {
            List<RecycleRequestView> requests = new List<RecycleRequestView>();
            string recycler = form["recycler"];
            Guid request = Guid.Parse(form["request"]);

            HttpClientHandler clientHandler1 = new HttpClientHandler();
            using (var client = new HttpClient(clientHandler1))
            {
                client.BaseAddress = new Uri(BASE_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getRC = await client.PostAsync("api/Procedures/RecyclerFound/" + request + "/" + recycler, null);
                if (getRC.IsSuccessStatusCode)
                {
                    HttpResponseMessage getdata = await client.GetAsync("api/Logs/RecycleRequestView");

                    if (getdata.IsSuccessStatusCode)
                    {
                        string response = await getdata.Content.ReadAsStringAsync();
                        requests = JsonConvert.DeserializeObject<List<RecycleRequestView>>(response);
                        return View("Index", requests);
                    }
                }
            }
            return View(requests);
        }

        public async Task<IActionResult> Complete(Guid id)
        {
            if (checksessions(HttpContext.Request.Path) == false)
            {
                return RedirectToAction("login");
            }
            ViewData["name"] = HttpContext.Session.GetString("Name");

            RecycleRequestView recReq = new RecycleRequestView(); ;
            List<string> recyclers = new List<string>();
            HttpClientHandler clientHandler1 = new HttpClientHandler();
            using (var client1 = new HttpClient(clientHandler1))
            {
                client1.BaseAddress = new Uri(BASE_URL);
                client1.DefaultRequestHeaders.Accept.Clear();
                client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getRC = await client1.GetAsync("api/Locations/Recyclers/");
                if (getRC.IsSuccessStatusCode)
                {
                    var response = getRC.Content.ReadAsStringAsync().Result;
                    var recyclersInDB = JsonConvert.DeserializeObject<List<Recycler>>(response);
                    foreach (var recycler in recyclersInDB) { recyclers.Add(recycler.Name); }
                }

                HttpResponseMessage getRR = await client1.GetAsync("api/Logs/RecycleRequestView/" + id);
                if (getRR.IsSuccessStatusCode)
                {
                    var response = getRR.Content.ReadAsStringAsync().Result;
                    var recyclerReq = JsonConvert.DeserializeObject<RecycleRequestView>(response);
                    recReq = recyclerReq;
                }
            }
            ViewData["Recyclers"] = recyclers;

            return View(recReq);
        }
        public bool checksessions(string returnUrl)
        {
            bool hasAccess = false;
            string temp = HttpContext.Session.GetString("Name");


            if (!temp.Equals(null))
            {
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
            return hasAccess;
        }
    }
}
