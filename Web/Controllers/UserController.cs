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
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository user)
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
