using ModelLayer.Model;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IUserAuthenticationRL
    {
        Task<UserEntity> UserRegister(UserDTO userDTO);
        Task<UserEntity> Login(LoginDTO loginDTO);
        Task<UserEntity> ForgetPassword(string Email);

        Task<UserEntity> ResetPassword(String Email, string Password, string ConfirmPassword);


    }
}
