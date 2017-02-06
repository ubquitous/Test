using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace CrossDoubleN.Models
{
    public class UserContext : DbContext
    {
        public UserContext()
            : base("Users")
        { }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
    }

    public class UserDbInitializer : DropCreateDatabaseAlways<UserContext>
    {
        
    }
}