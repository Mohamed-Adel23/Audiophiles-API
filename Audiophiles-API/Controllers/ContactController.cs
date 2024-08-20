using Audiophiles_API.Constants;
using Audiophiles_API.DTOs;
using Audiophiles_API.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Audiophiles_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpPost("user", Name = "contactUser")]
        public async Task<IActionResult> UserContact([FromBody] UserContactModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _contactService.UserContactAsync(model);

            if(!result.IsSucceeded)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = $"{RolesConstants.Admin}")]
        [HttpPost("admin", Name = "adminRespond")]
        public async Task<IActionResult> AdminRespond([FromBody] AdminRespondModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _contactService.AdminRespondAsync(model);

            if (!result.IsSucceeded)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = $"{RolesConstants.Admin}")]
        [HttpGet("cont/{id:int}", Name = "getContact")]
        public IActionResult GetContact(int id)
        {
            var result = _contactService.GetContactAsync(id);

            if(result is null)
                return NotFound(new { Message = "Something went wrong!" });

            return Ok(result);
        }

        [Authorize(Roles = $"{RolesConstants.Admin}")]
        [HttpGet("all", Name = "allContacts")]
        public IActionResult AllContacts()
        {
            var result = _contactService.GetAllContactsAsync();

            if (result.Count() <= 0)
                return NotFound(new { Message = "Something went wrong!" });

            return Ok(result);
        }
    }
}
