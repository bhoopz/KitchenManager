using Application.Responses;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Application.DTOs;
using Domain.Entities;
using Application.Helpers;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, JwtTokenGenerator jwtTokenGenerator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }
        public async Task<IdentityResult> RegisterUser(RegisterUserDto userData)
        {
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(userData.Email);
                if (existingUser != null)
                {
                    return IdentityResult.Failed(ErrorHelper.EmailTaken);              
                }

                var user = new ApplicationUser
                {
                    FirstName = userData.FirstName,
                    LastName = userData.LastName,
                    UserName = userData.Email,
                    Email = userData.Email,
                    PhoneNumber = userData.PhoneNumber,
                };

                return await _userManager.CreateAsync(user, userData.Password);

               
            }
            catch (Exception ex)
            {

                return IdentityResult.Failed(new IdentityError { Description = ex.Message });
            }
            
        }
        public async Task<ApiResponse<LoginResponseDto>> LoginUser(LoginUserDto credentials)
        {
            var user = await _userManager.FindByEmailAsync(credentials.Email);
            if (user != null)
            {
               var result = await _signInManager.PasswordSignInAsync(user.UserName, credentials.Password, false, true);

                if (!result.Succeeded)
                {
                    return ApiResponse<LoginResponseDto>.Failure(ErrorHelper.WrongCredentials);
                }
                else
                {
                    var token = _jwtTokenGenerator.GenerateToken(user);
                    Console.WriteLine(token.ToString());
                    return ApiResponse<LoginResponseDto>.Success(new LoginResponseDto { AccessToken = token});
                };
            }
            else
            {
                return ApiResponse<LoginResponseDto>.Failure(ErrorHelper.WrongCredentials);
            }

        }
        
    }
}
