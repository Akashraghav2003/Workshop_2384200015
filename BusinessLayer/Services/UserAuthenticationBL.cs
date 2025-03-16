using BusinessLayer.Interface;
using Microsoft.EntityFrameworkCore;
using ModelLayer.Model;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace BusinessLayer.Services
{
    public class UserAuthenticationBL : IUserAuthenticationBL
    {
        private readonly IUserAuthenticationRL _userAuthenticationRL;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;

        public UserAuthenticationBL(IUserAuthenticationRL userAuthenticationRL, ITokenService tokenService, IEmailService emailService)
        {
            _userAuthenticationRL = userAuthenticationRL;
            _tokenService = tokenService;
            _emailService = emailService;
        }

        public async Task<string> ForgetPassword(string Email)
        {
            try
            {
                var result = await _userAuthenticationRL.ForgetPassword(Email);
                string body =  _tokenService.GenerateToken(result);

                EmailModel emailModel = new EmailModel
                {
                    To = Email,
                    Subject = "Password reset link send on Email.",
                    Body = body
                };

                _emailService.SendEmail(emailModel);
                return "Check your Email."; 
            }catch(KeyNotFoundException ex)
            {
                throw;
            }catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<string> Login(LoginDTO loginDTO)
        {
            if (loginDTO == null)
            {
                throw new ArgumentNullException("checked the correct credentials.");
            }
            try
            {
                var result = await _userAuthenticationRL.Login(loginDTO);

                if (BCrypt.Net.BCrypt.Verify(loginDTO.Password, result.Password)) return _tokenService.GenerateToken(result);

                return null;
                
            }catch(KeyNotFoundException ex)
            {
                throw;
            }catch(ArgumentNullException ex)
            {
                throw;
            }
        }

        public async Task<UserEntity> UserRegister(UserDTO userDTO)
        {
            if(userDTO == null)
            {
                throw new ArgumentNullException("checked the correct credentials.");
            }
            try
            {
                userDTO.Password = BCrypt.Net.BCrypt.HashPassword(userDTO.Password);
                var result = await _userAuthenticationRL.UserRegister(userDTO);

                return result;
            }
            catch (InvalidOperationException ex)
            {
                throw;
            }
            
            catch (Exception ex)
            {
                throw;
            }
        }

        public async  Task<bool> ResetPassword(string token, string Password, string confirmPassword)
        {
            try
            {
                var email = _tokenService.ValidateToken(token);
                var newPassword = BCrypt.Net.BCrypt.HashPassword(Password);
                var result = await _userAuthenticationRL.ResetPassword(email, newPassword, confirmPassword);

                return true;
            }
            catch (KeyNotFoundException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
