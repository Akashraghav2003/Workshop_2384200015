using ModelLayer.Model;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IAddressBookBL
    {
        Task<List<AddressEntity>> GetAllContact();

        Task<AddressEntity> GetContactByID(int ID);

        Task<AddressEntity> AddAddress(AddresDTO addressDTO);
        Task<AddressEntity> UpdateAddress(AddresDTO addressDTO);

        Task<bool> DeleteAddress(int Id);
    }
}
