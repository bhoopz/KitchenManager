using Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IRestaurantRepository
    {
        Task<Restaurant> CreateRestaurantAsync(Restaurant restaurant);
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
