using Helperland.Controllers;
using Helperland.Data;
using Helperland.Models;
using Helperland.viewmodel;
using HelperLand.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HelperLand.Controllers
{
    public class CustomerController : HomeController
    {
        private readonly ILogger<HomeController> logger;
        private readonly HelperlandContext helperlandContext;

        public CustomerController(ILogger<HomeController> logger, HelperlandContext helperlandContext)
            : base(logger, helperlandContext)
        {
            this.logger = logger;
            this.helperlandContext = helperlandContext;
        }

        public IActionResult CustomerDashboard()
        {
            //int a = int.Parse(HttpContext.Session.GetString("UserId"));
            var x = helperlandContext.ServiceRequests.Where(m => m.UserId == 11).ToList();
            DashboardViewModel requestList = new DashboardViewModel();
            requestList.services = new List<ServiceRequest>();
            requestList.serviceEnd = new List<DateTime>();
            requestList.spName = new List<string>();
            if (x != null)
            {
                foreach (var z in x)
                {
                    if (z.ServiceProviderId != null)
                    {
                        var sp = helperlandContext.Users.Find(z.ServiceProviderId);
                        requestList.spName.Add(sp.FirstName + " " + sp.LastName);
                    }
                    else if (z.ServiceProviderId == null)
                    {
                        requestList.spName.Add(null);
                    }
                    requestList.services.Add(z);
                    var y = z.ServiceStartDate.AddHours(z.ServiceHours);
                    requestList.serviceEnd.Add(y);

                }
            }
            return View(requestList);
        }

        public IActionResult CustomerServiceHistory()
        {
            //var a = int.Parse(HttpContext.Session.GetString("UserId"));
            var x = helperlandContext.ServiceRequests.Where(m => m.UserId == 11).ToList();
            DashboardViewModel requestList = new DashboardViewModel();
            requestList.services = new List<ServiceRequest>();
            requestList.serviceEnd = new List<DateTime>();
            requestList.spName = new List<string>();
            requestList.rate = new List<decimal>();
            if (x != null)
            {
                foreach (var z in x)
                {
                    var r = helperlandContext.Ratings.Where(m => m.ServiceRequestId == z.ServiceRequestId).FirstOrDefault();
                    if (z.ServiceProviderId != null)
                    {
                        var sp = helperlandContext.Users.Find(z.ServiceProviderId);
                        requestList.spName.Add(sp.FirstName + " " + sp.LastName);
                        if (r != null)
                        {
                            requestList.rate.Add(r.Ratings);
                        }
                    }
                    if (z.ServiceProviderId == null)
                    {
                        requestList.spName.Add(null);
                    }
                    if (r == null)
                    {
                        requestList.rate.Add(0);
                    }
                    requestList.services.Add(z);
                    var y = z.ServiceStartDate.AddHours(z.ServiceHours);
                    requestList.serviceEnd.Add(y);

                }
            }
            return View(requestList);
        }

        public IActionResult CustomerSettings()
        {
            //var a = int.Parse(HttpContext.Session.GetString("UserId"));
            DashboardViewModel settings = new DashboardViewModel();
            settings.user = helperlandContext.Users.Find(11);
            if (settings.user.DateOfBirth != null)
            {
                var d = settings.user.DateOfBirth.GetValueOrDefault();
                settings.Day = d.Day;
                settings.Month = d.Month;
                settings.Year = d.Year;
            }
            settings.Address = helperlandContext.UserAddresses.Where(m => m.UserId == 11).ToList();
            return View(settings);
        }

        //[HttpPost]
        //public IActionResult ServiceReschedule(ServiceRescheduleViewModel model)
        //{
        //    if (model.serviceDate == null)
        //    {
        //        return Json(new { success = false });
        //    }
        //    else
        //    {
        //        var id = model.ServiceId;
        //        var p = helperlandContext.ServiceRequests.FirstOrDefault(m => m.ServiceId == id);
        //        var spService = helperlandContext.ServiceRequests.Where(m => m.ServiceProviderId == p.ServiceProviderId).ToList();
        //        if (spService.Any())
        //        {

        //            foreach (var z in spService)
        //            {
        //                if (z.ServiceId != id)
        //                {
        //                    var date = z.ServiceStartDate.ToString("yyyy-MM-dd");
        //                    if (date == model.serviceDate)
        //                    {
        //                        var startH = z.ServiceStartDate.ToString("HH");
        //                        var startM = z.ServiceStartDate.ToString("mm");
        //                        if (startM.Equals("30"))
        //                        {
        //                            startM = "5";
        //                        }
        //                        var start = double.Parse(startH + "." + startM);
        //                        var tmp = z.ServiceHours;
        //                        var end = tmp + start;
        //                        var updateendTime = model.serviceTime + tmp;
        //                        if ((model.serviceTime >= start && model.serviceTime <= end) || (updateendTime >= start && updateendTime <= end))
        //                        {
        //                            return Json(new { success = false });
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        var sDate = Convert.ToDateTime(model.serviceDate);
        //        var s = sDate.AddHours(model.serviceTime);
        //        p.ServiceStartDate = s;
        //        helperlandContext.ServiceRequests.Update(p);
        //        helperlandContext.SaveChanges();
        //        return Json(new { success = true });
        //    }
        //}

        [HttpPost]
        public IActionResult ServiceCancel(CancleServiceViewModel model)
        {
            var a = helperlandContext.ServiceRequests.Where(m => m.ServiceId == model.ServiceId).FirstOrDefault();
            a.Status = 2;
            helperlandContext.ServiceRequests.Update(a);
            helperlandContext.SaveChanges();
            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult Ratting(Rating model)
        {
            var a = helperlandContext.ServiceRequests.Where(m => m.ServiceRequestId == model.ServiceRequestId).FirstOrDefault();
            model.RatingFrom = a.UserId;
            model.RatingTo = (int)a.ServiceProviderId;
            model.RatingDate = DateTime.Now;
            helperlandContext.Ratings.Add(model);
            helperlandContext.SaveChanges();
            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult SaveDetails(SaveDetailsViewModel model)
        {
            //var a = int.Parse(HttpContext.Session.GetString("UserId"));
            var user = helperlandContext.Users.Find(11);
            if (user != null)
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Mobile = model.Mobile;
                user.LanguageId = model.LanguageId;
                user.DateOfBirth = model.DateOfBirth;
                helperlandContext.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = true });
        }


        [HttpPost]
        public IActionResult UpdateAddress(UserAddress model)
        {
            //var a = int.Parse(HttpContext.Session.GetString("UserId"));
            model.UserId = 11;
            helperlandContext.UserAddresses.Update(model);
            helperlandContext.SaveChanges();
            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult DeleteAddress(DeleteAddressViewModel model)
        {
            var a = helperlandContext.UserAddresses.Find(model.AddressId);
            helperlandContext.UserAddresses.Remove(a);
            helperlandContext.SaveChanges();
            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult AddAddress(UserAddress model)
        {
            //var a = int.Parse(HttpContext.Session.GetString("UserId"));
            model.UserId = 11;
            helperlandContext.UserAddresses.Add(model);
            helperlandContext.SaveChanges();
            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult UpdatePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, errors = ModelState.Values.Where(i => i.Errors.Count > 0) });
            }
            else
            {
                var a = int.Parse(HttpContext.Session.GetString("UserId"));
                var user = helperlandContext.Users.Find(a);
                if (user.Password != model.old)
                {
                    return Json(new { success = false, error = "Password doen't match" });
                }
                else
                {
                    user.Password = model.new1;
                    helperlandContext.Users.Update(user);
                    helperlandContext.SaveChanges();
                    return Json(new { success = true });
                }
            }
            return Json(new { success = true });
        }

    }
}
