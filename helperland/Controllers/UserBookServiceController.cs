﻿using Helperland.Data;
using Helperland.Models;
using Helperland.viewmodel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Helperland.Controllers
{
    public class UserBookServiceController : Controller
    {
        private readonly HelperlandContext _helperlandContext;

        public UserBookServiceController(HelperlandContext helperlandContext)
        {
            _helperlandContext = helperlandContext;
        }

        [HttpPost]
        public IActionResult CheckPostalCode(BookServiceViewModel bookServiceViewModel)
        {

            var spdetails = (from splist in _helperlandContext.Users
                             where splist.UserTypeId == 2 && splist.ZipCode == bookServiceViewModel.zipCodeViewModel.zipcode
                             select new
                             {
                                 splist.UserId,
                                 splist.FirstName,
                                 splist.LastName
                             }).ToList();

            if (spdetails.FirstOrDefault() != null)
            {
                HttpContext.Session.SetString("again_called", "spfound");
                HttpContext.Session.SetString("zipcode", bookServiceViewModel.zipCodeViewModel.zipcode);
                ViewBag.bookservice = "bookservice";
                return View("BookService");
            }
            else
            {
                HttpContext.Session.SetString("again_called", "temp");
                return RedirectToAction("BookService", "Home");
            }
        }

        [HttpPost]
        public IActionResult addServiceInfo(BookServiceViewModel bookServiceViewModel)
        {

            Debug.WriteLine("testing method call");

            var startdate_str = bookServiceViewModel.ServiceRequestViewModel.servicestartdate;
            Debug.WriteLine(" this is startdate_str " + startdate_str);
            DateTime startdate = DateTime.Parse(startdate_str);
            Debug.WriteLine(" this is start date " + startdate);
            var starttime = bookServiceViewModel.ServiceRequestViewModel.servicestrarttime;
            int s_hr = Int32.Parse(starttime.Substring(0, 2));
            int s_min = Int32.Parse(starttime.Substring(3, 2));
            TimeSpan servicestarttime = new TimeSpan(s_hr, s_min, 0);
            Debug.WriteLine("this is service start time" + servicestarttime);
            startdate = startdate.Date + servicestarttime;
            Debug.WriteLine("this is total date " + startdate);
            string userid = HttpContext.Session.GetString("UserId");
            string zip = HttpContext.Session.GetString("zipcode");
            float hours = bookServiceViewModel.ServiceRequestViewModel.servicehours;
            bool extraser1 = bookServiceViewModel.ServiceRequestViewModel.extraSer1;
            bool extraser2 = bookServiceViewModel.ServiceRequestViewModel.extraSer2;
            bool extraser3 = bookServiceViewModel.ServiceRequestViewModel.extraSer3;
            bool extraser4 = bookServiceViewModel.ServiceRequestViewModel.extraSer4;
            bool extraser5 = bookServiceViewModel.ServiceRequestViewModel.extraSer5;
            bool haspet = bookServiceViewModel.ServiceRequestViewModel.haspets;
            var getextraservice = extraser1 + "" + extraser2 + "" + extraser3 + "" + extraser4 + "" + extraser5;
            Debug.WriteLine(getextraservice);
            var trueto1 = getextraservice.Replace("True", "1");
            var falseto0 = trueto1.Replace("False", "0");
            int extraserInt = Int32.Parse(falseto0);
            Debug.WriteLine(falseto0);
            float subtotal = hours * 20;
            double extra_hr = 0.0;
            if (extraser1) { subtotal += 10; extra_hr += 0.5; }
            if (extraser2) { subtotal += 10; extra_hr += 0.5; }
            if (extraser3) { subtotal += 10; extra_hr += 0.5; }
            if (extraser4) { subtotal += 10; extra_hr += 0.5; }
            if (extraser5) { subtotal += 10; extra_hr += 0.5; }
            decimal total = new decimal(subtotal);

            Debug.WriteLine("this is service start time " + startdate);
            ServiceRequest service = new ServiceRequest()
            {
                UserId = Int32.Parse(userid),
                ServiceId = 1,
                ZipCode = zip,
                ServiceStartDate = startdate,
                ServiceHourlyRate = 20,
                ServiceHours = hours,
                ExtraHours = extra_hr,
                SubTotal = total,
                TotalCost = total,
                PaymentDue = false,
                HasPets = haspet,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                Distance = 10
            };
            _helperlandContext.ServiceRequests.Add(service);
            _helperlandContext.SaveChanges();

            var getservicerequestid = (from temp in _helperlandContext.ServiceRequests
                                       where temp.UserId == Int32.Parse(userid)
                                       orderby temp.ServiceRequestId
                                       select temp.ServiceRequestId).Last();
            ServiceRequestExtra service1 = new ServiceRequestExtra()
            {
                ServiceRequestId = getservicerequestid,
                ServiceExtraId = extraserInt
            };
            _helperlandContext.ServiceRequestExtras.Add(service1);
            _helperlandContext.SaveChanges();
            var addressid = bookServiceViewModel.addressId;

            var address = (from uaddress in _helperlandContext.UserAddresses
                           where uaddress.AddressId == addressid
                           select new
                           {

                               addressline1 = uaddress.AddressLine1,

                               city = uaddress.City,
                               phonenumber = uaddress.Mobile,

                               postalcode = uaddress.PostalCode
                           });

            var u_email = _helperlandContext.Users.Where(id => id.UserId == Int32.Parse(userid)).FirstOrDefault();

            ServiceRequestAddress serviceRequestAddress = new ServiceRequestAddress()
            {
                ServiceRequestId = getservicerequestid,
                AddressLine1 = address.FirstOrDefault().addressline1,
                City = address.FirstOrDefault().city,
                PostalCode = address.FirstOrDefault().postalcode,
                Mobile = address.FirstOrDefault().phonenumber,
                Email = u_email.Email

            };
            _helperlandContext.ServiceRequestAddresses.Add(serviceRequestAddress);
            _helperlandContext.SaveChanges();

            return RedirectToAction("Index", "Home");

        }

    }
}
