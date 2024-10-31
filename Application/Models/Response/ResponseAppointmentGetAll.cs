using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Response
{
    public class ResponseAppointmentGetAll
    {
        public int IdAppointment { get; set; }
        public string DoctorName{ get; set; } = string.Empty;
        public string DoctorSpecialty { get; set; } = string.Empty;
        public int? PatientId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public Domain.Enums.AppointmentStatus Status { get; set; }

        public static ResponseAppointmentGetAll CreateDto(Appointment appointment)
        {
            var newAppointmentDto = new ResponseAppointmentGetAll()
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

        public static IEnumerable<ResponseAppointmentGetAll> CreateList(IEnumerable<Appointment> list)
        {
            List<ResponseAppointmentGetAll> listDto = new List<ResponseAppointmentGetAll>();
            foreach (Appointment a in list)
            {
                listDto.Add(CreateDto(a));
            }
            return listDto;
        }
    }
}
