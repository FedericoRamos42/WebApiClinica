using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Response
{
    public class AssignAppointmentResponse
    {
        public int IdAppointment { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public string PatientName { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public Domain.Enums.AppointmentStatus Status { get; set; }

        public static AssignAppointmentResponse CreateDto(Appointment appointment)
        {
            var newAppointmentDto = new AssignAppointmentResponse()
            {
                IdAppointment = appointment.Id,
                DoctorName = appointment.Doctor.Name,
                PatientName = appointment.Patient.Name,
                Date = appointment.Date,
                Time = appointment.Time,
                Status = appointment.Status
            };
            return newAppointmentDto;
        }

        public static IEnumerable<AssignAppointmentResponse> CreateList(IEnumerable<Appointment> list)
        {
            List<AssignAppointmentResponse> listDto = new List<AssignAppointmentResponse>();
            foreach (Appointment a in list)
            {
                listDto.Add(CreateDto(a));
            }
            return listDto;
        }
    }
}
