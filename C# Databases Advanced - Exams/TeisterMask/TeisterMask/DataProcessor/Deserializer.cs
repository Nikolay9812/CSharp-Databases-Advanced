namespace TeisterMask.DataProcessor
{
    using System;
    using System.Collections.Generic;

    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using TeisterMask.Data.Models;
    using TeisterMask.Data.Models.Enums;
    using TeisterMask.DataProcessor.ImportDto;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedProject
            = "Successfully imported project - {0} with {1} tasks.";

        private const string SuccessfullyImportedEmployee
            = "Successfully imported employee - {0} with {1} tasks.";

        public static string ImportProjects(TeisterMaskContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(List<ProjectXmlImportModel>), new XmlRootAttribute("Projects"));

            var sb = new StringBuilder();

            using (var stream = new StringReader(xmlString))
            {
                var dto = (List<ProjectXmlImportModel>)serializer.Deserialize(stream);

                var readyProjects = new List<Project>();

                foreach (var project in dto)
                {
                    if (IsValid(project))
                    {
                        DateTime parsedOpenDate;
                        bool isValidOpenDate = DateTime.TryParseExact(project.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedOpenDate);

                        if (!isValidOpenDate)
                        {
                            sb.AppendLine(ErrorMessage);
                            continue;
                        }

                        DateTime parsedDuedate;
                        bool isValidDueDate = DateTime.TryParseExact(project.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDuedate);
                        DateTime? nullableDueDate = null;

                        if (isValidDueDate)
                        {
                            nullableDueDate = parsedDuedate;
                        }

                        var currProject = new Project
                        {
                            Name = project.Name,
                            OpenDate = parsedOpenDate,
                            DueDate = nullableDueDate
                        };

                        readyProjects.Add(currProject);

                        var listWithTasks = new List<Task>();

                        foreach (var t in project.Tasks)
                        {
                            if (IsValid(t))
                            {
                                DateTime parsedTaskOpenDate;
                                bool isValidTaskOpenDate = DateTime.TryParseExact(t.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedTaskOpenDate);

                                if (!isValidTaskOpenDate)
                                {
                                    sb.AppendLine(ErrorMessage);
                                    continue;
                                }

                                DateTime parsedTaskDueDate;
                                bool isValidTaskDueDate = DateTime.TryParseExact(t.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedTaskDueDate);

                                if (!isValidTaskDueDate)
                                {
                                    sb.AppendLine(ErrorMessage);
                                    continue;
                                }

                                if (currProject.OpenDate < parsedTaskOpenDate &&
                                    (currProject.DueDate == null || parsedTaskDueDate < currProject.DueDate))
                                {
                                    var currTask = new Task
                                    {
                                        Name = t.Name,
                                        OpenDate = parsedTaskOpenDate,
                                        DueDate = parsedTaskDueDate,
                                        ExecutionType = (ExecutionType)t.ExecutionType,
                                        LabelType = (LabelType)t.LabelType
                                    };

                                    listWithTasks.Add(currTask);
                                }
                                else
                                {
                                    sb.AppendLine(ErrorMessage);
                                }
                            }
                            else
                            {
                                sb.AppendLine(ErrorMessage);
                            }
                        }

                        currProject.Tasks = listWithTasks;

                        readyProjects.Add(currProject);
                        sb.AppendLine(string.Format(SuccessfullyImportedProject, currProject.Name, currProject.Tasks.Count));
                    }
                    else
                    {
                        sb.AppendLine(ErrorMessage);
                    }
                }

                context.Projects.AddRange(readyProjects);
                context.SaveChanges();

                return sb.ToString().TrimEnd();
            }
        }

        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {
            var dto = JsonConvert.DeserializeObject<List<EmployeJsonImportModel>>(jsonString);

            var sb = new StringBuilder();

            var readyEmployees = new List<Employee>();

            foreach (var employee in dto)
            {
                if (IsValid(employee))
                {
                    foreach (var currSymbol in employee.Username)
                    {
                        if (!char.IsLetterOrDigit(currSymbol))
                        {
                            sb.AppendLine(ErrorMessage);
                            continue;
                        }
                    }


                    var currEmployee = new Employee
                    {
                        Username = employee.Username,
                        Email = employee.Email,
                        Phone = employee.Phone
                    };

                    var uniqueTasks = employee.Tasks.Distinct();

                    foreach (var taskId in uniqueTasks)
                    {
                        var currTask = context.Tasks.FirstOrDefault(t => t.Id == taskId);

                        if (currTask != null)
                        {
                            currEmployee.EmployeesTasks.Add(new EmployeeTask
                            {
                                Employee = currEmployee,
                                Task = currTask
                            });
                        }
                        else
                        {
                            sb.AppendLine(ErrorMessage);
                        }
                    }

                    readyEmployees.Add(currEmployee);
                    sb.AppendLine(string.Format(SuccessfullyImportedEmployee, currEmployee.Username, currEmployee.EmployeesTasks.Count));
                }
                else
                {
                    sb.AppendLine(ErrorMessage);
                }
            }

            context.Employees.AddRange(readyEmployees);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}