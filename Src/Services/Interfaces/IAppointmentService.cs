using System;
using System.Collections.Generic;
using Src.Models.ViewModels.Doctor;
using Src.Models.ViewModels.Patient;
using System.Threading.Tasks;
using Src.Models.ViewModels.Appointment;

namespace Src.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<IEnumerable<DoctorVm>> GetDoctorList();
        Task<IEnumerable<PatientVm>> GetPatientList();
        Task<int> AddUpdate(AppointmentVm model);

        Task<List<AppointmentVm>> PatientsEventById(string patientId);
        Task<List<AppointmentVm>> DoctorEventsById(string doctorId);

        AppointmentVm GetById(int Id);

        Task<int> ConfirmAppointment(object Id);

        int DeleteAppointment(object Id);
    }
}