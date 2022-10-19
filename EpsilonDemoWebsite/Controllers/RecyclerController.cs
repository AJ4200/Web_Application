using EpsilonDemoWebsite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace EpsilonDemoWebsite.Controllers
{
    public class RecyclerController : Controller
    {
        private readonly string BASE_URL = "http://localhost:7157/api";

        public async Task<ActionResult> Index(string search)

        {
            if (checksessions(HttpContext.Request.Path) == false)
            {
                return RedirectToAction("login");
            }
            ViewData["name"] = HttpContext.Session.GetString("Name");

            List<Recycler> recyclers = new List<Recycler>();
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(clientHandler))
            {
                client.BaseAddress = new Uri(BASE_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getdata = await client.GetAsync("api/Locations/Recyclers/");

                if (getdata.IsSuccessStatusCode)
                {
                    string response = await getdata.Content.ReadAsStringAsync();
                    recyclers = JsonConvert.DeserializeObject<List<Recycler>>(response);

                   
                    
                    if (search != null)
                    {
                        List<Recycler> list = recyclers.Where(x => x.Name.ToLower().Contains(search.ToLower()) || search == null).ToList();
                        
                        return View(list);
                    }
                }

            }
            return View(recyclers);
        }
        public async Task<ActionResult> Add()
        {

            return View();

        }
        [HttpPost]
        public async Task<ActionResult> Add(IFormCollection Form)
        {
            Random r = new Random();
            int rand = r.Next(820, 999);
            string locid = "RS" + rand;
            string dtype = "Recycling Station";
            //string dtype = form["dtype"];
            string addr = Form["address"];
            var longitude = Form["longitude"];

            var latitude = Form["latitude"];
            var locToAdd = new Location(dtype.Trim(), addr, locid, longitude, latitude, null, true);

            //Api call
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(clientHandler))
            {
                client.BaseAddress = new Uri(BASE_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var result = client.PostAsJsonAsync("api/Locations/", locToAdd).Result;
                if (result.IsSuccessStatusCode)
                {
                    var recycler = new Recycler
                             (
                                  locid,
                                  Form["Name"],
                                  checkBool(Form["IndustrialWaste"]),
                                  checkBool(Form["CardboardandPaper"]),
                                  checkBool(Form["Plastic"]),
                                  checkBool(Form["Glass"]),
                                  checkBool(Form["GardenWaste"]),
                                  checkBool(Form["GeneralWaste"]),
                                  true
                             );

                    HttpClientHandler clientHandler2 = new HttpClientHandler();
                    clientHandler2.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                    using (var client2 = new HttpClient(clientHandler2))
                    {
                        client2.BaseAddress = new Uri(BASE_URL);
                        client2.DefaultRequestHeaders.Accept.Clear();
                        client2.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                        var response = client2.PostAsJsonAsync("api/Locations/Recycler", recycler).Result;



                        if (response.IsSuccessStatusCode)
                        {
                            string results = response.Content.ReadAsStringAsync().Result;
                            // user = JsonConvert.DeserializeObject<Staff>(results);
                        }

                    }
                }

            }
            



            return View();

        }
        public bool checkBool(string value)
        {
            if (value == "on")
            {
                return true;
            }
            else
            {
                return false;
            }
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
