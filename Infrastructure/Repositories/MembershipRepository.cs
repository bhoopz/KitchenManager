using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class MembershipRepository : IMembershipRepository
    {
        private readonly ApplicationDbContext _context;

        public MembershipRepository(ApplicationDbContext context)
        {
               _context = context;
        }
        public async Task<bool> IsUserInRestaurantAsync(Guid userId, Guid restaurantId)
        {
            return await _context.Memberships
                                 .AnyAsync(m => m.UserId == userId && m.RestaurantId == restaurantId);
        }
    }
}
