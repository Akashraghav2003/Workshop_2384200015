using ModelLayer.Model;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IUserAuthenticationBL
    {
        Task<UserEntity> UserRegister(UserDTO userDTO);
        Task<string> Login(LoginDTO loginDTO);
    }
}
