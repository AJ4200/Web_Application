using EpsilonDemoWebsite.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace EpsilonDemoWebsite.Controllers
{
    public class TruckController : Controller
    {
        // GET: TruckController

        string baseURL = "http://localhost:7157/api";
        public async Task<ActionResult> Index( string search)
        {

            if (checksessions(HttpContext.Request.Path) == false)
            {
                return RedirectToAction("login");
            }

            ViewData["name"] = HttpContext.Session.GetString("Name");
            var trucklist = new List<Truck>();
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(clientHandler))
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var getdata = await client.GetAsync("api/Trucks");

                if (getdata.IsSuccessStatusCode)
                {
                    string apiResponse = await getdata.Content.ReadAsStringAsync();
                    trucklist = JsonConvert.DeserializeObject<List<Truck>>(apiResponse);
                    //if (option == "NumberPlate")
                    //{
                    //    //Index action method will return a view with a student records based on what a user specify the value in textbox  
                       if (search != null) { 
                    List<Truck> list = trucklist.Where(x => x.NumberPlate.ToLower().Contains(search.ToLower()) || search == null).ToList();
                    List<Truck> list2 = trucklist.Where(x => x.Driver.ToLower().Contains(search.ToLower()) || search == null).ToList();
                    list.AddRange(list2);
                    return View(list);
                    }
                    //else if (option == "Driver")
                    //{
                    //    if (search != null)
                    //        return View(trucklist.Where(x => x.Driver.ToLower().Contains(search.ToLower()) || search == null).ToList());
                    //}
                    //else if (option != null)
                    //{
                    //    if (search != null)
                    //        return View(trucklist.Where(x => x.Driver.ToLower().Contains(search.ToLower()) || search == null).ToList());
                    //}
                }

            }



            return View(trucklist);
        }

        // GET: TruckController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public async Task<ActionResult> addtruck()
        {
            if (checksessions(HttpContext.Request.Path) == false)
            {
                return RedirectToAction("login");
            }
            ViewData["name"] = HttpContext.Session.GetString("Name");
            List<string> drivers = new List<string>();
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(clientHandler))
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getDrivers = await client.GetAsync("api/Staffs/Drivers");
                if (getDrivers.IsSuccessStatusCode)
                {
                    var response = getDrivers.Content.ReadAsStringAsync().Result;
                    var driversInDB = JsonConvert.DeserializeObject<List<Driver>>(response);
                    foreach (var driver in driversInDB) { drivers.Add(driver.StaffId); }
                }
            }
            ViewData["Drivers"] = drivers;

            List<string> Bins = new List<string>();
            HttpClientHandler clientHandler3 = new HttpClientHandler();
            clientHandler3.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client3 = new HttpClient(clientHandler3))
            {
                client3.BaseAddress = new Uri(baseURL);
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

            return View();
        }
        [HttpPost]
        // GET: TruckController/Create
        public async Task<ActionResult<Truck>> addtruck(IFormCollection Form)
        {
            Random r = new Random();
            string license = null;
            int rand = r.Next(70, 99);
            string truckid = "T" + rand;

            var truck = new Truck
           (
              truckid,
              Form["numberplate"],
              Form["bins"],
              Form["drivers"],
              true
           );

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.PostAsJsonAsync("api/Trucks/", truck).Result;



                if (response.IsSuccessStatusCode)
                {
                    List<string> drivers = new List<string>();
                    HttpClientHandler clientHandler2 = new HttpClientHandler();
                    clientHandler2.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                    using (var client2 = new HttpClient(clientHandler2))
                    {
                        client2.BaseAddress = new Uri(baseURL);
                        client2.DefaultRequestHeaders.Accept.Clear();
                        client2.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        HttpResponseMessage getDrivers = await client2.GetAsync("api/Staffs/Drivers");
                        if (getDrivers.IsSuccessStatusCode)
                        {
                            var response2 = getDrivers.Content.ReadAsStringAsync().Result;
                            var driversInDB = JsonConvert.DeserializeObject<List<Driver>>(response2);
                            foreach (var driver in driversInDB) { drivers.Add(driver.StaffId); }
                        }
                    }
                    ViewData["Drivers"] = drivers;

                    List<string> Bins = new List<string>();
                    HttpClientHandler clientHandler3 = new HttpClientHandler();
                    clientHandler3.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                    using (var client3 = new HttpClient(clientHandler3))
                    {
                        client3.BaseAddress = new Uri(baseURL);
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
            return RedirectToAction("Index");




        }



        // GET: TruckController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (checksessions(HttpContext.Request.Path) == false)
            {
                return RedirectToAction("login");
            }
            ViewData["name"] = HttpContext.Session.GetString("Name");
            Truck truck = new Truck();
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(clientHandler))
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getTrucks = await client.GetAsync("api/Trucks/" + id);
                if (getTrucks.IsSuccessStatusCode)
                {
                    var response = getTrucks.Content.ReadAsStringAsync().Result;
                    truck = JsonConvert.DeserializeObject<Truck>(response);
                }

            }
            List<string> Drivers = new List<string>();
            HttpClientHandler clientHandler1 = new HttpClientHandler();
            clientHandler1.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client1 = new HttpClient(clientHandler1))
            {
                client1.BaseAddress = new Uri(baseURL);
                client1.DefaultRequestHeaders.Accept.Clear();
                client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getDrivers = await client1.GetAsync("api/Staffs/Drivers");
                if (getDrivers.IsSuccessStatusCode)
                {
                    var response2 = getDrivers.Content.ReadAsStringAsync().Result;
                    var DriversInDB = JsonConvert.DeserializeObject<List<Driver>>(response2);
                    foreach (var driver in DriversInDB) { Drivers.Add(driver.StaffId); }
                }
            }
            ViewData["Drivers"] = Drivers;
            List<string> Bins = new List<string>();
            HttpClientHandler clientHandler2 = new HttpClientHandler();
            clientHandler2.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client2 = new HttpClient(clientHandler2))
            {
                client2.BaseAddress = new Uri(baseURL);
                client2.DefaultRequestHeaders.Accept.Clear();
                client2.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getBins = await client2.GetAsync("api/Bins");
                if (getBins.IsSuccessStatusCode)
                {
                    var response2 = getBins.Content.ReadAsStringAsync().Result;
                    var BinsInDB = JsonConvert.DeserializeObject<List<Bin>>(response2);
                    foreach (var bin in BinsInDB) { Bins.Add(bin.BinId); }
                }
            }
            ViewData["Bins"] = Bins;
            return View(truck);
        }

        // POST: TruckController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(IFormCollection Form)
        {





            var TruckToEdit = new Truck
           (
                 Form["TruckId"],
              Form["NumberPlate"],
              Form["Bins"],
              Form["Drivers"],
              true
           );
            List<string> Drivers = new List<string>();
            HttpClientHandler clientHandler1 = new HttpClientHandler();
            clientHandler1.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client1 = new HttpClient(clientHandler1))
            {
                client1.BaseAddress = new Uri(baseURL);
                client1.DefaultRequestHeaders.Accept.Clear();
                client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getDrivers = await client1.GetAsync("api/Staffs/Drivers");
                if (getDrivers.IsSuccessStatusCode)
                {
                    var response2 = getDrivers.Content.ReadAsStringAsync().Result;
                    var DriversInDB = JsonConvert.DeserializeObject<List<Driver>>(response2);
                    foreach (var driver in DriversInDB) { Drivers.Add(driver.StaffId); }
                }
            }
            ViewData["Drivers"] = Drivers;
            List<string> Bins = new List<string>();
            HttpClientHandler clientHandler2 = new HttpClientHandler();
            clientHandler2.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client2 = new HttpClient(clientHandler2))
            {
                client2.BaseAddress = new Uri(baseURL);
                client2.DefaultRequestHeaders.Accept.Clear();
                client2.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getBins = await client2.GetAsync("api/Bins");
                if (getBins.IsSuccessStatusCode)
                {
                    var response2 = getBins.Content.ReadAsStringAsync().Result;
                    var BinsInDB = JsonConvert.DeserializeObject<List<Bin>>(response2);
                    foreach (var bin in BinsInDB) { Bins.Add(bin.BinId); }
                }
            }
            ViewData["Bins"] = Bins;

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(clientHandler))
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var result = client.PutAsJsonAsync("api/Trucks/" + TruckToEdit.TruckId + "/", TruckToEdit).Result;

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

            }
            return RedirectToAction("Index");

        }

        // GET: TruckController/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            Truck temptruck = new Truck();
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(clientHandler))
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getdata = await client.GetAsync("api/Trucks/" + id);

                if (getdata.IsSuccessStatusCode)
                {


                    var response2 = getdata.Content.ReadAsStringAsync().Result;
                    temptruck = JsonConvert.DeserializeObject<Truck>(response2);


                }


            }
           


            HttpClientHandler clientHandler2 = new HttpClientHandler();
            clientHandler2.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(clientHandler2))
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                temptruck.Active = false;

                var result = client.PutAsJsonAsync("api/Trucks/" + temptruck.TruckId + "/", temptruck).Result;

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");

            



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
