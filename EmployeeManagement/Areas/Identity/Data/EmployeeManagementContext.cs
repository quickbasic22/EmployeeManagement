using EmployeeManagement.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EmployeeManagement.Models;
using System.Text.RegularExpressions;

namespace EmployeeManagement.Data;

public class EmployeeManagementContext : IdentityDbContext<AppUser, IdentityRole, string>
{
    public EmployeeManagementContext()
    {
    }
    public EmployeeManagementContext(DbContextOptions<EmployeeManagementContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

    public Employee GetEmployee(int id)
    {
        List<Employee> employees = this.Employees.ToList();
        Employee employee = employees.Find(a => a.Id == id);
        if (employee != null)
        {
            return employee;
        }
        return new Employee();
    }

    public IEnumerable<Employee> GetAllEmployees()
    {
        return this.Employees;
    }

    public void Save(Employee employee)
    {
       this.Update(employee);
    }

    public DbSet<Employee> Employees { get; set; }

     

}
