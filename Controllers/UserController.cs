using hanap_buhay_server.Entities;
using hanap_buhay_server.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace hanap_buhay_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Add(User user)
        {
            try
            {
                var data = await _service.Create(user);
                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(User user)
        {
            try
            {
                var data = await _service.Update(user);
                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [Route("/{userId}")]
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid userid)
        {
            try
            {
                await _service.Delete(userid);
                return Ok(userid);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("/{userId}")]
        [HttpGet]
        public async Task<IActionResult> Get(Guid userid)
        {
            try
            {
                var data = await _service.Read(userid);
                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var data = await _service.ReadAll();
                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
