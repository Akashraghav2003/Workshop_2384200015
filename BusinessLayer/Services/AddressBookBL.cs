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
        public AddressBookBL(IAddressBookRL addressRL)
        {
            _addressRL = addressRL;  
        }

        public async Task<AddressEntity> AddAddress(AddresDTO addressDTO)
        {
            try
            {
                var result = await _addressRL.AddAddress(addressDTO);

                return result;
            }
            catch (ArgumentException ex)
            {
                throw;
            }
            catch (InvalidOperationException ex)
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
