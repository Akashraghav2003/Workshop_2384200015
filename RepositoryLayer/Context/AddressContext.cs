using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Context
{
    public class AddressContext : DbContext
    {
        public AddressContext(DbContextOptions<AddressContext> options) : base(options) { }

        public virtual DbSet<AddressEntity> AddressEntities { get; set; }

        public virtual DbSet<UserEntity> UserEntities { get; set; }

    }
}
