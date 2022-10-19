using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using EpsilonDemoWebsite.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace EpsilonDemoWebsite.Controllers
{

    public class staffController : Controller
    {
        List<Staff> stafflist = null;

        string baseURL = "http://localhost:7157/api";



        //hash function
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
        public async Task<ActionResult> Index(string id, string option, string search)
        {
            if (checksessions(HttpContext.Request.Path) == false)
            {
                return RedirectToAction("login");
            }

            ViewData["name"] = HttpContext.Session.GetString("Name");



            stafflist = new List<Staff>();
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(clientHandler))
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                switch (id)
                {
                    case "SU":
                        HttpResponseMessage getSUs = await client.GetAsync("api/Staffs/Supervisors/");
                        if (getSUs.IsSuccessStatusCode)
                        {
                            var response = getSUs.Content.ReadAsStringAsync().Result;
                            var supervisors = JsonConvert.DeserializeObject<List<Supervisor>>(response);
                            
                            if (search != null)
                            {
                                List<Supervisor> list = supervisors.Where(x => x.Name.ToLower().Contains(search.ToLower()) || search == null).ToList();
                                List<Supervisor> list2 = supervisors.Where(x => x.Surname.ToLower().Contains(search.ToLower()) || search == null).ToList();
                                list.AddRange(list2);
                                foreach (var su in list) { stafflist.Add(new Staff(null, su.StaffId, null, su.Name, su.Surname, null, su.Email, su.Telephone, null, su.Active)); }
                                
                                    return View(stafflist);
                            }
                            else
                            {
                                foreach (var su in supervisors) { stafflist.Add(new Staff(null, su.StaffId, null, su.Name, su.Surname, null, su.Email, su.Telephone, null, su.Active)); }
                            }

                        }
                        break;
                    case "DR":
                        HttpResponseMessage getDRs = await client.GetAsync("api/Staffs/Drivers/");
                        if (getDRs.IsSuccessStatusCode)
                        {
                            var response = getDRs.Content.ReadAsStringAsync().Result;
                            var drivers = JsonConvert.DeserializeObject<List<Driver>>(response);
                           
                            if (search != null)
                            {
                                List<Driver> list = drivers.Where(x => x.Name.ToLower().Contains(search.ToLower()) || search == null).ToList();
                                List<Driver> list2 = drivers.Where(x => x.Surname.ToLower().Contains(search.ToLower()) || search == null).ToList();
                                list.AddRange(list2);
                                foreach (var su in list) { stafflist.Add(new Staff(null, su.StaffId, null, su.Name, su.Surname, null, su.Email, su.Telephone, null, su.Active)); }

                                return View(stafflist);
                            }
                            else
                            {
                                foreach (var dr in drivers) { stafflist.Add(new Staff(null, dr.StaffId, null, dr.Name, dr.Surname, null, dr.Email, dr.Telephone, null, dr.Active)); }
                            }
                        }
                        break;
                    case "AD":
                        HttpResponseMessage getADs = await client.GetAsync("api/Staffs/Admins/");
                        if (getADs.IsSuccessStatusCode)
                        {
                            var response = getADs.Content.ReadAsStringAsync().Result;
                            var admins = JsonConvert.DeserializeObject<List<Admin>>(response);
                            
                            if (search != null)
                            {  
                            List<Admin> list = admins.Where(x => x.Name.ToLower().Contains(search.ToLower()) || search == null).ToList();
                            List<Admin> list2 = admins.Where(x => x.Surname.ToLower().Contains(search.ToLower()) || search == null).ToList();
                            list.AddRange(list2);
                                foreach (var su in list) { stafflist.Add(new Staff(null, su.StaffId, null, su.Name, su.Surname, null, su.Email, su.Telephone, null, su.Active)); }

                                return View(stafflist);
                            }
                            else
                            {
                                foreach (var ad in admins) { stafflist.Add(new Staff(null, ad.StaffId, null, ad.Name, ad.Surname, null, ad.Email, ad.Telephone, null, ad.Active)); }
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            return View(stafflist);
        }
        public ActionResult addstaff()
        {
            if (checksessions(HttpContext.Request.Path) == false)
            {
                return RedirectToAction("login");
            }
            ViewData["name"] = HttpContext.Session.GetString("Name");
            return View();
        }
        // 
        // GET: /HelloWorld/Welcome/ 
        [HttpPost]
        public async Task<ActionResult> addstaff(IFormCollection Form)
        {
            Random r = new Random();
            string license = null;
            int rand;
            if (Form["dtype"].Equals("Driver"))
            {
                license = Form["licenceNumber"];
                rand = r.Next(170, 299);
            } else if (Form["dtype"].Equals("Admin"))
            {
                rand = r.Next(102, 199);
            } else
            {
                rand = r.Next(535, 699);
            }
            string id = "STF" + rand;
           var staff = new Staff
           (
                Form["dtype"],
               id,
              Form["idnumber"],
                Form["name"],
              Form["surname"],
              GetHash(Form["password"]),
              Form["email"],
                Form["telephone"],
              license,
               true
           );

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(clientHandler))
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.PostAsJsonAsync("api/Staffs/", staff).Result;



                if (response.IsSuccessStatusCode)
                {
                    string results = response.Content.ReadAsStringAsync().Result;
                    // user = JsonConvert.DeserializeObject<Staff>(results);
                }

            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(IFormCollection Form)
        {
            var StaffToEdit = new Staff
          (
                Form["dtype"],
              Form["staffid"],
             Form["idnumber"],
               Form["name"],
             Form["surname"],

            Form["password"],
             Form["email"],
               Form["telephone"],
             Form["licenceNumber"],
             true
          );

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(clientHandler))
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var getdata = await client.PutAsJsonAsync("api/Staffs/" + StaffToEdit.StaffId + "/", StaffToEdit);

                if (getdata.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Home");
                }

            }
            return View();

        }

        public async Task<IActionResult> Edit(string id)
        {

            if (checksessions(HttpContext.Request.Path) == false)
            {
                return RedirectToAction("login");
            }
            ViewData["name"] = HttpContext.Session.GetString("Name");
            Staff staff = new Staff();
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(clientHandler))
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getStaff = await client.GetAsync("api/Staffs/" + id);
                if (getStaff.IsSuccessStatusCode)
                {
                    var response = getStaff.Content.ReadAsStringAsync().Result;
                    staff = JsonConvert.DeserializeObject<Staff>(response);
                }
            }



            return View(staff);
        }





        //public async Task<ActionResult> Delete(string id)
        //{
        //    if (id is null)
        //    {
        //        throw new ArgumentNullException(nameof(id));
        //    }

        //    HttpClientHandler clientHandler = new HttpClientHandler();
        //    clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
        //    using (var client = new HttpClient(clientHandler))
        //    {
        //        client.BaseAddress = new Uri(baseURL);
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

        //        var getdata = await client.DeleteAsync("api/Staffs/" + id);

        //        if (getdata.IsSuccessStatusCode)
        //        {
        //            return RedirectToAction("Index", "Home");
        //        }

        //    }
        //    return RedirectToAction("Index", "Home");


        //}

        public async Task<ActionResult> Delete(string id)
        {
            Staff temp = new Staff();
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(clientHandler))
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getdata = await client.GetAsync("api/Staffs/" + id);

                if (getdata.IsSuccessStatusCode)
                {


                    var response2 = getdata.Content.ReadAsStringAsync().Result;
                    temp = JsonConvert.DeserializeObject<Staff>(response2);


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

                var result = client.PutAsJsonAsync("api/Staffs/" + temp.StaffId + "/", temp).Result;

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("Index", "Home");
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
