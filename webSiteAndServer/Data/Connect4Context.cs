using System;
using Microsoft.EntityFrameworkCore;
using webSiteAndServer.Model;

namespace webSiteAndServer.Data
{
    public class Connect4Context : DbContext
    {

        public Connect4Context(DbContextOptions options):base(options)
        {

        }
		public DbSet<User> users { get; set; }


     

    }


}

