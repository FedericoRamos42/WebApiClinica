﻿using Application.Models;
using Domain.Entities;
using Domain.Enums;
using Domain.InterFaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Infrastructure.Data
{
    public class AppointmentRepository : BaseRepository<Appointment>, IAppointmentRepository
    {
        private readonly ApplicationContext _repository;

        public AppointmentRepository(ApplicationContext repository) : base(repository)
        {
            _repository = repository;
        }

        public Appointment? GetAppointmentByWithPatientAndDoctor(int id)
        {
            var appointment = _repository.Appointments
                                        .Include(c => c.Patient)
                                        .Include(c => c.Doctor)
                                        .FirstOrDefault(c => c.Id == id);
            return appointment;
        }

        public IEnumerable<Appointment> GetAppointmentByPatientId(int patientId)
        {
            var appointments = _repository.Appointments
                                        .Where(a => a.PatientId == patientId && a.Status == AppointmentStatus.Reserved)
                                        .Include(c => c.Doctor)
                                        .ThenInclude(d => d.Specialty)
                                        .ToList();

            return appointments;
        }

        public IEnumerable<Appointment> GetAppointmentByDoctorId(int doctorId)
        {
            var appointments = _repository.Appointments
                                        .Where(a => a.DoctorId == doctorId && a.Status == AppointmentStatus.Reserved)
                                        .Include(a => a.Patient)
                                        .Include(d=>d.Doctor)
                                        .ToList();
            return appointments;
        }

       

        public IEnumerable<Appointment> GetByAvailable(int id)
        {

            DateTime today = DateTime.Today; 

            return _repository.Appointments
                              .Where(a => a.DoctorId == id
                                       && a.Status == AppointmentStatus.Available
                                       && a.Date >= today)
                              .ToList();
        }
        public IEnumerable<Appointment> GetFilteredAppointments(DateTime? date, int? specialty)
        {
            DateTime today = DateTime.Today;

            var query = _repository.Appointments
                        .Where(a => a.Status == AppointmentStatus.Available && a.Date > today)
                        .Include(a => a.Doctor)
                        .ThenInclude(d => d.Specialty)
                        .AsQueryable();

            
            if (date.HasValue)
            {
                query = query.Where(a => a.Date.Date == date.Value.Date);
            }

            
            if (specialty.HasValue)
            {
                query = query.Where(a => a.Doctor.SpecialtyId == specialty.Value);
            }

            return query.ToList();
        }
        public IEnumerable<Appointment> GetAllAppointment() 
        {
            var appointments = _repository.Appointments
                                        .Where(a => a.Status == AppointmentStatus.Available && a.Date >= DateTime.Today)
                                        .Include(a => a.Doctor)
                                        .ThenInclude(d => d.Specialty);
            return appointments;
        }
    }
}
