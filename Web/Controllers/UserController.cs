using Application.Interfaces;
using Domain.InterFaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userRepository;

        public UserController(IUserService user)
        {
            _userRepository = user;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var list = _userRepository.GetAll();
            return Ok(list);
        }
    }
}
