using Audiophiles_API.DTOs.Auth;
using Audiophiles_API.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Audiophiles_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AudioAuthController : ControllerBase
    {
        private readonly IAudioAuthService _audioAuthService;

        public AudioAuthController(IAudioAuthService audioAuthService)
        {
            _audioAuthService = audioAuthService;
        }

        [HttpPost("register", Name = "register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel model)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _audioAuthService.RegisterAsync(model);

            if(!string.IsNullOrEmpty(result.Message)) 
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("login", Name = "login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _audioAuthService.LoginAsync(model);

            if (!string.IsNullOrEmpty(result.Message))
                return BadRequest(result);

            return Ok(result);
        }
    }
}
