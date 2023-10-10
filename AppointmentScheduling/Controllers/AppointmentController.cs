using AppointmentScheduling.Models;
using AppointmentScheduling.Models.ViewModels;
using AppointmentScheduling.Services;
using AppointmentScheduling.Utility;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentScheduling.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        public async Task<IActionResult> Index()
        {
            var doctorlist = await _appointmentService.GetDoctorlist();
            var patientlist = await _appointmentService.GetPatientlist();
            var duration = Helper.GetTimeDropdown();
            

            ViewBag.DoctorList = doctorlist;
            ViewBag.PatientList = patientlist;
            ViewBag.Duration = duration;
            return View();
        }
    }
}
