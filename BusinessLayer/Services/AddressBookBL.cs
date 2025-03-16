using BusinessLayer.Interface;
using ModelLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class AddressBookBL : IAddressBookBL
    {
        private readonly IAddressBookRL _addressRL;
        private readonly IRabbitMQProducer _rabbitMQProducer;
        public AddressBookBL(IAddressBookRL addressRL,IRabbitMQProducer rabbitMQProducer)
        {
            _addressRL = addressRL;
            _rabbitMQProducer = rabbitMQProducer;
        }

        public async Task<AddressEntity> AddAddress(AddresDTO addressDTO)
        {
            if (addressDTO == null)
                throw new ArgumentNullException(nameof(addressDTO), "Invalid address details.");

            try
            {
                var result = await _addressRL.AddAddress(addressDTO);

                // Publish event to RabbitMQ when a new contact is added
                _rabbitMQProducer.SendProductMessage(new
                {
                    Message = "New Contact Added",
                    Name = addressDTO.Name,
                    Email = addressDTO.Email,
                    PhoneNumber = addressDTO.PhoneNumber
                });

                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> DeleteAddress(int Id)
        {
            try
            {
                var result = await _addressRL.DeleteAddress(Id);

                return result;
            }catch(Exception ex)
            {
                throw;
            }
        }

        public  async Task<List<AddressEntity>> GetAllContact()
        {
            try
            {
                var result = await _addressRL.GetAllContact();
                return result;
            }catch(KeyNotFoundException ex)
            {
                throw;
            }
        }

        public async Task<AddressEntity> GetContactByID(int ID)
        {
            
            try
            {
                var result = await _addressRL.GetContactByID(ID);

                return result;
            }catch(KeyNotFoundException ex)
            {
                throw;
            }
        }

        public async Task<AddressEntity> UpdateAddress(AddresDTO addressDTO)
        {
            try
            {
                var result = await _addressRL.UpdateAddress(addressDTO);

                return result;
            }
            catch (KeyNotFoundException ex)
            {
                throw;
            }
            catch (ArgumentException ex)
            {
                throw;
            }
        }
    }
}
