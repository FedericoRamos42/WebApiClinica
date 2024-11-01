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

        [HttpGet("{id}")]
        public IActionResult GetbyId(int id)
        {
            var ent = _userRepository.GetById(id);
            return Ok(ent);
        }
        [HttpDelete("Delete/{id}")]
        public IActionResult DeleteLogic(int id) 
        {
            var response = _userRepository.Delete(id);
            return Ok(response);
        }
        [HttpGet(("Filtered"))]
        public IActionResult GetByStatus([FromQuery] bool? state)
        {
            var list = _userRepository.GetByState(state);
                return Ok(list);
        }
    }
}
