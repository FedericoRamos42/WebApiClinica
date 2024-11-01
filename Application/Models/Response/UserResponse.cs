using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Response
{
    public class UserResponse
    {
        public int Id {  get; set; }
        public string Name { get; set; } = string.Empty;
        public string LastName {  get; set; } = string.Empty;
        public string PhoneNumber {  get; set; } = string.Empty;    
        public string Email { get; set; } = string.Empty;
        public string Role {  get; set; } = string.Empty;
        public static UserResponse CreateDoctorDto(User user)
        {
            var doctorDto = new UserResponse()
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Role = user.UserRole.ToString(),
                Email = user.Email,
            };
            return doctorDto;
        }

        public static IEnumerable<UserResponse> CreatelistDto(IEnumerable<User> users)
        {
            List<UserResponse> listDto = new List<UserResponse>();
            foreach (var user in users)
            {
                listDto.Add(CreateDoctorDto(user));
            }
            return listDto;
        }

    }
}
