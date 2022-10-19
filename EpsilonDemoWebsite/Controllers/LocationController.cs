using EpsilonDemoWebsite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http.Headers;



namespace EpsilonDemoWebsite.Controllers
{
    public class LocationController : Controller
    {
        private readonly string BASE_URL = "http://localhost:7157/api";

        public async Task<IActionResult> Index(string id, string search)
        {
            if (checksessions(HttpContext.Request.Path) == false)
            {
                return RedirectToAction("login");
            }
            ViewData["name"] = HttpContext.Session.GetString("Name");


            List<Location> locs = new List<Location>();
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(clientHandler))
            {
                client.BaseAddress = new Uri(BASE_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                switch (id)
                {
                    case "GS":
                        HttpResponseMessage getGSs = await client.GetAsync("api/Locations/GardenSites/");
                        if (getGSs.IsSuccessStatusCode)
                        {
                            var response = getGSs.Content.ReadAsStringAsync().Result;
                            var gardenSites = JsonConvert.DeserializeObject<List<GardenSite>>(response);
                            foreach (var cs in gardenSites) { locs.Add(new Location(null, cs.Address, cs.LocationId, null, null, cs.Supervisor, null)); }

                            //Index action method will return a view with a student records based on what a user specify the value in textbox  
                            if (search != null)
                            {
                                List<Location> list = locs.Where(x => x.Address.ToLower().Contains(search.ToLower()) || search == null).ToList();
                                List<Location> list2 = locs.Where(x => x.Supervisor.ToLower().Contains(search.ToLower()) || search == null).ToList();
                                list.AddRange(list2);
                                return View(list);
                                
                                }
                        }
                        break;
                    case "LF":
                        HttpResponseMessage getLFs = await client.GetAsync("api/Locations/Landfills/");
                        if (getLFs.IsSuccessStatusCode)
                        {
                            var response = getLFs.Content.ReadAsStringAsync().Result;
                            var landfills = JsonConvert.DeserializeObject<List<Landfill>>(response);
                            foreach (var cs in landfills) { locs.Add(new Location(null, cs.Address, cs.LocationId, null, null, null, null)); }


                            //Index action method will return a view with a student records based on what a user specify the value in textbox  
                            if (search != null)
                                return View(locs.Where(x => x.Address.ToLower().Contains(search.ToLower()) || search == null).ToList());



                        }
                        break;
                    case "CS":
                        HttpResponseMessage getCSs = await client.GetAsync("api/Locations/ControlStations/");
                        if (getCSs.IsSuccessStatusCode)
                        {
                            var response = getCSs.Content.ReadAsStringAsync().Result;
                            var controlStations = JsonConvert.DeserializeObject<List<ControlStation>>(response);
                            foreach (var cs in controlStations) { locs.Add(new Location(null, cs.Address, cs.LocationId, null, null, null, null)); }

                            //Index action method will return a view with a student records based on what a user specify the value in textbox  
                            if (search != null)
                                return View(locs.Where(x => x.Address.ToLower().Contains(search.ToLower()) || search == null).ToList());

                        }
                        break;
                    default:
                        break;
                }
            }
            return View(locs);
        }

