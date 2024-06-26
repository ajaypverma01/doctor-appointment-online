﻿using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using DoctorPatient.Infrastructure;
using DoctorPatient.ViewModels;
using System.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using DoctorPatient.Email;

namespace DoctorPatient.Controllers
{
    public class HomeController : Controller
    {
        private HospitalDbContext db = new HospitalDbContext();

       
        public ActionResult Index()
        {            
            return View();
        }

        public ActionResult About()
        {
            IQueryable<AppointmentDateGroupViewModel> data = from appointment in db.Appointments
                                                             group appointment by appointment.Date into dateGroup
                                                             select new AppointmentDateGroupViewModel()
                                                             {
                                                                 AppointmentDate = dateGroup.Key,
                                                                 PatientCount = dateGroup.Count()
                                                             };
            return View(data.ToList());
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}