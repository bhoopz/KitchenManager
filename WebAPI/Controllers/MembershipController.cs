using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembershipController : ControllerBase
    {
        private readonly IMembershipService _memebershipService;
        public MembershipController(IMembershipService membershipService)
        {
            _memebershipService = membershipService;
        }

        [HttpGet, Route("my")]
        [Authorize]
        public async Task<IActionResult> MyMemberships()
        {
            var result = await _memebershipService.GetUserMemberships();
            if (result.Succeeded)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

    }
}
