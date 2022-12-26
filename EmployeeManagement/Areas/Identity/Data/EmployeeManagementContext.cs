using EmployeeManagement.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EmployeeManagement.Models;
using System.Text.RegularExpressions;
using System.Security.Cryptography.Xml;

namespace EmployeeManagement.Data;

public class EmployeeManagementContext : DbContext
{
   
   public EmployeeManagementContext(DbContextOptions<EmployeeManagementContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Employee>().HasData(
             new Employee()
             {
                 Id = 1,
                 Name = "Mary",
                 Department = Employee.Dept.IT,
                 Email = "mary@pramimtech.com",
                 PhotoPath = "~/Images/noimage.jpg"
             },
             new Employee
             {
                 Id = 2,
                 Name = "John",
                 Department = Employee.Dept.HR,
                 Email = "john@pragimtech.com",
                 PhotoPath = "~/Images/noimage.jpg"
             },
             new Employee
             {
                 Id = 3,
                 Name = "Sam",
                 Department = Employee.Dept.IT,
                 Email = "sam@pragimtech.com",
                 PhotoPath = "~/Images/noimage.jpg"
             }
          );
    }
       

    public DbSet<Employee> Employees { get; set; }

     

}
