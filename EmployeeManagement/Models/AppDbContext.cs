using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    //create DBContext to interact with Database
    //our class need to derived from DbContext class
    public class AppDbContext : DbContext
    {
        //we create an instance of DbContextOptions in order to have access to base DbContext
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        //need to write DbSet for each entity in the model
        //we use this to query and save instances of model's class
        public DbSet<Employee> Employees { get; set; } 
    }
}
