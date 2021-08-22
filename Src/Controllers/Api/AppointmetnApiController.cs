using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Src.Services.Interfaces;
using System.Security.Claims;
using Src.Models.ViewModels.Appointment;
using Src.Models.ViewModels.Response;
using Src.Helpers;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Src.Controllers.Api
{
    [Route("api/Appointment")]
    [ApiController]
    public class AppointmetnApiController : Controller
    {

        private readonly IAppointmentService _service;
        private readonly string loginUserId;
        private readonly string role;
        private readonly IHttpContextAccessor _httpaccessor;

        public AppointmetnApiController(IAppointmentService service, IHttpContextAccessor httpaccessor)
        {
            _service = service;
            _httpaccessor = httpaccessor;
            loginUserId = _httpaccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            role = _httpaccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);

        }


        [HttpPost]
        [Route("SaveCalendarData")]
        public async Task<JsonResult> SaveCalendarData(AppointmentVm model)
        {
            GenericResponse<int> response = new GenericResponse<int>();

            if (ModelState.IsValid)
            {

                try
                {
                    response.Status = await _service.AddUpdate(model);

                    if (response.Status == 1)  // Adding condition
                        response.message = Helper.AppointmentAdded;

                    else if (response.Status == 2) //update senerio
                        response.message = Helper.AppointmentUpdated;

                }
                catch (Exception e)
                {
                    response.message = e.Message;
                    response.Status = Helper.faliercode;
                }

            }
            else
            {
                response.Status = 0;
            }

            return Json(response);
        }



        [HttpGet]
        [Route("GetCalendarData")]
        public async Task<IActionResult> GetCalendarData(string doctorId)
        {
            GenericResponse<List<AppointmentVm>> commonresponse = new GenericResponse<List<AppointmentVm>>();

            try
            {
                if (role == Helper.Patient)
                {
                    commonresponse.dataenum = await _service.PatientsEventById(loginUserId);
                    commonresponse.Status = Helper.successCode;
                }
                else if (role == Helper.Doctor)
                {
                    commonresponse.dataenum = await _service.DoctorEventsById(loginUserId);
                    commonresponse.Status = Helper.successCode;
                }
                else
                {
                    commonresponse.dataenum = await _service.DoctorEventsById(doctorId);
                    commonresponse.Status = Helper.successCode;
                }

            }
            catch (Exception ex)
            {
                commonresponse.message = ex.Message;
                commonresponse.Status = Helper.faliercode;
            }

            return Ok(commonresponse);
        }


        [HttpGet]
        [Route("GetCalendarDataById/{Id}")]
        public IActionResult GetCalendarData(int Id)
        {
            GenericResponse<AppointmentVm> commonresponse = new GenericResponse<AppointmentVm>();

            try
            {
                commonresponse.dataenum = _service.GetById(Id);
                commonresponse.Status = Helper.successCode;

            }
            catch (Exception ex)
            {
                commonresponse.message = ex.Message;
                commonresponse.Status = Helper.faliercode;
            }

            return Ok(commonresponse);
        }



        [HttpPost]
        [Route("DeleteData/{Id}")]
        public IActionResult DeletAppointment(object Id)
        {
            GenericResponse<int> response = new GenericResponse<int>();


            try
            {
                if ((int)Id > 0) //beacuse appointment have integer Primary Key
                {
                    var result = _service.DeleteAppointment(Id);

                    if (result > 0)
                    {
                        response.Status = Helper.successCode;
                        response.message = Helper.AppointmentDeleted;
                        response.dataenum = result;
                    }
                    else if (result == 0)
                    {
                        response.Status = Helper.faliercode;
                        response.message = Helper.AppointmentDeafultErrors;
                        response.dataenum = result;
                    }
                }
            }
            catch
            {

            }

            return Ok(response);
        }



        [HttpGet]
        [Route("ConfrimAppointment/{Id}")]
        public async Task<IActionResult> ConfirmEvent(object Id)
        {
            GenericResponse<int> response = new GenericResponse<int>();


            try
            {
                if ((int)Id > 0)
                {
                    var result = await _service.ConfirmAppointment(Id);

                    if (result > 0)
                    {
                        response.Status = Helper.successCode;
                        response.message = Helper.AppointmentDeleted;
                        response.dataenum = result;
                    }
                    else if (result == 0)
                    {
                        response.Status = Helper.faliercode;
                        response.message = Helper.AppointmentDeafultErrors;
                        response.dataenum = result;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Status = Helper.faliercode;
                response.message = ex.Message;
                response.dataenum = 0;
            }

            return Ok(response);
        }
    }
}