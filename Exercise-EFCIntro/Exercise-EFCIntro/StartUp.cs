using System;
using System.Globalization;
using System.Text;
using Exercise_EFCIntro.Data;
using Exercise_EFCIntro.Models;
using Microsoft.EntityFrameworkCore;

namespace Exercise_EFCIntro
{
	public class StartUp
	{
		public static void Main(string[] args)
		{
			using SoftUniContext context = new SoftUniContext();

			string result = RemoveTown(context);

			Console.WriteLine(result);

			Console.ReadKey();
		}

		public static string GetEmployeesFullInformation(SoftUniContext context)
		{
			StringBuilder sb = new StringBuilder();

			var employees = context.Employees
				.OrderBy(e => e.EmployeeId)
				.Select(e => new
				{
					e.FirstName,
					e.LastName,
					e.MiddleName,
					e.JobTitle,
					e.Salary
				})
				.ToList();

			foreach(var employee in employees)
			{
				sb.AppendLine($"{employee.FirstName} {employee.LastName} {employee.MiddleName} {employee.JobTitle} {employee.Salary:F2}");
			}
			return sb.ToString().TrimEnd();
		}

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
		{
			StringBuilder sb = new StringBuilder();

			var employees = context.Employees
				.OrderBy(e => e.FirstName)
				.Select(e => new
				{
					e.FirstName,
					e.Salary
				})
				.Where(e => e.Salary >= 50000)
				.ToList();

			foreach(var employee in employees)
			{
				sb.AppendLine($"{employee.FirstName} - {employee.Salary:F2}");
			}

			return sb.ToString().TrimEnd();
		}

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
		{
			StringBuilder sb = new StringBuilder();

			var employees = context.Employees
                .OrderByDescending(e => e.FirstName)
                .OrderBy(e => e.Salary)
				.Select(e => new
				{
					e.FirstName,
					e.LastName,
					e.Department,
					e.Salary
				})
				.Where(e => e.Department.Name == "Research and Development")
				.ToList();

			foreach (var employee in employees)
			{
				sb.AppendLine($"{employee.FirstName} {employee.LastName} from {employee.Department.Name} - ${employee.Salary:F2}");
			}

			return sb.ToString().TrimEnd();
		}

