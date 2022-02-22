using Helperland.Data;
using Helperland.Models;
using Helperland.viewmodel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Helperland.Controllers
{
    public class HomeController : Controller
    {
        public static int cnt = 0;
        private readonly ILogger<HomeController> _logger;
        private readonly HelperlandContext helperlandContext;
        BookServiceViewModel userAddresses = new BookServiceViewModel();

        public HomeController(ILogger<HomeController> logger, HelperlandContext Helperland)
        {
            _logger = logger;
            helperlandContext = Helperland;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Prices()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ContactUS()
        {
            
            return View();
        }
        [HttpPost]
        public IActionResult ContactUS(ContactUsVIewModel contactUsVIewModel)
        {
            
            ContactU DbUserModel = new ContactU()
            {
                Name = contactUsVIewModel.Name + " " + contactUsVIewModel.LastName,
                Email = contactUsVIewModel.Email,
                Subject = contactUsVIewModel.Subject,
                PhoneNumber = contactUsVIewModel.PhoneNumber,
                Message = contactUsVIewModel.Message,
                CreatedOn = DateTime.Now
            };

            helperlandContext.ContactUs.Add(DbUserModel);
            helperlandContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult FAQ()
        {
            return View();
        }
         public IActionResult ServiceRequest()
        {
            return View();

        }

        public IActionResult About()
        {
            return View();
        }
        [HttpGet]
       public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterViewModel NewUserData)
        {
            User user = helperlandContext.Users.Where(x => x.Email == NewUserData.Email).FirstOrDefault();
            if(user == null && ModelState.IsValid)
           
            {
                User newuser = new User();
                newuser.CreatedDate = DateTime.Today;
                newuser.ModifiedDate = DateTime.Today;
                newuser.Mobile = NewUserData.Mobile.ToString();
                newuser.FirstName = NewUserData.FirstName;
                newuser.LastName = NewUserData.LastName;
                newuser.Email = NewUserData.Email;
                newuser.Password = NewUserData.Password;
                newuser.UserTypeId = 1;
                helperlandContext.Users.Add(newuser);
                helperlandContext.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            else
             {
                return View();
            }
        }      
        public IActionResult BecomeAPro()
        {
            return View();
        }
        [HttpPost]
        public IActionResult BecomeAPro(RegisterViewModel NewUserData)
        {
            var userEmailExist = helperlandContext.Users.Where(x => x.Email == NewUserData.Email).FirstOrDefault();
            if(ModelState.IsValid)
            { 
            User DbUserModel = new User()
            {
                FirstName = NewUserData.FirstName,
                LastName = NewUserData.LastName,
                Email = NewUserData.Email,
                Password = NewUserData.Password,
                Mobile = NewUserData.Mobile.ToString(),
                UserTypeId = 2,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                ModifiedBy = 0,

            };

            helperlandContext.Users.Add(DbUserModel);
            helperlandContext.SaveChanges();
            return RedirectToAction("Index");
            }

            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Index_Login()
        {
            ViewBag.modal = string.Format("invalid");
            return View("~/Views/Home/Register.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                User user = helperlandContext.Users.Where(x => x.Email == loginViewModel.Email.Trim() && x.Password == loginViewModel.Password.Trim()).FirstOrDefault();

                /*var details = (from userlist in _helperlandContext.Users
                               where userlist.Email == loginViewModel.Email && userlist.Password == loginViewModel.Password
                               select new
                               {
                                   userlist.UserId,
                                   userlist.FirstName,
                                   userlist.UserTypeId
                               }).ToList();*/
                if (user != null)
                {
                    HttpContext.Session.SetString("UserId",
                                                  user.UserId.ToString());
                    HttpContext.Session.SetString("FirstName", user.FirstName);
                    HttpContext.Session.SetString("UserTypeId", user.UserTypeId.ToString());

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Credentials");
                    ViewBag.modal = string.Format("invalid");
                    return View("~/Views/Home/Index.cshtml");
                }
            }
            return View(loginViewModel);

        }

        public IActionResult BookService()
         {
            if (HttpContext.Session.GetString("FirstName") == null)
            {
                return RedirectToAction("Index_Login", "Home");
            }
            else
            {
                string uname = HttpContext.Session.GetString("FirstName");
                ViewBag.Uname = uname;
                ViewBag.login_check = String.Format("loggedin");
                if (cnt != 0)
                {
                    if (HttpContext.Session.GetString("again_called") != "spfound")
                   {
                        HttpContext.Session.SetString("ss_step_2", "notset");
                        ViewBag.foundsp = string.Format("spnotfound");
                        string temp_var = ViewBag.foundsp;
                        Debug.WriteLine("this is viewbag foundsp" + temp_var);
                    }
                    else
                    {
                        ViewBag.foundsp = null;
                        HttpContext.Session.SetString("ss_step_2", "notset");

                    }

            }
                cnt = 1;
                getAddress();

                return View(userAddresses);
            }

        }

        //[HttpPost]
        //public bool CheckPostalCodeAvailability([FromBody] ZipcodeAvailability bookService)
        //{
        //    bool isAvailable = helperlandContext.Users.Any(val => val.ZipCode == bookService.ZipCode);
        //    return isAvailable;
        //}
        //public class ServiceRequestModel
        //{
        //    public string ZipCode { get; set; }
        //    public string Address1 { get; set; }
        //    public double Hours { get; set; }
        //    public string BookingStartTime { get; set; }
        //    public DateTime ServiceStartDate {
        //        get
        //        {
        //            DateTime date = DateTime.MinValue;
        //            if (!string.IsNullOrEmpty(this.BookingStartTime))
        //                DateTime.TryParse(this.BookingStartTime, out date);
        //            return date;
        //        }
        //    }
        //}

        //[HttpGet]
        //public IActionResult ForgetPassword()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public IActionResult ForgetPassword(string email)
        //{
        //   var isEmailAlreadyExist = helperlandContext.Users.Any(x => x.Email == email);
        //   if (isEmailAlreadyExist)
        //   {
        //        var user = helperlandContext.Users.Where(x => x.Email == email).FirstOrDefault();
        //        ViewBag.Data = user.UserId;
        //        return View("ChangePassword");
        //   }

        //   else
        //    {
        //        return RedirectToAction("Price");
        //    }
        //}

        //[HttpGet]
        //public IActionResult ChangePassword()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public IActionResult ChangePassword(RegisterViewModel model)
        //{
        //    if(model.Password == model.ConfirmPassword)
        //    {
        //        var user = helperlandContext.Users.Where(x => x.UserId == model.UserId).FirstOrDefault();
        //        user.Password = model.Password;
        //        helperlandContext.Users.Update(user);
        //        helperlandContext.SaveChanges();
        //        return RedirectToAction("Index");

        //    }
        //  return View("Index");
        //}

     

        public void getAddress()
        {
            Debug.WriteLine("this methd is called");

            HttpContext.Session.SetString("getaddress", "set");
            var userid = Int32.Parse(HttpContext.Session.GetString("UserId"));

            var addresses = (from uaddress in helperlandContext.UserAddresses
                             where uaddress.UserId == userid
                             select new AddressViewModel()
                             {

                                 addressline1 = uaddress.AddressLine1,

                                 city = uaddress.City,
                                 phonenumber = uaddress.Mobile,

                                 postalcode = uaddress.PostalCode
                             }).ToList();

            if (addresses.FirstOrDefault() != null)
            {
                
                userAddresses.address = new List<AddressViewModel>();
                int countAddress = 1;

                foreach (var add in addresses)
                {

                    add.id = countAddress;
                    userAddresses.address.Add(add);

                    countAddress += 1;
                }

            }

        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
        
    }


   

}
