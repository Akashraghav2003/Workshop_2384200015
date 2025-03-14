using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AddressBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressBook : ControllerBase
    {
        /// <summary>
        /// Fetch all Contacts
        /// </summary>
        /// <returns></returns>
        [HttpGet("AddressBook")]

        public Task<IActionResult> AllContacts() { }

        /// <summary>
        /// Get Contact by ID 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetById")]
        public Task<IActionResult> GetById() { }

        /// <summary>
        /// Add new Contact
        /// </summary>
        /// <returns></returns>
        [HttpPost("NewContact")]

        public Task<IActionResult> AddContact() { }

        /// <summary>
        /// Update the contact
        /// </summary>
        /// <returns></returns>
        [HttpPut("UpdateContact")]
        public Task<IActionResult> UpdateContact() { }


        /// <summary>
        /// Delete contact by Id
        /// </summary>
        /// <returns></returns>
        [HttpDelete("DeleteContact")]

        public Task<IActionResult> DeleteContact() { }
    }
}