        [HttpPost]
        public async Task<IActionResult> Add(IFormCollection form)
        {
            Random r = new Random();
            string locid = "";
            int rand;
            string dtype = form["dtype"];
            string addr = form["address"];
            var longitude = form["longitude"];
            var latitude = form["latitude"];
            
            var locToAdd = new Location(dtype.Trim(), addr, locid, longitude, latitude, null, true);
            switch (locToAdd.Dtype)
            {
                case "Control Station":
                    rand = r.Next(10, 99);
                    locToAdd.LocationId = "CS0" + rand;
                    break;
                case "Landfill":
                    rand = r.Next(600, 699);
                    locToAdd.LocationId = "LF" + rand;
                    break;
                case "Garden Site":
                    rand = r.Next(335, 599);
                    locToAdd.LocationId = "GS" + rand;
                    string supervisor = form["supervisor"];
                    locToAdd.Supervisor = supervisor.Trim();
                    break;
                case "Recycling Station":
                    rand = r.Next(820, 999);
                    locToAdd.LocationId = "RS" + rand;
                    break;
                default:
                    break;
            }

            //api call
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
                    if (form["dtype"] == "Garden Site")
                    {

                    }


                }

            }
            List<string> supervisors = new List<string>();
            HttpClientHandler clientHandler2 = new HttpClientHandler();
            clientHandler2.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client2 = new HttpClient(clientHandler2))
            {
                client2.BaseAddress = new Uri(BASE_URL);
                client2.DefaultRequestHeaders.Accept.Clear();
                client2.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getSupervisors = await client2.GetAsync("api/Staffs/Supervisors");
                if (getSupervisors.IsSuccessStatusCode)
                {
                    var response = getSupervisors.Content.ReadAsStringAsync().Result;
                    var supervisorsInDB = JsonConvert.DeserializeObject<List<Supervisor>>(response);
                    foreach (var supervisor in supervisorsInDB) { supervisors.Add(supervisor.StaffId); }
                }
            }
            ViewData["Supervisors"] = supervisors;
            return View();
        }

        // [Route("~/Location/Add/{id}")]
        public async Task<IActionResult> Add(string id)
        {
            if (checksessions(HttpContext.Request.Path) == false)
            {
                return RedirectToAction("login");
            }
            ViewData["name"] = HttpContext.Session.GetString("Name");
            if (id == "GS")
            {
                List<string> supervisors = new List<string>();
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                using (var client = new HttpClient(clientHandler))
                {
                    client.BaseAddress = new Uri(BASE_URL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage getSupervisors = await client.GetAsync("api/Staffs/Supervisors");
                    if (getSupervisors.IsSuccessStatusCode)
                    {
                        var response = getSupervisors.Content.ReadAsStringAsync().Result;
                        var supervisorsInDB = JsonConvert.DeserializeObject<List<Supervisor>>(response);
                        foreach (var supervisor in supervisorsInDB) { supervisors.Add(supervisor.StaffId); }
                    }
                }
                ViewData["Supervisors"] = supervisors;
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(IFormCollection form)
        {
            string dtype = form["dtype"];
            string addr = form["address"];
            string locId = form["locationId"];
            string longitude = form["longitude"];
            string latitude = form["latitude"];
            bool active = Convert.ToBoolean(form["Active"]);
            var locToEdit = new Location(dtype.Trim(), addr, locId.Trim(), longitude.Trim(), latitude.Trim(), null, active);
            switch (locToEdit.Dtype)
            {
                case "Control Station":
                    break;
                case "Landfill":
                    string capacity = form["capacity"];

                    break;
                case "Garden Site":
                    string supervisor = form["supervisor"];
                    locToEdit.Supervisor = supervisor.Trim();
                    break;
                default:
                    return View(locToEdit);
            }
            //api call
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(clientHandler))
            {
                client.BaseAddress = new Uri(BASE_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var result = client.PutAsJsonAsync("api/Locations/" + locToEdit.LocationId + "/", locToEdit).Result;
                if (result.IsSuccessStatusCode)
                {
                    return View(locToEdit);
                }
            }
            return View(locToEdit);
        }
        //[Route("~/Location/Edit/{loc}/{id}")]
        public async Task<IActionResult> Edit(string id)
        {
            if (checksessions(HttpContext.Request.Path) == false)
            {
                return RedirectToAction("login");
            }
            ViewData["name"] = HttpContext.Session.GetString("Name");
            Location loc = new Location();
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(clientHandler))
            {
                client.BaseAddress = new Uri(BASE_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getLoc = await client.GetAsync("api/Locations/" + id);
                if (getLoc.IsSuccessStatusCode)
                {
                    var response = getLoc.Content.ReadAsStringAsync().Result;
                    loc = JsonConvert.DeserializeObject<Location>(response);

                }
            }

            if (loc.Dtype.Contains("Garden Site"))
            {
                List<string> supervisors = new List<string>();
                HttpClientHandler clientHandler1 = new HttpClientHandler();
                using (var client1 = new HttpClient(clientHandler1))
                {
                    client1.BaseAddress = new Uri(BASE_URL);
                    client1.DefaultRequestHeaders.Accept.Clear();
                    client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage getSupervisors = await client1.GetAsync("api/Staffs/Supervisors");
                    if (getSupervisors.IsSuccessStatusCode)
                    {
                        var response = getSupervisors.Content.ReadAsStringAsync().Result;
                        var supervisorsInDB = JsonConvert.DeserializeObject<List<Supervisor>>(response);
                        foreach (var supervisor in supervisorsInDB) { supervisors.Add(supervisor.StaffId); }
                    }
                }
                ViewData["Supervisors"] = supervisors;
            }

            return View(loc);
        }

        //[Route("~/Location/Delete/{id}")]
        //public async Task<IActionResult> Delete(string id)
        //{
        //    HttpClientHandler clientHandler = new HttpClientHandler();
        //    clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
        //    using (var client = new HttpClient(clientHandler))
        //    {
        //        client.BaseAddress = new Uri(BASE_URL);
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //        //HTTP DELETE
        //        var deleteTask = client.DeleteAsync("api/Locations/" + id);
        //        deleteTask.Wait();

        //        var result = deleteTask.Result;
        //        if (result.IsSuccessStatusCode)
        //        {
        //            return RedirectToAction("Index", "Home");
        //        }
        //    }
        //    return View();
        //}

        public async Task<ActionResult> Delete(string id)
        {
            Location temp = new Location();
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(clientHandler))
            {
                client.BaseAddress = new Uri(BASE_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getdata = await client.GetAsync("api/Locations/" + id);

                if (getdata.IsSuccessStatusCode)
                {


                    var response2 = getdata.Content.ReadAsStringAsync().Result;
                    temp = JsonConvert.DeserializeObject<Location>(response2);


                }


            }



            HttpClientHandler clientHandler2 = new HttpClientHandler();
            clientHandler2.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(clientHandler2))
            {
                client.BaseAddress = new Uri(BASE_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                temp.Active = false;

                var result = client.PutAsJsonAsync("api/Locations/" + temp.LocationId + "/", temp).Result;

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> AssignBin(string ID)
        {
            if (checksessions(HttpContext.Request.Path) == false)
            {
                return RedirectToAction("login");
            }
            ViewData["name"] = HttpContext.Session.GetString("Name");
            List<string> bins = new List<string>();
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(clientHandler))
            {
                client.BaseAddress = new Uri(BASE_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getBins = await client.GetAsync("api/Procedures/UnassignedBins");
                if (getBins.IsSuccessStatusCode)
                {
                    var response = getBins.Content.ReadAsStringAsync().Result;
                    var binsInDB = JsonConvert.DeserializeObject<List<Bin>>(response);
                    foreach (var bin in binsInDB) { bins.Add(bin.BinId); }

                }
                ViewData["Bins"] = bins;
                ViewData["ID"] = ID;
            }



            return View();
        }


        [HttpPost]

        public async Task<ActionResult> AssignBin(IFormCollection Form)
        {




            var binslot = new BinSlot
           (
                0,
              Form["Bin"],
              Form["GardenSite"]


           );

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(clientHandler))
            {
                client.BaseAddress = new Uri(BASE_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.PostAsJsonAsync("api/BinSlots/", binslot).Result;



                if (response.IsSuccessStatusCode)
                {


                    List<string> Bins = new List<string>();
                    HttpClientHandler clientHandler3 = new HttpClientHandler();
                    clientHandler3.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                    using (var client3 = new HttpClient(clientHandler3))
                    {
                        client3.BaseAddress = new Uri(BASE_URL);
                        client3.DefaultRequestHeaders.Accept.Clear();
                        client3.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        HttpResponseMessage getBins = await client3.GetAsync("api/Bins");
                        if (getBins.IsSuccessStatusCode)
                        {
                            var response2 = getBins.Content.ReadAsStringAsync().Result;
                            var binsInDB = JsonConvert.DeserializeObject<List<Bin>>(response2);
                            foreach (var bin in binsInDB) { Bins.Add(bin.BinId); }
                        }
                    }
                    ViewData["Bins"] = Bins;
                }

            }
            return View();
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




