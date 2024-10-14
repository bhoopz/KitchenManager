using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Responses;
using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterUser(RegisterUserDto userData);
        Task<ApiResponse<LoginResponseDto>> LoginUser(LoginUserDto credentials);
    }

}
