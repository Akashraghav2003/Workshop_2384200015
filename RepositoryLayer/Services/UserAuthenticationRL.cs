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

namespace RepositoryLayer.Services
{
    public class UserAuthenticationRL : IUserAuthenticationRL
    {
        public readonly AddressContext _dbContext;
        

        public UserAuthenticationRL(AddressContext dbContext)
        {
            _dbContext = dbContext;
            
        }

        public async Task<UserEntity> ForgetPassword(string Email)
        {
            try
            {
                var result = await _dbContext.UserEntities.FirstOrDefaultAsync(e => e.Email == Email);

                if(result == null)
                {
                    throw new KeyNotFoundException("Email does not found.");
                }
                return result;
            }catch(KeyNotFoundException ex)
            {
                throw;

            }
        }

        public async Task<UserEntity> Login(LoginDTO loginDTO)
        {
            try
            {
                var result = await _dbContext.UserEntities.FirstOrDefaultAsync(e => e.Email == loginDTO.Email);

                if(result == null)
                {
                    throw new KeyNotFoundException("User not found.");
                }
                return result;
            }catch(KeyNotFoundException ex)
            {
                throw;
            }
        }

        public async Task<UserEntity> UserRegister(UserDTO userDTO)
        {
            try
            {
                var result = await _dbContext.UserEntities.FirstOrDefaultAsync(e => e.Email == userDTO.Email && e.UserName == userDTO.UserName);

                if (result != null)
                {
                    throw new InvalidOperationException("Give Email And Username already present.");
                }

                var newUser = new UserEntity
                {
                    UserName = userDTO.UserName,
                    Email = userDTO.Email,
                    Password = userDTO.Password
                };

                _dbContext.UserEntities.Add(newUser);
                await _dbContext.SaveChangesAsync();

                return newUser;
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

        public async Task<UserEntity> ResetPassword(String Email, string Password, string ConfirmPassword)
        {
            try
            {
                var result = await _dbContext.UserEntities.FirstOrDefaultAsync(e => e.Email == Email);

                if (result == null)
                {
                    throw new KeyNotFoundException("Email not found");
                }

                result.Password = Password;

                await _dbContext.SaveChangesAsync();

                return result;
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
