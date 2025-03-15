using BusinessLayer.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;

namespace AddressBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAuthentication : ControllerBase
    {
        private readonly IUserAuthenticationBL _userAuthenticationBL;

        public UserAuthentication(IUserAuthenticationBL userAuthenticationBL)
        {
            _userAuthenticationBL = userAuthenticationBL;
        }

        /// <summary>
        /// Register the user
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        [HttpPost("Register User")]
        public async Task<IActionResult> RegisterUser(UserDTO userDTO)
        {
            ResponseModel<string> response = new ResponseModel<string>();
            try
            {
                var result = await _userAuthenticationBL.UserRegister(userDTO);

                response.Success = true;
                response.Message = "User register Successfully";
                response.Data = $"{result.UserName} added Successfully.";

                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                response.Success = false;
                response.Message = "User does not register.";
                response.Data = ex.Message;

                return BadRequest(response);
            }
            catch (ArgumentNullException ex)
            {
                response.Success = false;
                response.Message = "User does not register.";
                response.Data = ex.Message;

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "User does not register.";
                response.Data = ex.Message;

                return BadRequest(response);
            }
        }

        [HttpPost("Login User")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            ResponseModel<string> response = new ResponseModel<string>();
            try
            {
                var result = await _userAuthenticationBL.Login(loginDTO);

                if(result == null)
                {
                    response.Success = false;
                    response.Message = "Password Mismatch.";
                    response.Data = "Give the correct password.";

                    return NotFound(response);
                }

                response.Success = true;
                response.Message = "Login Successful.";
                response.Data = result;

                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                response.Success = false;
                response.Message = "Email not found";
                response.Data = "Give the correct Email.";

                return NotFound(response);
            }
            catch (ArgumentNullException ex)
            {
                response.Success = false;
                response.Message = "Some error occurred";
                response.Data = "Give the correct ID, Password";

                return StatusCode(500,response);
            }
        }
    }
}
