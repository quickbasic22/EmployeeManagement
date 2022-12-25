﻿namespace EmployeeManagement.Models
{
    public interface IEmployeeRepository
    {
        Employee GetEmployee(int id);
        IEnumerable<Employee> GetAllEmployees();
        void Save(Employee employee);
    }
}
