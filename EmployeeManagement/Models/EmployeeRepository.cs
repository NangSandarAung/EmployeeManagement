using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    //This class provides the implementataions for IEmployeeRepository
    public class EmployeeRepository : IEmployeeRepository
    {
        private List<Employee> _empList;

        public EmployeeRepository()
        {
            _empList = new List<Employee>()
            {
                new Employee(){Id = 1, Name = "Nang", Email = "abc@gmail.com", Department = Department.IT},
                new Employee(){Id = 2, Name = "Mary", Email = "def@gmail.com", Department = Department.HR},
                new Employee(){Id = 3, Name = "Tone Tone", Email = "ghk@gmail.com", Department = Department.HR},
                new Employee(){Id = 4, Name = "Nang", Email = "abc@gmail.com", Department = Department.IT},
                new Employee(){Id = 5, Name = "Mary", Email = "def@gmail.com", Department = Department.HR},
                new Employee(){Id = 6, Name = "Tone Tone", Email = "ghk@gmail.com", Department = Department.IT},
            };
        }

        public IEnumerable<Employee> GetAllEmployee()
        {
            return _empList;
        }

        public Employee GetEmployee(int id)
        {
            Employee emp = _empList.FirstOrDefault(e => e.Id == id);
            return emp;
        }

        public Employee AddNewEmployee(Employee emp)
        {
            emp.Id = _empList.Max(e => e.Id) + 1;
            _empList.Add(emp);
            return emp;
        }

        public Employee UpdateEmployee(Employee emp)
        { 
            Employee employee = _empList.Find(e => e.Id == emp.Id);

            if(employee != null)
            {
                employee.Name = emp.Name;
                employee.Department = emp.Department;
                employee.Email = emp.Email;
            }   
            return employee;
        } 

        public Employee DeleteEmployee(int id)
        {
            Employee emp = _empList.FirstOrDefault(emp => emp.Id == id);
            if(emp != null)
            {
                _empList.Remove(emp);
               
            }
            return emp;
        }
    }
}
 