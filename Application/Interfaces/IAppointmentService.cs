﻿using Application.Models;
using Application.Models.Request;
using Application.Models.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAppointmentService
    {
        AppointmentDto GetById(int id);
        IEnumerable<ResponseDoctorID> GetAllByDoctorId(int id);
        IEnumerable<ResponseAppointmentGetAll> GetAllByPatientId(int id);
        IEnumerable<AppointmentDto> GetAppointmentsAvailable(int id);
        void GenerateAppointmentForDoctor(int doctorId, DateRangeRequest Date);
        AppointmentDto CreateAppointment(AppointmentCreateRequest appointment);
        AppointmentDto CancelAppointment(int IdAppointment);
        AppointmentDto AssignAppointment(AppointmentAssignForRequest appointmentAssign);
        AppointmentDto DeleteAppointment(int IdAppointment);
        IEnumerable<ResponseAppointmentGetAll> GetAllAppointment();
        IEnumerable<ResponseAppointmentGetAll> FilteredAppointment(DateTime? date, int? specialty);
    }
}
