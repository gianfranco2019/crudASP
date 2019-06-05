using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEBASPLOGIN.Models;

namespace WEBASPLOGIN.Models
{
    public class OurDbContext : DbContext
    {
        public OurDbContext(DbContextOptions<OurDbContext> options) : base(options)
        {

        }
        public DbSet<UserAccount> userAccount { get; set; }
    
            
    }
}