        public static string AddNewAddressToEmployee(SoftUniContext context)
		{
			StringBuilder sb = new StringBuilder();

			Address newAddress = new Address()
			{
				AddressText = "Vitoshka 15",
				AddressId = 4
			};

			Employee employee = context.Employees
				.First(e => e.LastName == "Nakov");

			employee.Address = newAddress;

			context.SaveChanges();

			var addresses = context.Employees
				.OrderByDescending(e => e.AddressId)
				.Take(10)
				.Select(e => e.Address.AddressText)
				.ToList();

			foreach (var address in addresses)
			{
				sb.AppendLine(address);
			}

			return sb.ToString().TrimEnd();
		}

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .OrderBy(e => e.EmployeeId)
                .Take(10)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    ManagerFirstName = e.Manager.FirstName,
                    ManagerLastName = e.Manager.LastName,
                    Projects = e.Projects
                        .Where(p => p.StartDate.Year >= 2001 &&
                                    p.StartDate.Year <= 2003)
                        .Select(p => new
                        {
                            p.Name,
                            p.StartDate,
                            p.EndDate
                        })
                        .ToArray()
                })
                .ToArray();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} - Manager: {employee.ManagerFirstName} {employee.ManagerLastName}");

                foreach (var project in employee.Projects)
                {
                    string startDate = project.StartDate
                        .ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);

                    string endDate = project.EndDate == null
                        ? "not finished"
                        : project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);

                    sb.AppendLine($"--{project.Name} - {startDate} - {endDate}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetAddressesByTown(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var addresses = context.Addresses
                .OrderByDescending(a => a.Employees.Count)
                .ThenBy(a => a.Town.Name)
                .ThenBy(a => a.AddressText)
                .Take(10)
                .Select(a => new
                {
                    AddressText = a.AddressText,
                    TownName = a.Town.Name,
                    EmployeeCount = a.Employees.Count
                })
                .ToList();

            foreach (var address in addresses)
            {
                sb.AppendLine($"{address.AddressText}, {address.TownName} - {address.EmployeeCount} employees");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployee147(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employee = context.Employees
                .Where(e => e.EmployeeId == 147)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    Projects = e.Projects
                        .OrderBy(p => p.Name)
                        .Select(p => p.Name)
                        .ToList()
                })
                .First();

            sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");

            foreach (string projectName in employee.Projects)
            {
                sb.AppendLine(projectName);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var departments = context.Departments
                .Where(d => d.Employees.Count > 5)
                .OrderBy(d => d.Employees.Count)
                .ThenBy(d => d.Name)
                .Select(d => new
                {
                    DepartmentName = d.Name,
                    ManagerFirstName = d.Manager.FirstName,
                    ManagerLastName = d.Manager.LastName,
                    Employees = d.Employees
                        .OrderBy(e => e.FirstName)
                        .ThenBy(e => e.LastName)
                        .Select(e => new
                        {
                            e.FirstName,
                            e.LastName,
                            e.JobTitle
                        })
                        .ToList()
                })
                .ToList();

            foreach (var department in departments)
            {
                sb.AppendLine($"{department.DepartmentName} - {department.ManagerFirstName} {department.ManagerLastName}");

                foreach (var employee in department.Employees)
                {
                    sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetLatestProjects(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var projects = context.Projects
                .OrderByDescending(p => p.StartDate)
                .Take(10)
                .Select(p => new
                {
                    p.Name,
                    p.Description,
                    p.StartDate
                })
                .ToList()
                .OrderBy(p => p.Name);

            foreach (var project in projects)
            {
                sb.AppendLine(project.Name);
                sb.AppendLine(project.Description);
                sb.AppendLine(project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture));
            }

            return sb.ToString().TrimEnd();
        }

        public static string IncreaseSalaries(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            string[] departmentNames =
            {
                "Engineering",
                "Tool Design",
                "Marketing",
                "Information Services"
            };

            var employees = context.Employees
                .Where(e => departmentNames.Contains(e.Department.Name))
                .ToList();

            foreach (var employee in employees)
            {
                employee.Salary *= 1.12m;
            }

            context.SaveChanges();

            employees = employees
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToList();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} (${employee.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .Where(e => e.FirstName.StartsWith("Sa"))
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    e.Salary
                })
                .ToList();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} - {employee.LastName} - (${employee.Salary:F2})");
            }

            return sb.ToString().TrimEnd();
        }

        public static string DeleteProjectById(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var project = context.Projects
                .Include(p => p.Employees)
                .First(p => p.ProjectId == 2);

            foreach(var employee in project.Employees.ToList())
            {
                project.Employees.Remove(employee);
            }

            context.SaveChanges();
            context.Projects.Remove(project);
            context.SaveChanges();

            var projects = context.Projects
                .OrderBy(p => p.ProjectId)
                .Take(10)
                .Select(p => p.Name)
                .ToList();

            foreach (var item in projects)
            {
                sb.AppendLine($"{item}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string RemoveTown(SoftUniContext context)
        {
            var town = context.Towns
                .First(t => t.Name == "Seattle");

            var addresses = context.Addresses
                .Where(a => a.TownId == town.TownId)
                .ToList();

            int deletedAddressesCount = addresses.Count;

            int[] addressIds = addresses
                .Select(a => a.AddressId)
                .ToArray();

            var employees = context.Employees
                .Where(e => e.AddressId != null && addressIds.Contains(e.AddressId.Value))
                .ToList();

            foreach (var employee in employees)
            {
                employee.AddressId = null;
            }

            context.SaveChanges();

            context.Addresses.RemoveRange(addresses);

            context.SaveChanges();

            context.Towns.Remove(town);

            context.SaveChanges();

            return $"{deletedAddressesCount} addresses in Seattle were deleted";
        }
    }
}