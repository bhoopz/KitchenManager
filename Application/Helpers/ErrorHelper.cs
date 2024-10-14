using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers
{
    public static class ErrorHelper
    {
        public static IdentityError EmailTaken => new IdentityError
        {
            Code = "EmailTaken",
            Description = "Email is already taken."
        };

        public static ApiError Test => new ApiError("Test code", "Test desc");
        public static ApiError WrongCredentials => new ApiError("WrongCredentials", "Invalid Email or Password");
        public static ApiError Sww => new ApiError("Sww", "Something went wrong");
        public static ApiError Unauthorized => new ApiError("Unauthorized", "User not found");
        public static ApiError UserNotFound => new ApiError("NotFound", "User not found");
        public static ApiError RestaurantNotFound => new ApiError("NotFound", "Restaurant not found");
        public static ApiError Forbidden => new ApiError("Forbidden", "Access denied");
        public static ApiError AlreadyInvited => new ApiError("AlreadyInvited", "User already invited");
        public static ApiError AlreadyMember => new ApiError("AlreadyMember", "User is already member of this restaurant");
        public static ApiError InvitationNotFound => new ApiError("NotFound", "Invitation not found");
        public static ApiError WrongChat => new ApiError("NotFound", "User is not a participant in this chat.");


    }
}
