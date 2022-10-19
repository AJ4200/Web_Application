using Microsoft.AspNetCore.Mvc;
using EpsilonDemoWebsite.Models;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Globalization;
using EpsilonDemoWebsite.Models.Procedures;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EpsilonDemoWebsite.Controllers
{
    public class ReportsController : Controller
    {
        string BASE_URL = "http://localhost:7157/api";

        public IActionResult Index()
        {


            ViewData["name"] = HttpContext.Session.GetString("Name");

            return View();
        }


        // not needed
        public async Task<IActionResult> Collection()
        {
            if (checksessions(HttpContext.Request.Path) == false)
            {
                return RedirectToAction("login");
            }
            ViewData["name"] = HttpContext.Session.GetString("Name");
            List<string> month = new List<string>();
            List<int?> amount = new List<int?>();
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(clientHandler))
            {
                client.BaseAddress = new Uri(BASE_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getCollections = await client.GetAsync("api/Procedures/MonthlyCollections/");
                if (getCollections.IsSuccessStatusCode)
                {
                    var response = getCollections.Content.ReadAsStringAsync().Result;
                    var collections = JsonConvert.DeserializeObject<List<spMonthlyCollectionsResult>>(response);
                    foreach (var col in collections) { month.Add(col.Month); amount.Add(col.Amount); }
                }
            }
            ViewBag.Month = JsonConvert.SerializeObject(month.ToArray());
            ViewBag.Amount = JsonConvert.SerializeObject(amount.ToArray());
            return View();
        }
        public IActionResult MockTables()
        {
            return View();
        }
        //waste recycled


        public async Task<IActionResult> WasteRecycled()
        {
            if (checksessions(HttpContext.Request.Path) == false)
            {
                return RedirectToAction("login");
            }

            ViewData["name"] = HttpContext.Session.GetString("Name");
            bool boolRecycled = true;
            int? numMax;
            int? numMin;
            decimal? numlineMax;
            decimal? numlineMin;

            string[] Months = { "November", "December", "January", "February", "March",
                "April", "May", "June", "July", "August", "September", "October" };
            List<decimal?> Tonnages = new List<decimal?>(12) { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            List<decimal?> Tonnage1 = new List<decimal?>(12) { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            List<decimal?> Tonnage2 = new List<decimal?>(12) { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            List<decimal?> Tonnage3 = new List<decimal?>(12) { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            List<decimal?> Tonnage4 = new List<decimal?>(12) { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            List<decimal?> Tonnage5 = new List<decimal?>(12) { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            List<decimal?> Tonnage6 = new List<decimal?>(12) { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            Dictionary<string, int?> waste_Numloads = new Dictionary<string, int?>();
            Dictionary<string, decimal?> Month_Tonnage = new Dictionary<string, decimal?>();
            List<spWasteDumpedResult> wastes;
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(clientHandler))
            {
                client.BaseAddress = new Uri(BASE_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getLFs = await client.GetAsync("api/Procedures/WasteDroppedOff/" + boolRecycled);
                if (getLFs.IsSuccessStatusCode)
                {
                    var response = getLFs.Content.ReadAsStringAsync().Result;
                    wastes = JsonConvert.DeserializeObject<List<spWasteDumpedResult>>(response);
                    List<int?> totalLoads = new List<int?>(6) { 0, 0, 0, 0, 0, 0 };

                    for (int w = 0; w < wastes.Count; w++)
                    {
                        switch (wastes.ElementAt(w).WasteType)
                        {
                            case "Plastic":
                                totalLoads[0] += wastes.ElementAt(w).NumberOfLoads;
                                for (int a = 1; a < 12; a++)
                                {
                                    if (wastes.ElementAt(w).Month.Equals(Months[a]))
                                    {

                                        Tonnage1[a] += wastes.ElementAt(w).Tonnage;
                                    }
                                }
                                break;
                            case "Glass":
                                totalLoads[1] += wastes.ElementAt(w).NumberOfLoads;
                                for (int a = 1; a < 12; a++)
                                {
                                    if (wastes.ElementAt(w).Month.Equals(Months[a]))
                                    {

                                        Tonnage2[a] += wastes.ElementAt(w).Tonnage;
                                    }
                                }
                                break;
                            case "Industrial Waste":
                                totalLoads[2] += wastes.ElementAt(w).NumberOfLoads;
                                for (int a = 1; a < 12; a++)
                                {
                                    if (wastes.ElementAt(w).Month.Equals(Months[a]))
                                    {

                                        Tonnage3[a] += wastes.ElementAt(w).Tonnage;
                                    }
                                }
                                break;
                            case "General Waste":
                                totalLoads[3] += wastes.ElementAt(w).NumberOfLoads;
                                for (int a = 1; a < 12; a++)
                                {
                                    if (wastes.ElementAt(w).Month.Equals(Months[a]))
                                    {

                                        //Tonnage4[a] += wastes.ElementAt(w).Tonnage
                                        Tonnage4[a] = 0;
                                    }
                                }
                                break;
                            case "Cardboard and Paper":
                                totalLoads[4] += wastes.ElementAt(w).NumberOfLoads;
                                for (int a = 1; a < 12; a++)
                                {
                                    if (wastes.ElementAt(w).Month.Equals(Months[a]))
                                    {

                                        Tonnage5[a] += wastes.ElementAt(w).Tonnage;
                                    }
                                }
                                break;
                            case "Garden Waste":
                                totalLoads[5] += wastes.ElementAt(w).NumberOfLoads;
                                for (int a = 1; a < 12; a++)
                                {
                                    if (wastes.ElementAt(w).Month.Equals(Months[a]))
                                    {

                                        Tonnage6[a] += wastes.ElementAt(w).Tonnage;
                                    }
                                }
                                break;
                            default:
                                break;
                        }

                    }



                    waste_Numloads.Add("Plastic", totalLoads[0]);
                    waste_Numloads.Add("Glass", totalLoads[1]);
                    waste_Numloads.Add("Industrial Waste", totalLoads[2]);
                    //waste_Numloads.Add("General Waste", totalLoads[3]);
                    waste_Numloads.Add("Cardboard and Paper", totalLoads[4]);
                    waste_Numloads.Add("Garden Waste", totalLoads[5]);




                }
            }
            ViewBag.WasteType = JsonConvert.SerializeObject(waste_Numloads.Keys);
            ViewBag.NumberOfLoads = JsonConvert.SerializeObject(waste_Numloads.Values);


            ViewBag.Months = JsonConvert.SerializeObject(Months);
            ViewBag.Tonnage1 = JsonConvert.SerializeObject(Tonnage1);
            ViewBag.Tonnage2 = JsonConvert.SerializeObject(Tonnage2);
            ViewBag.Tonnage3 = JsonConvert.SerializeObject(Tonnage3);
            ViewBag.Tonnage4 = JsonConvert.SerializeObject(Tonnage4);
            ViewBag.Tonnage5 = JsonConvert.SerializeObject(Tonnage5);
            ViewBag.Tonnage6 = JsonConvert.SerializeObject(Tonnage6);
            ViewBag.Tonnages = JsonConvert.SerializeObject(Tonnages);
            ViewBag.HP = numMax = waste_Numloads.Values.Max().GetValueOrDefault();
            ViewBag.LP = numMin = waste_Numloads.Values.Min().GetValueOrDefault();
            foreach (var w in waste_Numloads)
            {
                if (w.Value.Equals(numMax))
                {
                    ViewBag.HPWasteType = w.Key;
                }
                if (w.Value.Equals(numMin))
                {
                    ViewBag.LPWasteType = w.Key;
                }
            }
            ViewBag.HM = numlineMax = Month_Tonnage.Values.Max().GetValueOrDefault();
            ViewBag.LM = numlineMin = Month_Tonnage.Values.Min().GetValueOrDefault();
            foreach (var m in Month_Tonnage)
            {
                if (m.Value.Equals(numlineMax))
                {
                    ViewBag.HMonthName = m.Key;
                }

                if (m.Value.Equals(numlineMin))
                {
                    ViewBag.LMonthName = m.Key;
                }
            }


            return View();
        }

        //waste collected
        public async Task<IActionResult> WasteCollected(string months)
        {
            if (checksessions(HttpContext.Request.Path) == false)
            {
                return RedirectToAction("login");
            }

            ViewData["name"] = HttpContext.Session.GetString("Name");
            bool boolRecycled = false;
            int? numMax;
            int? numMin;


            string[] Months = { "November", "December", "January", "February", "March",
                "April", "May", "June", "July", "August", "September", "October" };

            List<decimal?> Tonnage1 = new List<decimal?>(12) { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            List<decimal?> Tonnage2 = new List<decimal?>(12) { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            List<decimal?> Tonnage3 = new List<decimal?>(12) { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            List<decimal?> Tonnage4 = new List<decimal?>(12) { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            List<decimal?> Tonnage5 = new List<decimal?>(12) { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            List<decimal?> Tonnage6 = new List<decimal?>(12) { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            Dictionary<string, int?> waste_Numloads = new Dictionary<string, int?>();

            List<spWasteDumpedResult> wastes;
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(clientHandler))
            {
                client.BaseAddress = new Uri(BASE_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                boolRecycled = false;
                HttpResponseMessage getLFs = await client.GetAsync("api/Procedures/WasteDroppedOff/" + boolRecycled);
                if (getLFs.IsSuccessStatusCode)
                {
                    var response = getLFs.Content.ReadAsStringAsync().Result;
                    wastes = JsonConvert.DeserializeObject<List<spWasteDumpedResult>>(response);
                    List<int?> totalLoads = new List<int?>(6) { 0, 0, 0, 0, 0, 0 };


                    for (int w = 0; w < wastes.Count; w++)
                    {
                        decimal? num = 0;
                        switch (wastes.ElementAt(w).WasteType)
                        {
                            case "Plastic":
                                totalLoads[0] += wastes.ElementAt(w).NumberOfLoads;

                                for (int a = 1; a < 12; a++)
                                {
                                    if (wastes.ElementAt(w).Month.Equals(Months[a]))
                                    {
                                        num += wastes.ElementAt(a).Tonnage;
                                        Tonnage1[a] = num;
                                    }
                                }

                                break;
                            case "Glass":
                                totalLoads[1] += wastes.ElementAt(w).NumberOfLoads;
                                for (int a = 1; a < 12; a++)
                                {
                                    if (wastes.ElementAt(w).Month.Equals(Months[a]))
                                    {

                                        Tonnage2[a] += wastes.ElementAt(w).Tonnage;
                                    }
                                }

                                break;
                            case "Industrial Waste":
                                totalLoads[2] += wastes.ElementAt(w).NumberOfLoads;
                                for (int a = 1; a < 12; a++)
                                {
                                    if (wastes.ElementAt(w).Month.Equals(Months[a]))
                                    {

                                        Tonnage3[a] += wastes.ElementAt(w).Tonnage;
                                    }
                                }
                                break;
                            case "General Waste":
                                totalLoads[3] += wastes.ElementAt(w).NumberOfLoads;
                                for (int a = 1; a < 12; a++)
                                {
                                    if (wastes.ElementAt(w).Month.Equals(Months[a]))
                                    {

                                        Tonnage4[a] += wastes.ElementAt(w).Tonnage;
                                    }
                                }
                                break;
                            case "Cardboard and Paper":
                                totalLoads[4] += wastes.ElementAt(w).NumberOfLoads;

                                for (int a = 1; a < 12; a++)
                                {
                                    if (wastes.ElementAt(w).Month.Equals(Months[a]))
                                    {

                                        Tonnage5[a] += wastes.ElementAt(w).Tonnage;
                                    }
                                }
                                break;
                            case "Garden Waste":
                                totalLoads[5] += wastes.ElementAt(w).NumberOfLoads;
                                for (int a = 1; a < 12; a++)
                                {
                                    if (wastes.ElementAt(w).Month.Equals(Months[a]))
                                    {

                                        Tonnage6[a] += wastes.ElementAt(w).Tonnage;
                                    }
                                }
                                break;
                            default:
                                break;
                        }



                    }

                    waste_Numloads.Add("Plastic", totalLoads[0]);
                    waste_Numloads.Add("Glass", totalLoads[1]);
                    waste_Numloads.Add("Industrial Waste", totalLoads[2]);
                    waste_Numloads.Add("General Waste", totalLoads[3]);
                    waste_Numloads.Add("Cardboard and Paper", totalLoads[4]);
                    waste_Numloads.Add("Garden Waste", totalLoads[5]);




                }
            }
            ViewBag.WasteType = JsonConvert.SerializeObject(waste_Numloads.Keys);
            ViewBag.NumberOfLoads = JsonConvert.SerializeObject(waste_Numloads.Values);

            ViewBag.Months = JsonConvert.SerializeObject(Months);
            ViewBag.Tonnage1 = JsonConvert.SerializeObject(Tonnage1);
            ViewBag.Tonnage2 = JsonConvert.SerializeObject(Tonnage2);
            ViewBag.Tonnage3 = JsonConvert.SerializeObject(Tonnage3);
            ViewBag.Tonnage4 = JsonConvert.SerializeObject(Tonnage4);
            ViewBag.Tonnage5 = JsonConvert.SerializeObject(Tonnage5);
            ViewBag.Tonnage6 = JsonConvert.SerializeObject(Tonnage6);
            ViewBag.HP = numMax = waste_Numloads.Values.Max().GetValueOrDefault();
            ViewBag.LP = numMin = waste_Numloads.Values.Min().GetValueOrDefault();
            foreach (var w in waste_Numloads)
            {
                if (w.Value.Equals(numMax))
                {
                    ViewBag.HPWasteType = w.Key;
                }
                if (w.Value.Equals(numMin))
                {
                    ViewBag.LPWasteType = w.Key;
                }
            }


            return View();
        }
        public async Task<IActionResult> TruckBreakdown(string id)
        {
            if (checksessions(HttpContext.Request.Path) == false)
            {
                return RedirectToAction("login");
            }

            ViewData["name"] = HttpContext.Session.GetString("Name");
            if (id == null)
            {
                id = "6";
            }
            int numMonths = Int32.Parse(id);
            string start = DateTime.Now.AddMonths(-numMonths + 1).ToString("yyyy-MM-dd");
            string end = DateTime.Now.ToString("yyyy-MM-dd");

            List<string> months = new List<string>();
            List<int> breakdowns = new List<int>();
            List<int> collections = new List<int>();

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(clientHandler))
            {
                //breakdowns
                client.BaseAddress = new Uri(BASE_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage getInfo = await client.GetAsync("api/Procedures/TruckBreakdowns/" + start + "/" + end);
                if (getInfo.IsSuccessStatusCode)
                {
                    var response = getInfo.Content.ReadAsStringAsync().Result;
                    var breakdownsFromDB = JsonConvert.DeserializeObject<List<spTruckBreakdowns>>(response);
                    foreach (var b in breakdownsFromDB) { months.Add(b.Month); breakdowns.Add(b.Breakdowns); }
                }

                //collections
                getInfo = await client.GetAsync("api/Procedures/MonthlyCollections/" + start + "/" + end);
                if (getInfo.IsSuccessStatusCode)
                {
                    var response = getInfo.Content.ReadAsStringAsync().Result;
                    var collectionsFromDB = JsonConvert.DeserializeObject<List<spMonthlyCollectionsResult>>(response);
                    foreach (var c in collectionsFromDB) { collections.Add(c.Amount); }
                }
            }

            ViewBag.Collections = JsonConvert.SerializeObject(collections.GetRange(12 - numMonths, numMonths).ToArray());
            ViewBag.Breakdowns = JsonConvert.SerializeObject(breakdowns.GetRange(12 - numMonths, numMonths).ToArray());
            ViewBag.Months = JsonConvert.SerializeObject(months.GetRange(12 - numMonths, numMonths).ToArray());
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> TruckBreakdown(IFormCollection Form)
        {
            if (checksessions(HttpContext.Request.Path) == false)
            {
                return RedirectToAction("login");
            }
            ViewData["name"] = HttpContext.Session.GetString("Name");


            DateTime? startDT = Convert.ToDateTime(Form["startdate"]);
            DateTime? endDT = Convert.ToDateTime(Form["enddate"]);
            string start = startDT.Value.ToString("yyyy-MM-dd");
            string end = endDT.Value.ToString("yyyy -MM-dd");

            List<string> months = new List<string>();
            List<int> breakdowns = new List<int>();
            List<int> collections = new List<int>();

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(clientHandler))
            {
                //breakdowns
                client.BaseAddress = new Uri(BASE_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage getInfo = await client.GetAsync("api/Procedures/TruckBreakdowns/" + start + "/" + end);
                if (getInfo.IsSuccessStatusCode)
                {
                    var response = getInfo.Content.ReadAsStringAsync().Result;
                    var breakdownsFromDB = JsonConvert.DeserializeObject<List<spTruckBreakdowns>>(response);
                    foreach (var b in breakdownsFromDB) { months.Add(b.Month); breakdowns.Add(b.Breakdowns); }
                }

                //collections
                getInfo = await client.GetAsync("api/Procedures/MonthlyCollections/" + start + "/" + end);
                if (getInfo.IsSuccessStatusCode)
                {
                    var response = getInfo.Content.ReadAsStringAsync().Result;
                    var collectionsFromDB = JsonConvert.DeserializeObject<List<spMonthlyCollectionsResult>>(response);
                    foreach (var c in collectionsFromDB) { collections.Add(c.Amount); }
                }
            }

            ViewBag.Collections = JsonConvert.SerializeObject(collections.ToArray());
            ViewBag.Breakdowns = JsonConvert.SerializeObject(breakdowns.ToArray());
            ViewBag.Months = JsonConvert.SerializeObject(months.ToArray());
            return View();
        }

        public async Task<IActionResult> GardenSiteTraffic()
        {
            var StartEndDates = new StartEndDates
         (
             Convert.ToString(DateTime.Now.AddYears(-1)),
             Convert.ToString(DateTime.Now)
         );
            List<string?> aeraslist;
            List<int?> requestsList;
            List<spGardenSiteTrafficResult> GardenSiteTrafficList;

            if (checksessions(HttpContext.Request.Path) == false)
            {
                return RedirectToAction("login");
            }
            ViewData["name"] = HttpContext.Session.GetString("Name");

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(clientHandler))
            {
                client.BaseAddress = new Uri(BASE_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getInfo = await client.GetAsync("api/Procedures/GardenSiteTraffic/" + StartEndDates.startDate + "/" + StartEndDates.endDate);
                if (getInfo.IsSuccessStatusCode)
                {
                    var response = getInfo.Content.ReadAsStringAsync().Result;
                    GardenSiteTrafficList = JsonConvert.DeserializeObject<List<spGardenSiteTrafficResult>>(response);
                    aeraslist = new List<string?>();
                    requestsList = new List<int?>();
                    for (int w = 0; w < GardenSiteTrafficList.Count - 1; w++)
                    {
                        aeraslist.Add(GardenSiteTrafficList.ElementAt(w).Area);
                        requestsList.Add(GardenSiteTrafficList.ElementAt(w).Requests);
                    }



                    ViewBag.aerasList = JsonConvert.SerializeObject(aeraslist);
                    ViewBag.requestsList = JsonConvert.SerializeObject(requestsList);

                }
            }



            return View(StartEndDates);

        }

        [HttpPost]
        public async Task<IActionResult> GardenSiteTraffic(IFormCollection form)
        {
            string? start = form["startDate"];
            string? end = form["endDate"];
            var seDates = new StartEndDates
                (
                start,
                end
                );
            if (checksessions(HttpContext.Request.Path) == false)
            {
                return RedirectToAction("login");
            }

            ViewData["name"] = HttpContext.Session.GetString("Name");

            List<string?> aeraslist;

            List<int?> requestsList;

            List<spGardenSiteTrafficResult> GardenSiteTrafficList;
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(clientHandler))
            {
                client.BaseAddress = new Uri(BASE_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getLFs = await client.GetAsync("api/Procedures/GardenSiteTraffic/" + start + "/" + end);
                if (getLFs.IsSuccessStatusCode)
                {
                    var response = getLFs.Content.ReadAsStringAsync().Result;
                    GardenSiteTrafficList = JsonConvert.DeserializeObject<List<spGardenSiteTrafficResult>>(response);
                    aeraslist = new List<string?>();
                    requestsList = new List<int?>();
                    for (int w = 0; w < GardenSiteTrafficList.Count - 1; w++)
                    {
                        aeraslist.Add(GardenSiteTrafficList.ElementAt(w).Area);
                        requestsList.Add(GardenSiteTrafficList.ElementAt(w).Requests);
                    }



                    ViewBag.aerasList = JsonConvert.SerializeObject(aeraslist);
                    ViewBag.requestsList = JsonConvert.SerializeObject(requestsList);
                }
            }
            return View(seDates);
        }


        public async Task<IActionResult> MonthOverview()
        {
            if (checksessions(HttpContext.Request.Path) == false)
            {
                return RedirectToAction("login");
            }

            ViewData["name"] = HttpContext.Session.GetString("Name");






            List<int?> Loads;
            List<string> types = new List<string>(1) { "Landfill", "Recycling" };


            List<decimal?> Tonnages;

            List<spMonthOverview> MonthOverviewItem;
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(clientHandler))
            {
                client.BaseAddress = new Uri(BASE_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getLFs = await client.GetAsync("api/Procedures/CurrentMonthOverview");
                if (getLFs.IsSuccessStatusCode)
                {
                    var response = getLFs.Content.ReadAsStringAsync().Result;
                    MonthOverviewItem = JsonConvert.DeserializeObject<List<spMonthOverview>>(response);
                    Loads = new List<int?>();
                    Tonnages = new List<decimal?>();

                    Loads.Add(MonthOverviewItem.ElementAt(0).landfillLoads);
                    Loads.Add(MonthOverviewItem.ElementAt(0).recycledLoads);
                    Tonnages.Add(MonthOverviewItem.ElementAt(0).landfillTonnage);
                    Tonnages.Add(MonthOverviewItem.ElementAt(0).recycledTonnage);
                    var breakdowns = MonthOverviewItem.ElementAt(0).breakdowns;
                    ViewBag.types = JsonConvert.SerializeObject(types);
                    ViewBag.Loads = JsonConvert.SerializeObject(Loads);
                    ViewBag.Tonnages = JsonConvert.SerializeObject(Tonnages);
                    ViewBag.breakdowns = JsonConvert.SerializeObject(breakdowns);
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