using System.Data.Entity;
using System;

namespace ExampleApp
{
    class AppContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public AppContext() : base("DefaultConnection") { }
    }
}
