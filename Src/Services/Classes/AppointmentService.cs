using System;
using Src.Services.Interfaces;
using Src.Models.ViewModels.Doctor;
using Src.Models.ViewModels.Patient;
using System.Collections.Generic;
using Src.Models;
using System.Linq;
using Src.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Src.Models.ViewModels.Appointment;
using Src.Models.Entities;

namespace Src.Services.Classes
{
    public class AppointmentService : IAppointmentService
    {

        private readonly AppDbContext _context;

        public AppointmentService(AppDbContext context)
        {

            _context = context;
        }

        public async Task<int> AddUpdate(AppointmentVm model)
        {
            var startdate = DateTime.Parse(model.StartDate);
            var enddate = DateTime.Parse(model.StartDate).AddMinutes(Convert.ToDouble(model.Duration));

            if (model != null && model.Id > 0)
            {

                return 2;
            }
            else
            {
                Appointment appointment = new Appointment
                {
                    Title = model.Title,
                    Description = model.Description,
                    EndDate = enddate,
                    StartDate = startdate,
                    Admin_Id = model.Admin_Id,
                    Doctor_Id = model.Doctor_Id,
                    DoctorApproved = false,
                    Patient_Id = model.Patient_Id,
                    Duration = model.Duration,
                };


                _context.Appointments.Add(appointment);
                var result = await _context.SaveChangesAsync();

                if (result > 0) /// Should resturn 1 all the time  the 1 is for adding corrcotly
                    return result;
                else
                    return -2; /// if saveing pross go into problem its return -2
            }
        }

        public async Task<IEnumerable<DoctorVm>> GetDoctorList()
        {

            var roleId = await _context.Roles.SingleOrDefaultAsync(c => c.Name == Helper.Doctor);

            var users = _context.Users.Join(_context.UserRoles, user => user.Id, userrole => userrole.UserId, (user, userrole) => new
            {
                user.Name,
                user.Id,
                userrole.RoleId
            }).Where(c => c.RoleId == roleId.Id).Select(dvm => new DoctorVm { Id = dvm.Id, Name = dvm.Name }).AsQueryable();


            return users;

        }

        public async Task<IEnumerable<PatientVm>> GetPatientList()
        {
            var roleId = await _context.Roles.SingleOrDefaultAsync(c => c.Name == Helper.Patient);

            var users = _context.Users.Join(_context.UserRoles, user => user.Id, userrole => userrole.UserId, (user, userrole) => new
            {
                user.Name,
                user.Id,
                userrole.RoleId
            }).Where(c => c.RoleId == roleId.Id).Select(dvm => new PatientVm { Id = dvm.Id, Name = dvm.Name }).AsQueryable();

            return users;
        }

        public async Task<List<AppointmentVm>> DoctorEventsById(string doctorId)
        {
            return await _context.Appointments.Where(c => c.Doctor_Id == doctorId).Select(i => new AppointmentVm
            {
                Description = i.Description,
                DoctorApproved = i.DoctorApproved,
                Admin_Id = i.Admin_Id,
                StartDate = i.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = i.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Duration = i.Duration,
                Title = i.Title,
                Id = i.Id
            }).ToListAsync();
        }


        public async Task<List<AppointmentVm>> PatientsEventById(string patientId)
        {
            return await _context.Appointments.Where(c => c.Patient_Id == patientId).Select(i => new AppointmentVm
            {
                Description = i.Description,
                DoctorApproved = i.DoctorApproved,
                Admin_Id = i.Admin_Id,
                StartDate = i.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = i.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Duration = i.Duration,
                Title = i.Title,
                Id = i.Id
            }).ToListAsync();
        }

        public AppointmentVm GetById(int Id)
        {
            return _context.Appointments.Where(c => c.Id == Id).Select(i => new AppointmentVm
            {
                Description = i.Description,
                DoctorApproved = i.DoctorApproved,
                Admin_Id = i.Admin_Id,
                StartDate = i.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = i.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Duration = i.Duration,
                Title = i.Title,
                Id = i.Id,
                Patient_Id = i.Patient_Id,
                Doctor_Id = i.Doctor_Id,
                PatientName = _context.Users.Where(x => x.Id == i.Patient_Id).Select(c => c.Name).FirstOrDefault(),
                DoctorName = _context.Users.Where(x => x.Id == i.Doctor_Id).Select(c => c.Name).FirstOrDefault()
            }).SingleOrDefault();
        }

        public async Task<int> ConfirmAppointment(object Id)
        {
            var appointment = await _context.Appointments.SingleOrDefaultAsync(x => x.Id == (int)Id);

            appointment.DoctorApproved = true;

            _context.Appointments.Update(appointment);
            var result = await _context.SaveChangesAsync();

            return result;
        }

        public int DeleteAppointment(object Id)
        {
            var appointment = _context.Appointments.SingleOrDefault(x => x.Id == (int)Id);

            _context.Appointments.Remove(appointment);
            var result = _context.SaveChanges();

            return result;

        }
    }
}