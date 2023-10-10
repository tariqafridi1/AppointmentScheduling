using AppointmentScheduling.Models;
using AppointmentScheduling.Models.ViewModels;
using AppointmentScheduling.Utility;
using Humanizer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace AppointmentScheduling.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;

        public AppointmentService(ApplicationDbContext context,UserManager<ApplicationUser> _userManager)
        {
            _context = context;
            userManager = _userManager;
             
        }

        public async Task<int> AddUpdate(AppointmentVM model)
        {
            var startDate = DateTime.Parse(model.StartDate);
            var endDate = DateTime.Parse(model.StartDate).AddMinutes(Convert.ToDouble(model.Duration));
            if (model != null && model.Id > 0)
            {
                //Updation
                return 1;
            }
            else
            {
                if (model != null)
                {
                    //Insertion
                    Appointment appointment = new Appointment()
                    {
                        Title = model.Title,
                        StartDate = startDate,
                        EndDate = endDate,
                        Description = model.Description,
                        Duration = model.Duration,
                        DoctorId = model.DoctorId,
                        PatientId = model.PatientId,
                        IsDoctorApproved = false
                    };
                     _context.Appointments.Add(appointment);
                    await _context.SaveChangesAsync();
                }
                return 2;
            }

        }

        public async Task<int> ConfirmEvent(int id)
        {
            var appointment = _context.Appointments.FirstOrDefault(x => x.Id == id);
            if (appointment != null)
            {
                appointment.IsDoctorApproved = true;
                return await _context.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<int> Delete(int id)
        {
            var appointment = _context.Appointments.FirstOrDefault(x => x.Id == id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
                return await _context.SaveChangesAsync();
            }
            return 0;
        }

        public List<AppointmentVM> DoctorEventsById(string doctorId)
        {
           
                return _context.Appointments.Where(x => x.DoctorId == doctorId).ToList().Select(c => new AppointmentVM()
                {
                    Id = c.Id,
                    Description = c.Description,
                    StartDate = c.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    EndDate = c.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    Title = c.Title,
                    Duration = c.Duration,
                    IsDoctorApproved = c.IsDoctorApproved
                }).ToList();
            
        }

        public async Task<AppointmentVM> GetById(int id)
        {
            var doctors = await userManager.GetUsersInRoleAsync(Utility.Helper.Doctor);
            var patients = await userManager.GetUsersInRoleAsync(Utility.Helper.Patient);
            var data = _context.Appointments.Where(x => x.Id == id).ToList().Select(c => new AppointmentVM()
            {
                Id = c.Id,
                Description = c.Description,
                StartDate = c.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = c.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Title = c.Title,
                Duration = c.Duration,
                IsDoctorApproved = c.IsDoctorApproved,
                PatientId = c.PatientId,
                DoctorId = c.DoctorId,
                PatientName = patients.Where(x => x.Id == c.PatientId).Select(x => x.Name).FirstOrDefault(),
                DoctorName = doctors.Where(x => x.Id == c.DoctorId).Select(x => x.Name).FirstOrDefault()
            }).SingleOrDefault();
            return data;
        }

        public async Task<List<DoctorVM>> GetDoctorlist()
        {
            var doctorUsers = await userManager.GetUsersInRoleAsync(Helper.Doctor);

            var doctors = doctorUsers
                .Select(user => new DoctorVM
                {
                    Id = user.Id,
                    Name = user.Name,
                });
           
            return doctors.ToList();
        }

        public async Task<List<PatientVM>> GetPatientlist()
        {
            var patientUsers = await userManager.GetUsersInRoleAsync(Helper.Patient);
            var patients = patientUsers
                .Select(user => new PatientVM
                {
                    Id = user.Id,
                    Name = user.Name,
                })
                .ToList();
            return patients;
        }

        public List<AppointmentVM> PatientEventsById(string patientId)
        {
            return _context.Appointments.Where(x => x.PatientId == patientId).ToList()
                               .Select(c => new AppointmentVM()
                               {
                                   Id = c.Id,
                                   Description = c.Description,
                                   StartDate = c.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                   EndDate = c.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                   Title = c.Title,
                                   Duration = c.Duration,
                                   IsDoctorApproved = c.IsDoctorApproved
                               }).ToList();
        }


        //public async Task<List<DoctorVM>> GetDoctorList()
        //{

        //    var doctorUsers = await userManager.GetUsersInRoleAsync(Helper.Doctor);

        //    var doctors = doctorUsers
        //        .Select(user => new DoctorVM
        //        {
        //            Id = user.Id,
        //            Name = user.Name,
        //        })
        //    .ToList();
        //    return doctors;

        //}

        //public async Task <List<PatientVM>> GetPatientList()
        //{
        //    var patientUsers = await userManager.GetUsersInRoleAsync(Helper.Patient);
        //    var patients = patientUsers
        //        .Select(user => new PatientVM
        //        {
        //            Id = user.Id,
        //            Name = user.Name,
        //        })
        //        .ToList();
        //    return patients;
        //}
    }
    }
