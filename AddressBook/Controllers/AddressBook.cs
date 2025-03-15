using BusinessLayer.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;
using RepositoryLayer.Entity;

namespace AddressBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressBook : ControllerBase
    {
        private readonly IAddressBookBL _addressBL;
        

        public AddressBook(IAddressBookBL addressBL)
        {
            _addressBL = addressBL;
        }



        /// <summary>
        /// Fetch all Contacts
        /// </summary>
        /// <returns></returns>
        [HttpGet("AddressBook")]
        public async Task<IActionResult> AllContacts()
        {
            ResponseModel<IEnumerable<AddressEntity>> response = new ResponseModel<IEnumerable<AddressEntity>>();
            try
            {
                var result = await _addressBL.GetAllContact();

                response.Success = true;
                response.Message = "All contact Are following.";
                response.Data = result;

                return Ok(response);
            }
            catch(KeyNotFoundException ex)
            {
                return BadRequest("No data found");
            }
            catch(Exception ex)
            {
                return BadRequest("Some error occurred. " + ex);
            }
        }

        /// <summary>
        /// Get Address by ID 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int ID) 
        {
            ResponseModel<AddressEntity> response = new ResponseModel<AddressEntity>();
            try
            {
                var result = await _addressBL.GetContactByID(ID);

                response.Success = true;
                response.Message = "Contact by this ID";
                response.Data = result;

                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest("No data found");
            }
            catch (Exception ex)
            {
                return BadRequest("Some error occurred. " + ex);
            }
        }

        /// <summary>
        /// Add new Address
        /// </summary>
        /// <returns></returns>
        [HttpPost("NewContact")]

        public async Task<IActionResult> AddContact(AddresDTO addresDTO) 
        {
            ResponseModel<string> responseModel = new ResponseModel<string>();
            try
            {
                var result = await _addressBL.AddAddress(addresDTO);

                responseModel.Success = true;
                responseModel.Message = "New Address Added successfully.";
                responseModel.Data = $"{result.Email} added successfully.";

                return Ok(responseModel);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex);
            }catch(Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        /// <summary>
        /// Update the contact
        /// </summary>
        /// <returns></returns>
        [HttpPut("UpdateContact")]
        public async Task<IActionResult> UpdateContact(AddresDTO addressDTO)
        {
            ResponseModel<string> responseModel = new ResponseModel<string>();
            try
            {
                var result = await _addressBL.UpdateAddress(addressDTO);

                responseModel.Success = true;
                responseModel.Message = "Address update successfully.";
                responseModel.Data = $"{result.Email} update successfully.";

                return Ok(responseModel);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest("Email does not found.");
            }
            catch (ArgumentException ex)
            {
                return StatusCode(500, "Give correct Information.");
            }
        }


        /// <summary>
        /// Delete contact by Id
        /// </summary>
        /// <returns></returns>
        [HttpDelete("DeleteContact")]

        public async Task<IActionResult> DeleteContact(int id)
        {
            ResponseModel<string> responseModel = new ResponseModel<string>();

            try
            {
                bool result = await _addressBL.DeleteAddress(id); // Await the async method

                if (result)
                {
                    responseModel.Success = true;
                    responseModel.Message = "Address deleted successfully.";
                    return Ok(responseModel);
                }

                responseModel.Success = false;
                responseModel.Message = "Address not found.";
                return NotFound(responseModel); 
            }
            catch (Exception ex)
            {
                responseModel.Success = false;
                responseModel.Message = $"An error occurred: {ex.Message}";
                return StatusCode(500, responseModel); 
            }
        }
    }
}
