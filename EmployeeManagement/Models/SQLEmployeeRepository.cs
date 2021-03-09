using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class SQLEmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;
        public SQLEmployeeRepository(AppDbContext context)
        {
            _context = context;
        }
        public Employee AddNewEmployee(Employee emp)
        {
            _context.Employees.Add(emp);
            _context.SaveChanges();
            return emp;
        }

        public Employee DeleteEmployee(int id)
        {
            Employee emp = _context.Employees.Find(id);
            if(emp != null)
            {
                _context.Employees.Remove(emp);
                _context.SaveChanges();
            } 
            return emp;
        }

        public IEnumerable<Employee> GetAllEmployee()
        {
            List<Employee> empList = _context.Employees.ToList();
            return empList;
        }

        public Employee GetEmployee(int id)
        {
            Employee emp = _context.Employees.Find(id);
            return emp;
        }

        //public Employee UpdateEmployee(Employee emp)
        //{
        //    Employee employee = _context.Employees.Find(emp.Id);
        //    if(employee != null)
        //    {
        //        _context.Employees.Update(employee);
        //        _context.SaveChanges();
        //    }
        //    return employee;
        //}

        public Employee UpdateEmployee(Employee emp)
        {
            var employee = _context.Employees.Attach(emp);
            employee.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return emp;
        }
    }
}
