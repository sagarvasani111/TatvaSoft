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
                if (user != null && user.Email == loginViewModel.Email && user.Password == loginViewModel.Password && user.UserTypeId ==1)
                {
                    HttpContext.Session.SetString("UserId",
                                                  user.UserId.ToString());
                    HttpContext.Session.SetString("FirstName", user.FirstName);
                    HttpContext.Session.SetString("UserTypeId", user.UserTypeId.ToString());

                    return RedirectToAction("Index", "Home");
                }
                else if(user != null && user.Email == loginViewModel.Email && user.Password == loginViewModel.Password && user.UserTypeId == 2)
                      {
                        HttpContext.Session.SetString("UserId",
                                                      user.UserId.ToString());
                        HttpContext.Session.SetString("FirstName", user.FirstName);
                        HttpContext.Session.SetString("UserTypeId", user.UserTypeId.ToString());

                        return RedirectToAction("CustomerView", "Customer");
                      }
                else if (user != null && user.Email == loginViewModel.Email && user.Password == loginViewModel.Password && user.UserTypeId == 3)
                {
                    HttpContext.Session.SetString("UserId",
                                                  user.UserId.ToString());
                    HttpContext.Session.SetString("FirstName", user.FirstName);
                    HttpContext.Session.SetString("UserTypeId", user.UserTypeId.ToString());

                    return RedirectToAction("SPServiceRequest", "ServiceProvider");
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
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult MySettingTab()
        {
            return RedirectToAction("CustomerSettings", "Customer");
        }


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



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
        
    }


   

}
