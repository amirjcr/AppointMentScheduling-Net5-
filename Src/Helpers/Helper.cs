using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Src.Helpers
{
    public static class Helper
    {
        public const string Admin = "Admin";
        public const string Patient = "Patient";
        public const string Doctor = "Doctor";

        //Answers-------------

        public const string AppointmentAdded = "Appointment Added Successfuly";
        public const string AppointmentUpdated = "Appointment Updated Successfuly";
        public const string AppointmentDeleted = "Appointment Deleted Successfuly";
        public const string AppointmentExisited = "Appointment Is Exisiting";
        public const string AppointmentNotExisited = "Appointment Not Exisits";
        public const string AppointmentAddError = "someting going wrroing on adding appointment please try later";
        public const string AppointmentUpdateError = "someting going wrroing on updating appointment please try later";
        public const string AppointmentDeafultErrors = "the system dosnt pross your request its can be cause of wrong pross";


        /// int const variables
        public const int successCode = 650;
        public const int faliercode = -650;

        public static List<SelectListItem> GetrolesForDropDown()
        => new List<SelectListItem>{
            new SelectListItem{Value = Helper.Admin , Text = Helper.Admin},
            new SelectListItem{Value =Helper.Doctor, Text = Helper.Doctor},
            new SelectListItem{Value = Helper.Patient, Text = Helper.Patient}
        };



        public static List<SelectListItem> GetTimeDropDown()
        {
            int minute = 0;
            List<SelectListItem> duration = new List<SelectListItem>();

            for (int i = 1; i <= 12; i++)
            {
                duration.Add(new SelectListItem { Value = i.ToString() + minute, Text = i + (minute > 0 ? " : " + minute.ToString() : string.Empty) + " Hr" });
                minute += 30;
                duration.Add(new SelectListItem { Value = i.ToString() + minute, Text = i + (minute > 0 ? " : " + minute.ToString() : string.Empty) + " Hr" });
                minute += 60;

                if (minute >= 60)
                    minute = 0;
            }


            return duration;
        }
    }

}