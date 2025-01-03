﻿using Application.Interfaces;
using Application.Models;
using Application.Models.Request;
using Application.Models.Response;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.InterFaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IPatientRepository _patientRepository;

        public AppointmentService(IAppointmentRepository appointmentRepository, IDoctorRepository doctorRepository, IPatientRepository patientRepository)
        {
            _appointmentRepository = appointmentRepository;
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
        }

        public AppointmentDto GetById(int id)
        {
            var appointment = _appointmentRepository.GetAppointmentByWithPatientAndDoctor(id);

            return appointment == null
                ? throw new NotFoundException($"No se encontró la cita con el id {id}")
                : AppointmentDto.CreateDto(appointment);
        }

        public IEnumerable<ResponseDoctorID> GetAllByDoctorId(int id)
        {
            var doctor = _doctorRepository.GetById(id);
            if (doctor == null)
            {
                throw new NotFoundException($"No existe doctor con el id {id}");
            }
            var listAppointments = _appointmentRepository.GetAppointmentByDoctorId(id);
            return ResponseDoctorID.CreateList(listAppointments);
        }

        public IEnumerable<ResponseAppointmentGetAll> GetAllByPatientId(int id)
        {
            var patient = _patientRepository.GetByIdIncludeAddress(id);
            if(patient == null)
            {
                throw new NotFoundException($"No existe paciente con el id {id}");
            }
            var listAppointments = _appointmentRepository.GetAppointmentByPatientId(id);
            return ResponseAppointmentGetAll.CreateList(listAppointments);
        }
        public IEnumerable<ResponseAppointmentGetAll> GetAllAppointment()
        {
            var appointment = _appointmentRepository.GetAllAppointment();
            return ResponseAppointmentGetAll.CreateList(appointment);
        }
        public IEnumerable<AppointmentDto> GetAppointmentsAvailable(int id)
        {
            var entity = _doctorRepository.GetById(id);

            if (entity ==null)
            {
                throw new NotFoundException($"No se encontró doctor con el id indicado {id}");
            }
            var listAppointments = _appointmentRepository.GetByAvailable(id);
            return AppointmentDto.CreateList(listAppointments);

        }

     
        public void GenerateAppointmentForDoctor(int doctorId, DateRangeRequest Date)
        {
            var doctor = _doctorRepository.GetById(doctorId);

            if (doctor == null)
            {
                throw new NotFoundException("Doctor no encontrado.");
            }

            var appointmentsDb = _appointmentRepository.GetAppointmentByDoctorId(doctorId);

            for (var date = Date.StartDate; date <= Date.EndDate; date = date.AddDays(1))
            {
                if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                {
                    continue;
                }


                for (var time = new TimeSpan(9, 0, 0); time < new TimeSpan(12, 0, 0); time = time.Add(new TimeSpan(1, 0, 0)))
                {
                    var appointment = new AppointmentCreateRequest
                    {
                        DoctorId = doctorId,
                        Date = date.Date,
                        Time = time.ToString(@"hh\:mm\:ss"),
                        Status = AppointmentStatus.Available,
                        PatientId = null
                    };

                    CreateAppointment(appointment);
                }


                for (var time = new TimeSpan(14, 0, 0); time < new TimeSpan(18, 0, 0); time = time.Add(new TimeSpan(1, 0, 0)))
                {
                    var appointment = new AppointmentCreateRequest
                    {
                        DoctorId = doctorId,
                        Date = date.Date,
                        Time = time.ToString(@"hh\:mm\:ss"),
                        Status = AppointmentStatus.Available,
                        PatientId = null
                    };

                    CreateAppointment(appointment);
                }
            }
        }

        public AppointmentDto CreateAppointment(AppointmentCreateRequest appointment)
        {
            if (!TimeSpan.TryParse(appointment.Time, out var appointmentTime)) 
            {
                throw new NotFoundException("Hora inválida. Ingrese la hora en el formato HH:mm.");
            }
            
            var appointmentsDb = _appointmentRepository.GetAppointmentByDoctorId(appointment.DoctorId);
            if (appointment.PatientId is not null) {
                var patient = _patientRepository.GetByIdIncludeAddress(appointment.PatientId.Value);
                if (patient == null)
                {
                    throw new NotFoundException("No existe paciente con el ID indicado");
                } 
            }

            DateTime appointmentDateTime = appointment.Date.Date + TimeSpan.Parse(appointment.Time);

            DateTime currentDateTime = DateTime.Now;
            
            if (appointmentDateTime < currentDateTime)
            {
                throw new NotFoundException("No se pueden crear turnos en una fecha y hora pasada.");
            }

            if (!appointmentsDb.Any(a => (a.Date.Date + a.Time) == appointmentDateTime))
            {
                var newAppointment = new Appointment()
                {
                    DoctorId = appointment.DoctorId,
                    PatientId = appointment.PatientId,
                    Time = TimeSpan.Parse(appointment.Time),
                    Date = appointment.Date,
                    Status = AppointmentStatus.Available,
                };

                _appointmentRepository.Create(newAppointment);
                return AppointmentDto.CreateDto(newAppointment);
            }

            throw new NotFoundException("Ya existe un turno en el horario ingresado.");
            }

      public AppointmentDto CancelAppointment(int IdAppointment)
      {
            var entity = _appointmentRepository.GetById(IdAppointment) ?? throw new NotFoundException("Cita no encontrada.");

            entity.Status = AppointmentStatus.Available;
            entity.PatientId = null;

            _appointmentRepository.Update(entity);

            return AppointmentDto.CreateDto(entity);
        }

        public AppointmentDto AssignAppointment(AppointmentAssignForRequest appointmentAssign)
        {
            var entity = _appointmentRepository.GetById(appointmentAssign.IdAppointment);

            if (entity == null)
            {
                throw new NotFoundException("Cita no encontrada.");
            }

            var patient = _patientRepository.GetByIdIncludeAddress(appointmentAssign.IdPatient);

            if (patient == null)
            {
                throw new NotFoundException("Paciente no encontrado.");
            }

            if (entity.Status != AppointmentStatus.Available)
            {
                throw new NotFoundException("No esta disponible.");
            }


            var currentDateTime = DateTime.Now;
            var appointmentDateTime = entity.Date + entity.Time;

            if ((appointmentDateTime - currentDateTime).TotalMinutes <= 30)
            {
                throw new NotFoundException("No se puede asignar turnos con menos de 30 minutos de anticipación.");
            }

            entity.PatientId = appointmentAssign.IdPatient;

            entity.Status = AppointmentStatus.Reserved;

            _appointmentRepository.Update(entity);

            return AppointmentDto.CreateDto(entity);
        }

        public AppointmentDto DeleteAppointment(int IdAppointment)
        {
            var appointment = _appointmentRepository.GetById(IdAppointment);

            var entity = _appointmentRepository.Delete(appointment);

            return AppointmentDto.CreateDto(entity);
        }
        public IEnumerable<ResponseAppointmentGetAll> FilteredAppointment(DateTime? date, int? specialty)
        {
            var list = _appointmentRepository.GetFilteredAppointments(date, specialty);
            return ResponseAppointmentGetAll.CreateList(list);
        }
    }
}
