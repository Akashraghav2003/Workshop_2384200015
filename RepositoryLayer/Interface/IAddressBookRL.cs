using ModelLayer.Model;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IAddressBookRL
    {
        Task<List<AddressEntity>> GetAllContact();
        Task<AddressEntity> GetContactByID(int ID);
        Task<AddressEntity> AddAddress(AddresDTO addresDTO);
        Task<AddressEntity> UpdateAddress(AddresDTO addresDTO);
        Task<bool> DeleteAddress(int Id);
    }
}
