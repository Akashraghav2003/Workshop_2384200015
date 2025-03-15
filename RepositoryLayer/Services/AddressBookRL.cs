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
    public class AddressBookRL : IAddressBookRL
    {
        private readonly AddressContext _dbContext;

        public AddressBookRL(AddressContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AddressEntity> AddAddress(AddresDTO addressDTO)
        {
            if(addressDTO == null)
            {
                throw new ArgumentNullException("Give correct Data.");
            }
            try
            {
                var result = await _dbContext.AddressEntities.FirstOrDefaultAsync(e => e.Email == addressDTO.Email);

                if(result != null)
                {
                    throw new InvalidOperationException("Email is already register.");
                }

                var newAddress = new AddressEntity
                {
                    Email = addressDTO.Email,
                    Address = addressDTO.Address,
                    Name = addressDTO.Name,
                    PhoneNumber = addressDTO.PhoneNumber,
                    UserId = addressDTO.UserId
                };

                _dbContext.Add(newAddress);
                _dbContext.SaveChanges();

                return newAddress;
            }catch(ArgumentException ex)
            {
                throw;
            }
            catch(InvalidOperationException ex)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAddress(int Id)
        {
            try
            {
                var result = await _dbContext.AddressEntities.FirstOrDefaultAsync(e => e.Id == Id);

                if(result == null)
                {
                    return false;
                }

                _dbContext.Remove(result);
                _dbContext.SaveChanges();

                return true;
            }catch(Exception ex)
            {
                throw;
            }
        }

        public async  Task<List<AddressEntity>> GetAllContact()
        {
            try
            {
                var result = await _dbContext.AddressEntities.ToListAsync();

                if (result == null)
                {
                    throw new KeyNotFoundException();
                }

                return result;
            }
            catch(KeyNotFoundException ex)
            {
                throw;
            }
        }

        public async Task<AddressEntity> GetContactByID(int ID)
        {
            try
            {
                var result = await _dbContext.AddressEntities.FirstOrDefaultAsync(e => e.Id == ID);

                if(result == null)
                {
                    throw new KeyNotFoundException("Id does not found.");
                }

                return result;
            }catch(KeyNotFoundException ex)
            {
                throw;
            }
        }

        public async Task<AddressEntity> UpdateAddress(AddresDTO addressDTO)
        {
            if (addressDTO == null)
            {
                throw new ArgumentNullException("Give correct Data.");
            }
            try
            {
                var result = await _dbContext.AddressEntities.FirstOrDefaultAsync(e => e.Email == addressDTO.Email);

                if (result == null)
                {
                    throw new KeyNotFoundException("Email is not register.");
                }



                result.Address = addressDTO.Address;
                result.Name = addressDTO.Name;
                result.PhoneNumber = addressDTO.PhoneNumber;
                result.UserId = addressDTO.UserId;


                _dbContext.Update(result);
                _dbContext.SaveChanges();

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
