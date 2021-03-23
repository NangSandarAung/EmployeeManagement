using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Models
{
    //create DBContext to interact with Database
    //our class need to derived from DbContext class
    //specify ApplicationUser as the generic argument for IdentityDbContext
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        //we create an instance of DbContextOptions in order to have access to base DbContext
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        //to seed data into database
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //call the ModelBuilder extension class's method to seed data
            modelBuilder.Seed();
           
        }


        //need to write DbSet for each entity in the model
        //we use this to query and save instances of model's class
        public DbSet<Employee> Employees { get; set; } 
    }
}
