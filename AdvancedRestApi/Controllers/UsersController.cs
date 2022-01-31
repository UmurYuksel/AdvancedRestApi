using AdvancedRestApi.DTO_s;
using AdvancedRestApi.Interfaces;
using AdvancedRestApi.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdvancedRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private IUser _userService;

        public UsersController(IUser userService)
        {
            _userService = userService;
        }

        // GET: api/<UsersController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
          var result = await _userService.GetAllUser();
            if (result.IsSuccess)
            {
                return Ok(result.Item2);
            }
            return NotFound(result.ErrorMessage);
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _userService.GetUserById(id);
            if (result.IsSuccess)
            {
                return Ok(result.Item2);
            }
            return NotFound(result.ErrorMessage);
        }

        // POST api/<UsersController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserDTO user)
        {
        //I used some deconstruction thing here.
          var (IsSuccess, ErrorMessage) =  await _userService.AddUser(user);
            if (IsSuccess)
            {
                return StatusCode(StatusCodes.Status201Created);
            }
            return BadRequest(ErrorMessage);
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UserDTO user)
        {
            var (IsSuccess, ErrorMessage) = await _userService.UpdateUser(id, user);
            if (IsSuccess)
            {
                return NoContent();
            }
            return BadRequest(ErrorMessage);

        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result =await _userService.DeleteUser(id);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return BadRequest(result.ErrorMessage);
        }
    }
}
