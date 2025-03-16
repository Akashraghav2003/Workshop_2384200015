using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;
using RepositoryLayer.Interface;

namespace AddressBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAuthentication : ControllerBase
    {
        private readonly IUserAuthenticationBL _userAuthenticationBL;
        private readonly ICacheService _cacheService;

        public UserAuthentication(IUserAuthenticationBL userAuthenticationBL, ICacheService cacheService)
        {
            _userAuthenticationBL = userAuthenticationBL;
            _cacheService = cacheService;
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


        /// <summary>
        /// User Login Register
        /// </summary>
        /// <param name="loginDTO"></param>
        /// <returns></returns>
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

        [HttpPost("ForgetPassword")]

        public async Task<IActionResult> ForgetPassword(string Email)
        {
            ResponseModel<string> response = new ResponseModel<string>();
            try
            {
                var result = await _userAuthenticationBL.ForgetPassword(Email);

                response.Success = true;
                response.Message = "Check your Email.";
                response.Data = result;

                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                response.Success = false;
                response.Message = "Email does not match";
                response.Data = "Give correct email";

                return NotFound(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Some error occurred.";
                

                return StatusCode(500,response);
            }
        }

        /// <summary>
        /// Reset Password
        /// </summary>
        /// <param name="token"></param>
        /// <param name="newPassword"></param>
        /// <param name="confirmPassword"></param>
        /// <returns>Response body</returns>
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(string token, string newPassword, string confirmPassword)
        {
            ResponseModel<string> responseModel = new ResponseModel<string>();
            try
            {
                var result = await _userAuthenticationBL.ResetPassword(token, newPassword, confirmPassword);

                responseModel.Success = true;
                responseModel.Message = "Password Reset Successfully.";
                responseModel.Data = "";

                return Ok(responseModel);
            }
            catch (KeyNotFoundException ex)
            {
                responseModel.Success = false;
                responseModel.Message = "Email Does not match.";
                responseModel.Data = ex.Message;
                return Unauthorized(responseModel);
            }
            catch (Exception ex)
            {
                responseModel.Success = false;
                responseModel.Message = "Some Error Occurred";
                responseModel.Data = ex.Message;
                return Unauthorized(responseModel);
            }
        }

        /// <summary>
        /// User Logout (Invalidate Token)
        /// </summary>
        [HttpPost("LogoutUser")]
        public async Task<IActionResult> Logout(string email)
        {
            var response = new ResponseModel<string>();
            try
            {
                await _cacheService.RemoveAsync(email); // Remove cached token
                response.Success = true;
                response.Message = "User logged out successfully.";
                response.Data = "";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error occurred during logout.";
                response.Data = ex.Message;
                return StatusCode(500, response);
            }
        }
    }
}
