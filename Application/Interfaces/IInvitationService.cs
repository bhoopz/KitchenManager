using Application.DTOs;
using Application.Responses;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IInvitationService
    {

        Task<ApiResponse<object>> InviteUserToRestaurant(string email, Guid restaurantId);
        Task<ApiResponse<object>> HandleInvitationDecision(InvitationDecisionDto decisionDto);
        Task<ApiResponse<IEnumerable<Invitation>>> GetInvitationsList();
    }
}
