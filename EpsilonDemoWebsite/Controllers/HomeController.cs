using EpsilonDemoWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;
using Newtonsoft.Json;
using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EpsilonDemoWebsite.Models.Procedures;

namespace EpsilonDemoWebsite.Controllers
{

    public class HomeController : Controller
    {
        public const string SessionKeyID = "_ID";
        private readonly ILogger<HomeController> _logger;
        public string email = "";
        string password = "";
        public string hashpassword = "";
        string baseURL = "http://localhost:7157/api";
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            if (checksessions(HttpContext.Request.Path) == false)
            {
                return RedirectToAction("login");
            }
            ViewData["name"] = HttpContext.Session.GetString("Name");
            ViewData["email"] = HttpContext.Session.GetString("Email");


            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(clientHandler))
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //GET TRUCK QUEUE
                List<spTruckQueueResult> queue = new List<spTruckQueueResult>();
                HttpResponseMessage request = await client.GetAsync("api/Procedures/TruckQueue");
                if (request.IsSuccessStatusCode)
                {
                    var response = request.Content.ReadAsStringAsync().Result;
                    var queueInDB = JsonConvert.DeserializeObject<List<spTruckQueueResult>>(response);
                    foreach (var q in queueInDB) { queue.Add(q); }

                }
                ViewBag.Queue = queue;

                //GET PENDING REQUESTS
                List<spPendingRequestsResult> pendingRequests = new List<spPendingRequestsResult>();
                request = await client.GetAsync("api/Procedures/PendingRequests");
                if (request.IsSuccessStatusCode)
                {
                    var response = request.Content.ReadAsStringAsync().Result;
                    var pending = JsonConvert.DeserializeObject<List<spPendingRequestsResult>>(response);
                    foreach (var p in pending) { pendingRequests.Add(p); }

                }
                ViewBag.PendingRequests = pendingRequests;

                //GET CURRENT MONTH'S COLLECTION LOG
                List<spCurrentMonthCollectionLogResult> collectionLog = new List<spCurrentMonthCollectionLogResult>();
                request = await client.GetAsync("api/Procedures/CurrentMonthCollectionLog");
                if (request.IsSuccessStatusCode)
                {
                    var response = request.Content.ReadAsStringAsync().Result;
                    var currentMonth = JsonConvert.DeserializeObject<List<spCurrentMonthCollectionLogResult>>(response);
                    foreach (var c in currentMonth) { collectionLog.Add(c); }

                }
                ViewBag.CollectionLog = collectionLog;
            }

            // call web api
            var staff = new Session();
            staff.Name = HttpContext.Session.GetString("Name");
            staff.Email = HttpContext.Session.GetString("Email");

            return View(staff);
        }

        public IActionResult login()
        {
            // call web api
            return View();
        }
        private static string GetHash(string text)
        {
            // SHA512 is disposable by inheritance.  
            using (var sha256 = SHA256.Create())
            {
                // Send a sample text to hash.  
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
                // Get the hashed string.  
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        //checks the info from the login page
        public async Task<IActionResult> checklogin()
        {
            // call web api
            email = Request.Form["email"];
            password = Request.Form["password"];

            // hash password
            hashpassword = GetHash(password);

            //api call
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(clientHandler))
            {
                client.BaseAddress = new Uri(baseURL + "/Staffs/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getdata = await client.GetAsync("Login/" + email + "/" + hashpassword);

                Staff staff = null;
                if (getdata.IsSuccessStatusCode)
                {
                    staff = await getdata.Content.ReadFromJsonAsync<Staff>();
                    // set session variables
                    HttpContext.Session.SetString("Email", JsonConvert.SerializeObject(staff.Email));
                    HttpContext.Session.SetString("Name", staff.Name);
                    return RedirectToAction("Index");
                }
            }
            return View("login");
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

        public IActionResult logout()
        {
            HttpContext.Session.SetString("Name", "");
            HttpContext.Session.SetString("Email", "");
            return RedirectToAction("login");
        }
    }
}