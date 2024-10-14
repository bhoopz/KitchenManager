using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;
        private readonly IInvitationService _invitationService;
        public RestaurantController(IRestaurantService restaurantService, IInvitationService invitationService)
        {
            _restaurantService = restaurantService;
            _invitationService = invitationService;
        }

        [HttpPost, Route("create")]
        [Authorize]
        public async Task<IActionResult> CreateRestaurant(CreateRestaurantDto restaurantData)
        {
            var result = await _restaurantService.CreateRestaurant(restaurantData);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost("{restaurantId}/invite")]
        [Authorize]
        public async Task<IActionResult> InviteUser(InviteUserDto inviteUserDto, Guid restaurantId)
        {
            var result = await _invitationService.InviteUserToRestaurant(inviteUserDto.Email, restaurantId);
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
