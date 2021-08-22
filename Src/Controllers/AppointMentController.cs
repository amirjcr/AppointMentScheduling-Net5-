using System;
using Microsoft.AspNetCore.Mvc;
using Src.Services.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Src.Helpers;



namespace Src.Controllers
{
    public class AppointMentController : Controller
    {

        private readonly IAppointmentService _service;
        public AppointMentController(IAppointmentService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.Duration = Helper.GetTimeDropDown();
            ViewBag.PatientList = await _service.GetPatientList();
            
            return View();
        }
    }
}