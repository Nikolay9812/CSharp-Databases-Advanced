using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            var context = new SoftUniContext();

            var result = GetEmployeesByFirstNameStartingWithSa(context);

            Console.WriteLine(result);
        }

        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context
                .Employees
                .OrderBy(e => e.EmployeeId)
                .ToList();

            foreach (var employee in employees)
            {
                sb
                    .AppendLine($"{employee.FirstName} {employee.LastName} {employee.MiddleName} {employee.JobTitle} {employee.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context
                .Employees
                .Where(e => e.Salary > 50000)
                .OrderBy(e => e.FirstName)
                .ToList();

            foreach (var employee in employees)
            {
                sb
                    .AppendLine($"{employee.FirstName} - {employee.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var emploees = context
                .Employees
                .Where(e => e.Department.Name == "Research and Development")
                .Select(e=> new 
                { 
                    e.FirstName,
                    e.LastName,
                    DepartmentName = e.Department.Name,
                    e.Salary
                })
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName)
                .ToList();

            foreach (var emploee in emploees)
            {
                sb
                    .AppendLine($"{emploee.FirstName} {emploee.LastName} from {emploee.DepartmentName} - ${emploee.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var address = new Address
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            var employee = context
                .Employees
                .First(e => e.LastName == "Nakov");

            context.Addresses.Add(address);
            employee.Address = address;
            context.SaveChanges();

            var adresses = context
                .Employees
                .OrderByDescending(e => e.AddressId)
                .Take(10)
                .Select(e => e.Address.AddressText)
                .ToList();

            foreach (var adress in adresses)
            {
                sb
                    .AppendLine(adress);
            }

            return sb.ToString().TrimEnd();

        }

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                                   .Where(p => p.EmployeesProjects.Any(s => s.Project.StartDate.Year >= 2001 && s.Project.StartDate.Year <= 2003))
                                   .Select(e => new
                                   {
                                       EmployeeFullName = e.FirstName + " " + e.LastName,
                                       ManagerFullName = e.Manager.FirstName + " " + e.Manager.LastName,
                                       Projects = e.EmployeesProjects.Select(p => new
                                       {
                                           ProjectName = p.Project.Name,
                                           StartDate = p.Project.StartDate,
                                           EndDate = p.Project.EndDate
                                       }).ToList()
                                   })
                                   .Take(10)
                                   .ToList();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.EmployeeFullName} - Manager: {employee.ManagerFullName}");

                foreach (var project in employee.Projects)
                {
                    var startDate = project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                    var endDate = project.EndDate.HasValue ? project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
                                  : "not finished";

                    sb.AppendLine($"--{project.ProjectName} - {startDate} - {endDate}");
                }
            }

            return sb.ToString().Trim();
        }

        public static string GetAddressesByTown(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context
                .Addresses
                .OrderByDescending(a => a.Employees.Count)
                .ThenBy(a => a.Town.Name)
                .Take(10)
                .Select(e=>new 
                {
                    e.AddressText,
                    TownName=e.Town.Name,
                    EmployeesCount = e.Employees.Count
                })
                .ToList();

            foreach (var employee in employees)
            {
                sb
                    .AppendLine($"{employee.AddressText}, {employee.TownName} - {employee.EmployeesCount} employees");
            }


            return sb.ToString().TrimEnd();
        }

        public static string GetEmployee147(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employee = context
                .Employees
                .Where(e => e.EmployeeId == 147)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    Projects = e.EmployeesProjects
                        .Select(ep => ep.Project.Name)
                        .OrderBy(ep => ep)
                        .ToList()
                })
                .First();

            sb
                .AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");

            foreach (var project in employee.Projects)
            {
                sb
                    .AppendLine(project);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var departments = context
                .Departments
                .Where(e => e.Employees.Count > 5)
                .OrderBy(e => e.Employees.Count)
                .ThenBy(d => d.Name)
                .Select(d => new
                {
                    DepartmentName = d.Name,
                    ManagerFirstName = d.Manager.FirstName,
                    ManagerLastName = d.Manager.LastName,
                    Employees = d.Employees
                        .OrderBy(e => e.FirstName)
                        .ThenBy(e => e.LastName)
                        .ToList()
                })
                .ToList();

            foreach (var department in departments)
            {
                sb
                    .AppendLine($"{department.DepartmentName} – {department.ManagerFirstName} {department.ManagerLastName}");

                foreach (var employee in department.Employees)
                {
                    sb
                        .AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetLatestProjects(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var projects = context
                .Projects
                .OrderByDescending(p => p.StartDate)
                .Take(10)
                .OrderBy(p => p.Name)
                .ToList();

            foreach (var project in projects)
            {
                sb
                    .AppendLine(project.Name);
                sb
                    .AppendLine(project.Description);
                sb
                    .AppendLine(project.StartDate.ToString("M/d/yyyy h:mm:ss tt",CultureInfo.InvariantCulture));
            }

            return sb.ToString().TrimEnd();
        }

        public static string IncreaseSalaries(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            IQueryable<Employee> employees = context
                .Employees
                .Where(d => d.Department.Name == "Engineering" ||
                          d.Department.Name == "Tool Design" ||
                          d.Department.Name == "Marketing" ||
                          d.Department.Name == "Information Services");

            foreach (Employee currEmployee in employees)
            {
                currEmployee.Salary += currEmployee.Salary * 0.12m;
            }

            context.SaveChanges();

            var employeeInfo = employees
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToList();

            foreach (var employee in employeeInfo)
            {
                sb
                    .AppendLine($"{employee.FirstName} {employee.LastName} (${employee.Salary:f2})");
            }
                
            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                                   .Where(f => f.FirstName.StartsWith("Sa"))
                                   .OrderBy(f => f.FirstName)
                                   .ThenBy(f => f.LastName)
                                   .ToList();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle} - (${employee.Salary:F2})");
            }


            return sb.ToString().Trim();
        }

        public static string DeleteProjectById(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var project = context.Projects.FirstOrDefault(x => x.ProjectId == 2);

            var employeeProjects = context.EmployeesProjects.Where(x => x.ProjectId == 2);

            context.EmployeesProjects.RemoveRange(employeeProjects);

            context.Projects.Remove(project);

            context.SaveChanges();

            var projects = context.Projects
                                  .Select(x => x.Name)
                                  .Take(10)
                                  .ToList();

            foreach (var currentProject in projects)
            {
                sb.AppendLine(currentProject);
            }

            return sb.ToString().Trim();
        }

        public static string RemoveTown(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            string townName = "Seattle";

            context.Employees
                .Where(e => e.Address.Town.Name == townName)
                .ToList()
                .ForEach(e => e.AddressId = null);

            int addressesCount = context.Addresses
                .Where(a => a.Town.Name == townName)
                .Count();

            context.Addresses
                .Where(a => a.Town.Name == townName)
                .ToList()
                .ForEach(a => context.Addresses.Remove(a));

            context.Towns
                .Remove(context.Towns
                    .SingleOrDefault(t => t.Name == townName));

            context.SaveChanges();

            sb.AppendLine($"{addressesCount} {(addressesCount == 1 ? "address" : "addresses")} in {townName} {(addressesCount == 1 ? "was" : "were")} deleted");

            return sb.ToString().Trim();
        }
    }
}
