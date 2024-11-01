using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Response
{
    public class ResponseDoctorID
    {
            public int Id { get; set; } 
            public string PatientName { get; set; } = string.Empty;
            public DateTime Date { get; set; }
            public TimeSpan Time { get; set; }
            public AppointmentStatus Status { get; set; }

            public static ResponseDoctorID CreateDto(Appointment appointment)
            {
                var newAppointmentDto = new ResponseDoctorID()
                {
                    Id = appointment.Id,
                    PatientName = $"{appointment.Patient.Name} {appointment.Patient.LastName}",
                    Date = appointment.Date,
                    Time = appointment.Time,
                    Status = appointment.Status
                };
                return newAppointmentDto;
            }

            public static IEnumerable<ResponseDoctorID> CreateList(IEnumerable<Appointment> list)
            {
                List<ResponseDoctorID> listDto = new List<ResponseDoctorID>();
                foreach (Appointment a in list)
                {
                    listDto.Add(CreateDto(a));
                }
                return listDto;
            }
    }
}

