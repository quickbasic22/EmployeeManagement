namespace EmployeeManagement.Models
{
    public class MockEmployeeReposity : IEmployeeRepository
    {
        public List<Employee> _employeeList;

        public MockEmployeeReposity()
        {
            _employeeList = new List<Employee>()
            { new Employee() { Id = 1, Name = "Mary", Department = "HR", Email = "mary@pragimtech.com" },
              new Employee() { Id = 2, Name = "John", Department = "IT", Email = "john@pramitech.com" },
              new  Employee() { Id = 3, Name = "Sam", Department = "IT", Email = "sam@pramitech.com" }
            };
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _employeeList;
        }

        public Employee GetEmployee(int id)
        {
            return _employeeList.FirstOrDefault(e => e.Id == id);
        }
    }
}
