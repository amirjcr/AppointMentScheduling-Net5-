using System;


namespace Src.Models.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Duration{get;set;}
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Doctor_Id { get; set; }
        public string Patient_Id { get; set; }
        public bool DoctorApproved { get; set; }
        public string Admin_Id { get; set; }

    }
}