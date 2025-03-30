using AddressBook.Controllers;
using BusinessLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;
using Moq;
using NUnit.Framework;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AddressBookTests
{
    [TestFixture]
    public class AddressBookAPITests
    {
        private Mock<IUserAuthenticationBL> _authServiceMock;
        private Mock<IAddressBookBL> _addressServiceMock;
        private Mock<ICacheService> _cacheServiceMock;
        private UserAuthentication _authController;
        private AddressBook.Controllers.AddressBook _addressController;

        [SetUp]
        public void Setup()
        {
            _authServiceMock = new Mock<IUserAuthenticationBL>();
            _addressServiceMock = new Mock<IAddressBookBL>();
            _cacheServiceMock = new Mock<ICacheService>();

            _authController = new UserAuthentication(_authServiceMock.Object, _cacheServiceMock.Object);
            _addressController = new AddressBook.Controllers.AddressBook(_addressServiceMock.Object);
        }

        //PASS Test Case: User Registration (Valid Data)
        [Test]
        public async Task RegisterUser_ShouldReturnSuccess_WhenValidData()
        {
            var userDTO = new UserDTO { UserName = "Amit Sharma", Email = "amit.sharma@gmail.com", Password = "Amit@123" };
            var response = new UserEntity { UserName = "Amit Sharma", Email = "amit.sharma@gmail.com" };

            _authServiceMock.Setup(service => service.UserRegister(userDTO)).ReturnsAsync(response);

            var result = await _authController.RegisterUser(userDTO) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        //FAIL Test Case: User Registration (Missing Email)
        [Test]
        public async Task RegisterUser_ShouldReturnBadRequest_WhenEmailMissing()
        {
            var userDTO = new UserDTO { UserName = "Rahul Gupta", Email = "", Password = "Rahul@123" };

            _authServiceMock.Setup(service => service.UserRegister(userDTO))
                .ThrowsAsync(new ArgumentNullException("Email cannot be empty"));

            var result = await _authController.RegisterUser(userDTO) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }

        //PASS Test Case: User Login (Valid Credentials)
        [Test]
        public async Task LoginUser_ShouldReturnSuccess_WhenValidCredentials()
        {
            var loginDTO = new LoginDTO { Email = "priya.verma@gmail.com", Password = "Priya@456" };
            var response = "Valid JWT Token";

            _authServiceMock.Setup(service => service.Login(loginDTO)).ReturnsAsync(response);

            var result = await _authController.Login(loginDTO) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        // FAIL Test Case: User Login (Invalid Password)
        [Test]
        public async Task LoginUser_ShouldReturnNotFound_WhenInvalidPassword()
        {
            var loginDTO = new LoginDTO { Email = "priya.verma@gmail.com", Password = "WrongPass" };

            _authServiceMock.Setup(service => service.Login(loginDTO))
                .ReturnsAsync((string)null);

            var result = await _authController.Login(loginDTO) as NotFoundObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }

        // PASS Test Case: Add Contact (Valid Data)
        [Test]
        public async Task AddContact_ShouldReturnSuccess_WhenValidContact()
        {
            var contactDTO = new AddresDTO { Name = "Rohan Gupta", Email = "rohan.gupta@gmail.com", Address = "Mumbai, Maharashtra" };
            var response = new AddressEntity { Name = "Rohan Gupta", Email = "rohan.gupta@gmail.com" };

            _addressServiceMock.Setup(service => service.AddAddress(contactDTO)).ReturnsAsync(response);

            var result = await _addressController.AddContact(contactDTO) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        // FAIL Test Case: Add Contact (Invalid Email)
        [Test]
        public async Task AddContact_ShouldReturnBadRequest_WhenInvalidEmail()
        {
            var contactDTO = new AddresDTO { Name = "Sneha Iyer", Email = "invalid-email", Address = "Chennai, Tamil Nadu" };

            _addressServiceMock.Setup(service => service.AddAddress(contactDTO))
                .ThrowsAsync(new ArgumentException("Invalid email format"));

            var result = await _addressController.AddContact(contactDTO) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }

        // PASS Test Case: Get Contact by ID (Valid ID)
        [Test]
        public async Task GetContactById_ShouldReturnContact_WhenValidId()
        {
            int contactId = 1;
            var contact = new AddressEntity { 
                Name = "Neha kumari",
                Email = "neha.nair@gmail.com", 
                Address = "Kochi, Kerala"
            };

            _addressServiceMock.Setup(service => service.GetContactByID(contactId)).ReturnsAsync(contact);

            var result = await _addressController.GetById(contactId) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        // FAIL Test Case: Get Contact by ID (Non-existent ID)
        [Test]
        public async Task GetContactById_ShouldReturnNotFound_WhenIdNotExists()
        {
            int contactId = 999;

            _addressServiceMock.Setup(service => service.GetContactByID(contactId))
                .ThrowsAsync(new KeyNotFoundException("Contact not found"));

            var result = await _addressController.GetById(contactId) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }

        // PASS Test Case: Delete Contact (Valid ID)
        [Test]
        public async Task DeleteContact_ShouldReturnSuccess_WhenValidId()
        {
            int contactId = 2;
            _addressServiceMock.Setup(service => service.DeleteAddress(contactId)).ReturnsAsync(true);

            var result = await _addressController.DeleteContact(contactId) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        //  FAIL Test Case: Delete Contact (Non-existent ID)
        [Test]
        public async Task DeleteContact_ShouldReturnNotFound_WhenIdNotExists()
        {
            int contactId = 999;

            _addressServiceMock.Setup(service => service.DeleteAddress(contactId)).ReturnsAsync(false);

            var result = await _addressController.DeleteContact(contactId) as NotFoundObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }
    }
}
