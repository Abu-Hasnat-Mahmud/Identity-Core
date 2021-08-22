using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityCore.Models
{
    public class DBContext:IdentityDbContext<ApplicationUser>
    {
        public DBContext(DbContextOptions<DBContext> options):base(options)
        {

        }
    }
}
