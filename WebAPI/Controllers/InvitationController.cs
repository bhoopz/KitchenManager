using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvitationController : ControllerBase
    {
        private readonly IInvitationService _invitationService;

        public InvitationController(IInvitationService invitationService)
        {
            _invitationService = invitationService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ManageInvitation(InvitationDecisionDto decisionDto)
        {
            var result = await _invitationService.HandleInvitationDecision(decisionDto);

            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> InvitationsList(InvitationDecisionDto decisionDto)
        {
            var result = await _invitationService.GetInvitationsList();

            return result.Succeeded ? Ok(result) : BadRequest(result);
        }
    }

    
}
