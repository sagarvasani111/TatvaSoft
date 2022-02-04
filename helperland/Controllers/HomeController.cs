using Helperland.Data;
using Helperland.Models;
using Helperland.viewmodel;
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
        private readonly ILogger<HomeController> _logger;
        private readonly HelperlandContext helperlandContext;

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

        public IActionResult ContactUS()
        {
            return View();
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
                UserTypeId = 1,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                ModifiedBy = 0,

            };

            helperlandContext.Users.Add(DbUserModel);
            helperlandContext.SaveChanges();

            return RedirectToAction("Index");
            }
            
            return RedirectToAction("Index");

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
                UserTypeId = 1,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                ModifiedBy = 0,

            };

            helperlandContext.Users.Add(DbUserModel);
            helperlandContext.SaveChanges();
            return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            User user = helperlandContext.Users.Where(x => x.Email == email).FirstOrDefault();
            if(user == null)
            {
                return RedirectToAction("Register");
            }

            else
            {
                if(user.Email == email && user.Password == password && user.UserTypeId == 1)
                {
                    return View("ServiceRequest");
                }

                else
                {
                    return View("Index");
                }
            }
        }

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgetPassword(string email)
        {
           var isEmailAlreadyExist = helperlandContext.Users.Any(x => x.Email == email);
           if (isEmailAlreadyExist)
           {
                var user = helperlandContext.Users.Where(x => x.Email == email).FirstOrDefault();
                ViewBag.Data = user.UserId;
                return View("ChangePassword");
           }

           else
            {
                return RedirectToAction("Price");
            }
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(RegisterViewModel model)
        {
            if(model.Password == model.ConfirmPassword)
            {
                var user = helperlandContext.Users.Where(x => x.UserId == model.UserId).FirstOrDefault();
                user.Password = model.Password;
                helperlandContext.Users.Update(user);
                helperlandContext.SaveChanges();
                return RedirectToAction("Index");

            }
          return View("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
        
    }
}
