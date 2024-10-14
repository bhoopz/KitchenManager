using Application.DTOs;
using Application.Helpers;
using Application.Interfaces;
using Application.Responses;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class InvitationService : IInvitationService
    {
        private readonly IUserContextService _userContextService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public InvitationService(IUserContextService userContextService, UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {
            _userContextService = userContextService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<object>> InviteUserToRestaurant(string email, Guid restaurantId)
        {
            var invitingUserId = _userContextService.GetUserId();
            if(invitingUserId == Guid.Empty) return ApiResponse<object>.Failure(ErrorHelper.Unauthorized);

            var memberships = await _unitOfWork.GetGenericRepository<Membership>().FindAllAsync(m => m.UserId == invitingUserId && m.RestaurantId == restaurantId);
            var ownerMembership = memberships.FirstOrDefault(m => m.Role == UserRole.Owner);
            if (ownerMembership == null) return ApiResponse<object>.Failure(ErrorHelper.Forbidden);

            var invitedUser = await _userManager.FindByEmailAsync(email);
            if (invitedUser == null || invitingUserId == invitedUser.Id) return ApiResponse<object>.Failure(ErrorHelper.UserNotFound);

            var isMember = await _unitOfWork.GetGenericRepository<Membership>().FindAllAsync(m => m.UserId == invitedUser.Id && m.RestaurantId == restaurantId && m.EndDate == null);
            if (isMember.Any()) return ApiResponse<object>.Failure(ErrorHelper.AlreadyMember);

            var isInvited = await _unitOfWork.GetGenericRepository<Invitation>().FindAllAsync(i => i.InvitedUserId == invitedUser.Id && i.RestaurantId == restaurantId);
            if (isInvited.Any()) return ApiResponse<object>.Failure(ErrorHelper.AlreadyInvited);

            var restaurant = await _unitOfWork.GetGenericRepository<Restaurant>().GetByIdAsync(restaurantId);
            if (restaurant == null) return ApiResponse<object>.Failure(ErrorHelper.RestaurantNotFound);

            var invitation = new Invitation
            {
                RestaurantId = restaurantId,
                InvitedUserId = invitedUser.Id,
                InvitingUserId = invitingUserId,
            };

            try
            {
                await _unitOfWork.GetGenericRepository<Invitation>().AddAsync(invitation);
                await _unitOfWork.CompleteAsync();

                return ApiResponse<object>.Success();
            }
            catch (Exception)
            {
                return ApiResponse<object>.Failure(ErrorHelper.Sww);
            }
        }

        public async Task<ApiResponse<object>> HandleInvitationDecision(InvitationDecisionDto decisionDto)
        {
            var userId = _userContextService.GetUserId();
            if (userId == Guid.Empty) return ApiResponse<object>.Failure(ErrorHelper.Unauthorized);

            var invitation = await _unitOfWork.GetGenericRepository<Invitation>().GetByIdAsync(decisionDto.InvitationId);
            if (invitation == null || invitation.InvitedUserId != userId) return ApiResponse<object>.Failure(ErrorHelper.InvitationNotFound);

            try
            {
                if (decisionDto.Accept)
                {
                    var membership = new Membership
                    {
                        UserId = invitation.InvitedUserId,
                        RestaurantId = invitation.RestaurantId,
                        Role = UserRole.Employee,
                        StartDate = DateTime.UtcNow
                    };

                    await _unitOfWork.GetGenericRepository<Membership>().AddAsync(membership);
                }

                _unitOfWork.GetGenericRepository<Invitation>().Delete(invitation);

                await _unitOfWork.CompleteAsync();

                return ApiResponse<object>.Success();
            }
            catch (Exception)
            {
                return ApiResponse<object>.Failure(ErrorHelper.Sww);
            }
        }

        public async Task<ApiResponse<IEnumerable<Invitation>>> GetInvitationsList()
        {
            Guid userId = _userContextService.GetUserId();
            if (userId == Guid.Empty) return ApiResponse<IEnumerable<Invitation>>.Failure(ErrorHelper.Unauthorized);

            var invitations = await _unitOfWork.GetGenericRepository<Invitation>().FindAllAsync(i => i.InvitedUserId == userId);

            return ApiResponse<IEnumerable<Invitation>>.Success(invitations);
        }
    }
}
