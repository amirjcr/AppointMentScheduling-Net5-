using System;

namespace Src.Models.ViewModels.Appointment
{
    public class AppointmentVm
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Doctor_Id { get; set; }
        public string Patient_Id { get; set; }
        public bool DoctorApproved { get; set; }
        public string Admin_Id { get; set; }
        public string Duration { get; set; }


        public string DoctorName { get; set; }
        public string PatientName { get; set; }
        public string AdminName { get; set; }
        public bool Isforclient { get; set; }
    }
}