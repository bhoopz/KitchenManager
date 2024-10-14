using Application.DTOs;
using Application.Helpers;
using Application.Interfaces;
using Application.Responses;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RestaurantService(IUnitOfWork unitOfWork)
        {
             _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<Restaurant>> CreateRestaurant(CreateRestaurantDto restaurantData)
        {
            var restaurant = new Restaurant{
                Name = restaurantData.Name,
                Address = restaurantData.Address,
                PhoneNumber = restaurantData.PhoneNumber,
            };

            try
            {
                var createdRestaurant = await _unitOfWork.RestaurantRepository.CreateRestaurantAsync(restaurant);

                if (createdRestaurant != null)
                {
                    var membership = new Membership
                    {
                        UserId = createdRestaurant.CreatedBy,
                        RestaurantId = createdRestaurant.RestaurantId,
                        Role = UserRole.Owner,
                        StartDate = DateTime.UtcNow
                    };
                    await _unitOfWork.GetGenericRepository<Membership>().AddAsync(membership);

                    var chat = new Chat
                    {
                        Name = createdRestaurant.Name,
                        RestaurantId = createdRestaurant.RestaurantId,
                    };
                    await _unitOfWork.GetGenericRepository<Chat>().AddAsync(chat);
                    await _unitOfWork.CompleteAsync();

                    return ApiResponse<Restaurant>.Success(createdRestaurant);
                }

                return ApiResponse<Restaurant>.Failure(ErrorHelper.Sww);
            }
            catch (Exception)
            {
                return ApiResponse<Restaurant>.Failure(ErrorHelper.Sww);
            }
        }

    }
}
