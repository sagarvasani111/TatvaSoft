using Helperland.Controllers;
using Helperland.Data;
using Helperland.Models;
using Helperland.viewmodel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HelperLand.Controllers
{
    public class AdminController : HomeController
    {
        private readonly ILogger<HomeController> logger;
        private readonly HelperlandContext helperlandContext;

        public AdminController(ILogger<HomeController> logger, HelperlandContext helperlandContext) :
            base(logger, helperlandContext)
        {
            this.logger = logger;
            this.helperlandContext = helperlandContext;
        }

        public IActionResult AdminUser()
        {
            //var a = int.Parse(HttpContext.Session.GetString("UserId"));
            var user = helperlandContext.Users.Find(19);
            ViewBag.name = user.FirstName + " " + user.LastName;
            AdminViewModel model = new AdminViewModel();
            model.users = new List<User>();

            var uList = helperlandContext.Users.Where(m => m.UserId != 0).ToList();
            foreach (var i in uList)
            {
                model.users.Add(i);
            }

            return View(model);
        }

        public IActionResult AdminService()
        {
            //var a = int.Parse(HttpContext.Session.GetString("UserId"));
            var user = helperlandContext.Users.Find(19);
            ViewBag.name = user.FirstName + " " + user.LastName;
            AdminViewModel model = new AdminViewModel();
            model.services = new List<ServiceRequest>();
            model.address = new List<ServiceRequestAddress>();
            model.Name = new List<string>();
            model.serviceEnd = new List<DateTime>();
            model.serviceDate = new List<DateTime>();
            model.serviceStart = new List<double>();


            var sp = helperlandContext.ServiceRequests.Where(m => m.ServiceRequestId != 0).ToList();

            if (sp.Any())
            {
                foreach (var i in sp)
                {
                    model.services.Add(i);
                    model.serviceDate.Add(i.ServiceStartDate);
                    var startH = i.ServiceStartDate.ToString("HH");
                    var startM = i.ServiceStartDate.ToString("mm");
                    if (startM.Equals("30"))
                    {
                        startM = "5";
                    }
                    var start = double.Parse(startH + "." + startM);
                    model.serviceStart.Add(start);
                    var address = helperlandContext.ServiceRequestAddresses
                        .Where(m => m.ServiceRequestId == i.ServiceRequestId).FirstOrDefault();
                    model.address.Add(address);
                    var y = i.ServiceStartDate.AddHours(i.ServiceHours);
                    model.serviceEnd.Add(y);
                    var u = helperlandContext.Users.Find(i.UserId);
                    model.Name.Add(u.FirstName + " " + u.LastName);
                }

            }
            return View(model);
        }

        [HttpPost]
        public IActionResult AdminReschedule(AdminRescheduleViewModel model)
        {
            var sp = helperlandContext.ServiceRequests.Find(model.serviceId);
            var serviceAddress = helperlandContext.ServiceRequestAddresses.Where(m => m.ServiceRequestId == model.serviceId).FirstOrDefault();
            var service = helperlandContext.ServiceRequests.Where(m => m.ServiceProviderId == sp.ServiceProviderId).ToList();
            if (service.Any())
            {

                foreach (var z in service)
                {
                    if (z.ServiceId != sp.ServiceId && z.Status == null)
                    {
                        var date = z.ServiceStartDate.ToString("yyyy-MM-dd");
                        var date2 = model.ServiceDate.ToString("yyyy-MM-dd");
                        if (date == date2)
                        {
                            var startH = z.ServiceStartDate.ToString("HH");
                            var startM = z.ServiceStartDate.ToString("mm");
                            if (startM.Equals("30"))
                            {
                                startM = "5";
                            }
                            var start = double.Parse(startH + "." + startM);
                            var tmp = z.ServiceHours;
                            var end = tmp + start;
                            var updateendTime = model.time + tmp;
                            if ((model.time >= start && model.time <= end) || (updateendTime >= start && updateendTime <= end))
                            {
                                return Json(new { success = false });
                            }
                        }
                    }
                }
            }

            var sDate = Convert.ToDateTime(model.ServiceDate.ToString("yyyy-MM-dd"));
            var s = sDate.AddHours(model.time);
            sp.ServiceStartDate = s;
            helperlandContext.ServiceRequests.Update(sp);
            helperlandContext.SaveChanges();
            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult ActiveInActiveUser(DeleteAddressViewModel model)
        {
            var a = helperlandContext.Users.Find(model.AddressId);
            if (a.IsActive)
            {
                a.IsActive = false;
            }
            else
            {
                a.IsActive = true;
            }
            helperlandContext.Users.Update(a);
            helperlandContext.SaveChanges();
            return Json(new { success = true });

        }
    }
}
