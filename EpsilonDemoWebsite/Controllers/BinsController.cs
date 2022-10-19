using EpsilonDemoWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using Net.Codecrete.QrCodeGenerator;
using System.Text;
using System.Net.Http.Headers;

namespace EpsilonDemoWebsite.Controllers
{
    public class BinsController : Controller
    {
        Bin bin = new Bin();
        List<Bin> binlist = new List<Bin>();
        string baseURL = "http://localhost:7157/api";
        public async Task<ActionResult> Index(string search)
        {
            if (checksessions(HttpContext.Request.Path) == false)
            {
                return RedirectToAction("login");
            }
            ViewData["name"] = HttpContext.Session.GetString("Name");


            DataTable dt = new DataTable();
            List<Bin> binlist = new List<Bin>();
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(clientHandler))
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var getdata = await client.GetAsync("api/Bins");

                if (getdata.IsSuccessStatusCode)
                {
                    string apiResponse = await getdata.Content.ReadAsStringAsync();
                    binlist = JsonConvert.DeserializeObject<List<Bin>>(apiResponse);

                    //Index action method will return a view with a student records based on what a user specify the value in textbox  
                    if (search != null)
                        return View(binlist.Where(x => x.BinId.ToLower().Contains(search.ToLower()) || search == null).ToList());


                }

            }
            return View(binlist);
        }


        public ActionResult addbin()
        {
            if (checksessions(HttpContext.Request.Path) == false)
            {
                return RedirectToAction("login");
            }

            Random r = new Random();
            string license = null;
            int rand = r.Next(10000, 99999);
            string binid = "BIN" + rand;
            var qr = QrCode.EncodeText(binid, QrCode.Ecc.Medium);
            string svg = qr.ToSvgString(4);
            byte[] qrbyte = Encoding.ASCII.GetBytes(svg);

            var bin = new Bin
           (
              binid,
              0,
              qrbyte,
              true
           );

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(clientHandler))
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.PostAsJsonAsync("api/bins/", bin).Result;



                if (response.IsSuccessStatusCode)
                {
                    string results = response.Content.ReadAsStringAsync().Result;
                    // user = JsonConvert.DeserializeObject<Staff>(results);
                }
            }

            ViewData["name"] = HttpContext.Session.GetString("Name");
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Details(string id)
        {
            bin = new Bin();
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(clientHandler))
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var getdata = await client.GetAsync("api/Bins/" + id);

                if (getdata.IsSuccessStatusCode)
                {
                    string apiResponse = await getdata.Content.ReadAsStringAsync();
                    bin = JsonConvert.DeserializeObject<Bin>(apiResponse);
                }

            }
            return View(bin);
        }


        [HttpPost]
        public async Task<ActionResult> addbin(IFormCollection Form)
        {
            var qr = QrCode.EncodeText(Form["binid"], QrCode.Ecc.Medium);
            string svg = qr.ToSvgString(4);
            byte[] qrbyte = Encoding.ASCII.GetBytes(svg);

            var bin = new Bin
           (
               Form["binid"],
              0,
              qrbyte,
              Convert.ToBoolean(Form["Active"])
           );

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(clientHandler))
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.PostAsJsonAsync("api/bins/", bin).Result;



                if (response.IsSuccessStatusCode)
                {
                    string results = response.Content.ReadAsStringAsync().Result;
                    // user = JsonConvert.DeserializeObject<Staff>(results);
                }
            }


            return RedirectToAction("index");
        }

        [HttpPost]
        public IActionResult Edit(IFormCollection Form)
        {
            string binId = Form["binId"];
            string qrcode = Form["qrcode"];
            string StrWaste = Form["waste"];
            int waste = int.Parse(StrWaste);
            var qr = QrCode.EncodeText(Form["binid"], QrCode.Ecc.Medium);
            string svg = qr.ToSvgString(4);
            byte[] qrbyte = Encoding.ASCII.GetBytes(svg);

            var bin = new Bin
           (
               binId,
              int.Parse(StrWaste),
              qrbyte,
              Convert.ToBoolean(Form["Active"])
           );
            var BinToEdit = new Bin(binId, waste, qrbyte, Convert.ToBoolean(Form["Active"]));

            //api call
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(clientHandler))
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var result = client.PutAsJsonAsync("api/Bins/" + BinToEdit.BinId + "/", BinToEdit).Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Bins");
                }
            }
            return RedirectToAction("Index", "Bins");
        }
        //[Route("~/Location/Edit/{loc}/{id}")]
        public async Task<IActionResult> Edit(string id)
        {
            if (checksessions(HttpContext.Request.Path) == false)
            {
                return RedirectToAction("login");
            }
            ViewData["name"] = HttpContext.Session.GetString("Name");
            Bin bin = new Bin();
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(clientHandler))
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getBin = await client.GetAsync("api/Bins/" + id);
                if (getBin.IsSuccessStatusCode)
                {
                    string apiResponse = await getBin.Content.ReadAsStringAsync();
                    bin = JsonConvert.DeserializeObject<Bin>(apiResponse);


                }
            }
            return View(bin);
        }
        //public async Task<ActionResult> Delete(string BinID)
        //{
        //    Staff removeStaff = new Staff();
        //    HttpClientHandler clientHandler = new HttpClientHandler();
        //    clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
        //    using (var client = new HttpClient(clientHandler))
        //    {
        //        client.BaseAddress = new Uri(baseURL);
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

        //        var getdata = await client.DeleteAsync("api/Bins/" + BinID);

        //        if (getdata.IsSuccessStatusCode)
        //        {
        //            return RedirectToAction("Index", "Home");
        //        }

        //    }
        //    return RedirectToAction("Index", "Home");


        //}

        public async Task<ActionResult> Delete(string id)
        {
            Bin temp = new Bin();
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(clientHandler))
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getdata = await client.GetAsync("api/Bins/" + id);

                if (getdata.IsSuccessStatusCode)
                {


                    var response2 = getdata.Content.ReadAsStringAsync().Result;
                    temp = JsonConvert.DeserializeObject<Bin>(response2);


                }


            }



            HttpClientHandler clientHandler2 = new HttpClientHandler();
            clientHandler2.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(clientHandler2))
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                temp.Active = false;

                var result = client.PutAsJsonAsync("api/Bins/" + temp.BinId + "/", temp).Result;

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
