using Application.Helpers;
using Application.Interfaces;
using Application.Responses;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class MembershipService : IMembershipService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        public MembershipService(IUnitOfWork unitOfWork, IUserContextService userContextService) { 
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
        }

        public async Task<ApiResponse<IEnumerable<Membership>>> GetUserMemberships()
        {
            Guid userId = _userContextService.GetUserId();

            if(userId == Guid.Empty) return ApiResponse<IEnumerable<Membership>>.Failure(ErrorHelper.Unauthorized);

            var memberships = await _unitOfWork.GetGenericRepository<Membership>()
            .FindAllAsync(m => m.UserId == userId, m => m.Restaurant);

            return ApiResponse<IEnumerable<Membership>>.Success(memberships);
        }
    }
}
