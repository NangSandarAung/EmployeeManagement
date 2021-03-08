using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
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
    }
}
 