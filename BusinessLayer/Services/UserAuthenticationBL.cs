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
        public readonly ICacheService _cacheService;
        private readonly IRabbitMQProducer _rabbitMQProducer;

        public UserAuthenticationBL(IUserAuthenticationRL userAuthenticationRL, ITokenService tokenService, IEmailService emailService, ICacheService cacheService, IRabbitMQProducer rabbitMQProducer)
        {
            _userAuthenticationRL = userAuthenticationRL;
            _tokenService = tokenService;
            _emailService = emailService;
            _cacheService = cacheService;
            _rabbitMQProducer = rabbitMQProducer;
        }

        public async Task<string> ForgetPassword(string Email)
        {
            await _cacheService.RemoveAsync(Email);

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

                await _cacheService.SetAsync(Email, "Reset link sent", TimeSpan.FromMinutes(10));

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


                if (!BCrypt.Net.BCrypt.Verify(loginDTO.Password, result.Password))
                {
                    return null;
                }
                var cachedToken = await _cacheService.GetAsync<string>(loginDTO.Email);
                if (!string.IsNullOrEmpty(cachedToken))
                {
                    return cachedToken; // Return cached token if available
                }
                var token = _tokenService.GenerateToken(result);
                await _cacheService.SetAsync(result.Email, token, TimeSpan.FromMinutes(20));
                return token;
                


                
                
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
            if (userDTO == null)
                throw new ArgumentNullException(nameof(userDTO), "Invalid user details.");

            try
            {
                userDTO.Password = BCrypt.Net.BCrypt.HashPassword(userDTO.Password);
                var result = await _userAuthenticationRL.UserRegister(userDTO);

                // Publish event to RabbitMQ to send email
                var emailModel = new EmailModel
                {
                    To = userDTO.Email,
                    Subject = "Welcome to Our Platform",
                    Body = userDTO.ToString()
                };

                _rabbitMQProducer.SendProductMessage(emailModel);

                return result;
            }
            catch
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

        public async Task<bool> Logout(string email)
        {
            try
            {
                // Remove Token from Cache (Invalidate Session)
                await _cacheService.RemoveAsync(email);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
