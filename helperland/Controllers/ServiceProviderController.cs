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
    public class ServiceProviderController : HomeController
    {
        private readonly ILogger<HomeController> logger;
        private readonly HelperlandContext helperlandContext;

        public ServiceProviderController(ILogger<HomeController> logger, HelperlandContext helperlandContext) :
            base(logger, helperlandContext)
        {
            this.logger = logger;
            this.helperlandContext = helperlandContext;
        }

        public IActionResult SPServiceRequest()
        {
            var spRequest = helperlandContext.ServiceRequests.Where(m => m.ServiceProviderId == null).ToList();
            ServiceProviderViewModel service = new ServiceProviderViewModel();
            service.addresses = new List<ServiceRequestAddress>();
            service.services = new List<ServiceRequest>();
            service.serviceEnd = new List<DateTime>();
            service.custName = new List<string>();
            service.extras = new List<string>();
            if (spRequest.Any())
            {
                foreach (var x in spRequest)
                {
                    service.services.Add(x);
                    var address = helperlandContext.ServiceRequestAddresses
                        .Where(m => m.ServiceRequestId == x.ServiceRequestId).FirstOrDefault();
                    service.addresses.Add(address);
                    var y = x.ServiceStartDate.AddHours(x.ServiceHours);
                    service.serviceEnd.Add(y);
                    var sp = helperlandContext.Users.Find(x.UserId);
                    service.custName.Add(sp.FirstName + " " + sp.LastName);
                    var extraList = helperlandContext.ServiceRequestExtras.Where(m => m.ServiceRequestId == x.ServiceRequestId).ToList();
                    if (extraList.Any())
                    {
                        var s = "";
                        foreach (var p in extraList)
                        {
                            if (p.ServiceExtraId == 1)
                                s += "Inside Cabinets";
                            if (p.ServiceExtraId == 2)
                                s += " Inside Fridge";
                            if (p.ServiceExtraId == 3)
                                s += " Inside Oven";
                            if (p.ServiceExtraId == 4)
                                s += " Laundry wash & dry";
                            if (p.ServiceExtraId == 5)
                                s += " Interior Windows";
                        }
                        service.extras.Add(s);
                    }
                    else
                    {
                        service.extras.Add("");
                    }

                }
            }
            return View(service);
        }

        [HttpPost]
        public IActionResult AcceptService(AcceptServiceViewModel model)
        {
            var a = int.Parse(HttpContext.Session.GetString("UserId"));
            var data = helperlandContext.ServiceRequests.Find(model.ServiceRequestId);
            if (a != 0)
            {
                var services = helperlandContext.ServiceRequests.Where(m => m.ServiceProviderId == a).ToList();
                if (services.Any())
                {
                    var date = data.ServiceStartDate.ToString("yyyy-MM-dd");
                    foreach (var i in services)
                    {
                        var dateCompare = i.ServiceStartDate.ToString("yyyy-MM-dd");
                        if (date == dateCompare)
                        {
                            var startH = data.ServiceStartDate.ToString("HH");
                            var startM = data.ServiceStartDate.ToString("mm");
                            if (startM.Equals("30"))
                            {
                                startM = "5";
                            }
                            var s1Start = double.Parse(startH + "." + startM);

                            var startH2 = i.ServiceStartDate.ToString("HH");
                            var startM2 = i.ServiceStartDate.ToString("mm");
                            if (startM2.Equals("30"))
                            {
                                startM2 = "5";
                            }
                            var s2Start = double.Parse(startH2 + "." + startM2);

                            var s1Total = data.ServiceHours;
                            var s1End = s1Start + s1Total;

                            var s2Total = i.ServiceHours;
                            var s2End = s2Start + s2Total;

                            if (s1Start == s2Start || s1Start == s2End)
                            {
                                return Json(new { success = false });
                            }
                            if (s1Start > s2Start && s1Start > s2End)
                            {
                                if (s1Start - s2End < 1)
                                {
                                    return Json(new { success = false });
                                }
                            }
                            if (s1Start < s2Start && s1End < s2Start)
                            {
                                if (s2Start - s1End < 1)
                                {
                                    return Json(new { success = false });
                                }
                            }

                        }
                    }
                    data.ServiceProviderId = a;
                    helperlandContext.ServiceRequests.Update(data);
                    helperlandContext.SaveChanges();
                    return Json(new { success = true });
                }
                else
                {
                    data.ServiceProviderId = a;
                    helperlandContext.ServiceRequests.Update(data);
                    helperlandContext.SaveChanges();
                    return Json(new { success = true });
                }
            }

            return Json(new { success = false });
        }
        public IActionResult SPUpcomingService()
        {
            var a = int.Parse(HttpContext.Session.GetString("UserId"));
            ServiceProviderViewModel service = new ServiceProviderViewModel();
            service.time = DateTime.Now;
            service.addresses = new List<ServiceRequestAddress>();
            service.services = new List<ServiceRequest>();
            service.serviceEnd = new List<DateTime>();
            service.custName = new List<string>();
            service.extras = new List<string>();
            if (a != 0)
            {
                var sData = helperlandContext.ServiceRequests.Where(m => m.ServiceProviderId == a).ToList();
                if (sData.Any())
                {
                    foreach (var i in sData)
                    {
                        if (i.Status == null)
                        {
                            service.services.Add(i);
                            var addRess = helperlandContext.ServiceRequestAddresses
                                .Where(m => m.ServiceRequestId == i.ServiceRequestId).FirstOrDefault();
                            service.addresses.Add(addRess);
                            var y = i.ServiceStartDate.AddHours(i.ServiceHours);
                            service.serviceEnd.Add(y);
                            var sp = helperlandContext.Users.Find(i.UserId);
                            service.custName.Add(sp.FirstName + " " + sp.LastName);
                            var extraList = helperlandContext.ServiceRequestExtras.Where(m => m.ServiceRequestId == i.ServiceRequestId).ToList();
                            if (extraList.Any())
                            {
                                var s = "";
                                foreach (var p in extraList)
                                {
                                    if (p.ServiceExtraId == 1)
                                        s += "Inside Cabinets";
                                    if (p.ServiceExtraId == 2)
                                        s += " Inside Fridge";
                                    if (p.ServiceExtraId == 3)
                                        s += " Inside Oven";
                                    if (p.ServiceExtraId == 4)
                                        s += " Laundry wash & dry";
                                    if (p.ServiceExtraId == 5)
                                        s += " Interior Windows";
                                }
                                service.extras.Add(s);
                            }
                            else
                            {
                                service.extras.Add("");
                            }
                        }
                    }
                }
            }
            return View(service);
        }

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
        public IActionResult ServiceComplete(AcceptServiceViewModel model)
        {
            var a = helperlandContext.ServiceRequests.Find(model.ServiceRequestId);
            a.Status = 1;
            helperlandContext.ServiceRequests.Update(a);
            helperlandContext.SaveChanges();
            return Json(new { success = true });
        }


        public IActionResult SPServiceHistory()
        {
            var a = int.Parse(HttpContext.Session.GetString("UserId"));
            //var a = 17;
            ServiceProviderViewModel service = new ServiceProviderViewModel();
            service.time = DateTime.Now;
            service.addresses = new List<ServiceRequestAddress>();
            service.services = new List<ServiceRequest>();
            service.serviceEnd = new List<DateTime>();
            service.custName = new List<string>();
            service.extras = new List<string>();
            if (a != 0)
            {
                var sData = helperlandContext.ServiceRequests.Where(m => m.ServiceProviderId == a).ToList();
                if (sData.Any())
                {
                    foreach (var i in sData)
                    {
                        if (i.Status == 1)
                        {
                            service.services.Add(i);
                            var addRess = helperlandContext.ServiceRequestAddresses
                                .Where(m => m.ServiceRequestId == i.ServiceRequestId).FirstOrDefault();
                            service.addresses.Add(addRess);
                            var y = i.ServiceStartDate.AddHours(i.ServiceHours);
                            service.serviceEnd.Add(y);
                            var sp = helperlandContext.Users.Find(i.UserId);
                            service.custName.Add(sp.FirstName + " " + sp.LastName);
                            var extraList = helperlandContext.ServiceRequestExtras.Where(m => m.ServiceRequestId == i.ServiceRequestId).ToList();
                            if (extraList.Any())
                            {
                                var s = "";
                                foreach (var p in extraList)
                                {
                                    if (p.ServiceExtraId == 1)
                                        s += "Inside Cabinets";
                                    if (p.ServiceExtraId == 2)
                                        s += " Inside Fridge";
                                    if (p.ServiceExtraId == 3)
                                        s += " Inside Oven";
                                    if (p.ServiceExtraId == 4)
                                        s += " Laundry wash & dry";
                                    if (p.ServiceExtraId == 5)
                                        s += " Interior Windows";
                                }
                                service.extras.Add(s);
                            }
                            else
                            {
                                service.extras.Add("");
                            }
                        }
                    }
                }
            }

            return View(service);
        }


        public IActionResult SPRatting()
        {
            var a = int.Parse(HttpContext.Session.GetString("UserId"));
            //var a = 17;
            ServiceProviderViewModel service = new ServiceProviderViewModel();
            service.services = new List<ServiceRequest>();
            service.serviceEnd = new List<DateTime>();
            service.custName = new List<string>();
            service.ratings = new List<Rating>();
            service.ratingRes = new List<string>();
            if (a != 0)
            {
                var r = helperlandContext.Ratings.Where(m => m.RatingTo == a).ToList();
                if (r.Any())
                {
                    foreach (var i in r)
                    {
                        service.ratings.Add(i);
                        if (i.Ratings <= 1)
                        {
                            service.ratingRes.Add("Very Poor");
                        }
                        else if (i.Ratings <= 2)
                        {
                            service.ratingRes.Add("Poor");
                        }
                        else if (i.Ratings <= 3)
                        {
                            service.ratingRes.Add("Good");
                        }
                        else if (i.Ratings <= 4)
                        {
                            service.ratingRes.Add("Very Good");
                        }
                        else if (i.Ratings <= 5)
                        {
                            service.ratingRes.Add("Excellent");
                        }
                        var s = helperlandContext.ServiceRequests.Find(i.ServiceRequestId);
                        service.services.Add(s);
                        var y = s.ServiceStartDate.AddHours(s.ServiceHours);
                        service.serviceEnd.Add(y);
                        var sp = helperlandContext.Users.Find(i.RatingFrom);
                        service.custName.Add(sp.FirstName + " " + sp.LastName);
                    }
                }
            }
            return View(service);
        }

        public IActionResult SPBlock()
        {
            var a = int.Parse(HttpContext.Session.GetString("UserId"));
            //var a = 17;
            var service = helperlandContext.ServiceRequests.Where(m => m.ServiceProviderId == a).Select(m => m.UserId).Distinct().ToList();
            ServiceProviderViewModel model = new ServiceProviderViewModel();
            model.favorites = new List<FavoriteAndBlocked>();
            model.custName = new List<string>();
            model.userid = new List<int>();

            if (service.Any())
            {
                foreach (var i in service)
                {
                    var user = helperlandContext.Users.Where(m => m.UserId == i).FirstOrDefault();
                    model.custName.Add(user.FirstName + " " + user.LastName);
                    model.userid.Add(user.UserId);
                    var b = helperlandContext.FavoriteAndBlockeds.Where(m => m.TargetUserId == i).ToList();
                    FavoriteAndBlocked c = new FavoriteAndBlocked();
                    if (b.Any())
                    {
                        foreach (var x in b)
                        {
                            if (x.UserId == a)
                                c = x;
                        }
                    }
                    if (c != null)
                    {
                        model.favorites.Add(c);
                    }
                    else
                    {
                        model.favorites.Add(null);
                    }
                }
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult BlockandUnblock(FavoriteAndBlocked model)
        {
            var a = int.Parse(HttpContext.Session.GetString("UserId"));
            //var a = 17;
            var favBlock = helperlandContext.FavoriteAndBlockeds.Where(m => m.UserId == a).FirstOrDefault();
            if (favBlock != null)
            {
                if (favBlock.TargetUserId == model.TargetUserId)
                {
                    if (favBlock.IsBlocked)
                    {
                        favBlock.IsBlocked = false;
                        favBlock.IsFavorite = true;
                        helperlandContext.FavoriteAndBlockeds.Update(favBlock);
                        helperlandContext.SaveChanges();
                        return Json(new { success = true });
                    }
                    else
                    {
                        favBlock.IsFavorite = false;
                        favBlock.IsBlocked = true;
                        helperlandContext.FavoriteAndBlockeds.Update(favBlock);
                        helperlandContext.SaveChanges();
                        return Json(new { success = true });
                    }
                }

                else
                {
                    model.UserId = a;
                    helperlandContext.FavoriteAndBlockeds.Add(model);
                    helperlandContext.SaveChanges();
                    return Json(new { success = true });
                }
            }

            else
            {
                model.UserId = a;
                helperlandContext.FavoriteAndBlockeds.Add(model);
                helperlandContext.SaveChanges();
                return Json(new { success = true });
            }
        }

        public IActionResult SPSettings()
        {
            var a = int.Parse(HttpContext.Session.GetString("UserId"));
            //var a = 17;

            ServiceProviderViewModel model = new ServiceProviderViewModel();

            model.userAddress = helperlandContext.UserAddresses.Where(m => m.UserId == a).FirstOrDefault();
            model.user = helperlandContext.Users.Find(a);

            if (model.user.DateOfBirth != null)
            {
                var d = model.user.DateOfBirth.GetValueOrDefault();
                model.Day = d.Day;
                model.Month = d.Month;
                model.Year = d.Year;
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult UpdateSPDetails(ServiceProviderDetailsViewModel model)
        {
            var a = int.Parse(HttpContext.Session.GetString("UserId"));
            //var a = 17;
            if (a != 0)
            {
                var user = helperlandContext.Users.Find(a);
                if (user != null)
                {
                    user.FirstName = model.User.FirstName;
                    user.LastName = model.User.LastName;
                    user.Mobile = model.User.Mobile;
                    user.NationalityId = model.User.NationalityId;
                    user.DateOfBirth = model.User.DateOfBirth;
                    user.Gender = model.User.Gender;
                    user.UserProfilePicture = model.User.UserProfilePicture;
                    var address = helperlandContext.UserAddresses.Find(model.Address.AddressId);
                    address.AddressLine1 = model.Address.AddressLine1;
                    address.AddressLine2 = model.Address.AddressLine2;
                    address.City = model.Address.City;
                    address.PostalCode = model.Address.PostalCode;

                    helperlandContext.Users.Update(user);
                    helperlandContext.UserAddresses.Update(address);

                    helperlandContext.Users.Update(user);
                    helperlandContext.UserAddresses.Update(address);

                    helperlandContext.SaveChanges();
                    return Json(new { success = true });
                }
            }
            return Json(new { success = false });
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
                var a = 17;
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
        }

    }
}