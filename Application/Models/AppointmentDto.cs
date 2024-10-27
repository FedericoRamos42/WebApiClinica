﻿using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class AppointmentDto
    {
        public int IdAppointment { get; set; }
        public string DoctorName{ get; set; } = string.Empty;
        public string DoctorSpecialty { get; set; } = string.Empty;
        public int? PatientId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public AppointmentStatus Status { get; set; }

        public static AppointmentDto CreateDto(Appointment appointment)
        {
            var newAppointmentDto = new AppointmentDto()
            {
                IdAppointment = appointment.Id,
                DoctorName = appointment.Doctor.Name,
                DoctorSpecialty = appointment.Doctor.Specialty.Name,
                PatientId = appointment.PatientId,
                Date = appointment.Date,
                Time = appointment.Time,
                Status = appointment.Status
            };
            return newAppointmentDto;
        }

        public static IEnumerable<AppointmentDto> CreateList(IEnumerable<Appointment> list)
        {
            List<AppointmentDto> listDto = new List<AppointmentDto>();
            foreach (Appointment a in list)
            {
                listDto.Add(CreateDto(a));
            }
            return listDto;
        }
    }
}
