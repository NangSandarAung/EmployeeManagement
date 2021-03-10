using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    //create an extension class of ModelBuilder 
    //need to be a static class
    public static class ModelBuilderExtension
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            //put the model class that we want to seed inside database
            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    Id = 1,
                    Name = "john",
                    Email = "john@gmail.com",
                    Department = Department.IT,
                },
                 new Employee
                 {
                     Id = 2,
                     Name = "honey",
                     Email = "honey@gmail.com",
                     Department = Department.HR,
                 }
                );
        }
    }
}
