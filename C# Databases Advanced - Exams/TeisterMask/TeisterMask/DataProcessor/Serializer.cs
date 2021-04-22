namespace TeisterMask.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using TeisterMask.DataProcessor.ExportDto;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {
            var projects = context.Projects
                .ToList()
                .Where(p => p.Tasks.Count > 0)
                .Select(p => new ProjectXmlExportModel
                {
                    ProjectName = p.Name,
                    TasksCount = p.Tasks.Count,
                    HasEndDate = p.DueDate.HasValue ? "Yes" : "No",
                    Tasks = p.Tasks.Select(t => new TaskXmlExportModel
                    {
                        Name = t.Name,
                        Label = t.LabelType.ToString()
                    })
                    .OrderBy(x => x.Name)
                    .ToList()
                })
                .OrderByDescending(x => x.TasksCount)
                .ThenBy(x => x.ProjectName)
                .ToList();

            var serializer = new XmlSerializer(typeof(List<ProjectXmlExportModel>), new XmlRootAttribute("Projects"));

            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            var sb = new StringBuilder();
            var stream = new StringWriter(sb);
            serializer.Serialize(stream, projects, ns);


            return sb.ToString().TrimEnd();
        }

        public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
        {
            var employees = context.Employees
                .ToList()
                .Select(x => new EmployeeJsonExportModel
                {
                    Username = x.Username,
                    Tasks = x.EmployeesTasks
                    .Where(et => et.Task.OpenDate >= date).ToList()
                    .OrderByDescending(a => a.Task.DueDate)
                    .ThenBy(xs => xs.Task.Name)
                    .Select(t => new EmploeeTaskJsonExportModel
                    {
                        TaskName = t.Task.Name,
                        OpenDate = t.Task.OpenDate.ToString("d", CultureInfo.InvariantCulture),
                        DueDate = t.Task.DueDate.ToString("d", CultureInfo.InvariantCulture),
                        LabelType = t.Task.LabelType.ToString(),
                        ExecutionType = t.Task.ExecutionType.ToString()
                    })
                    .ToList()
                })
                .OrderByDescending(f => f.Tasks.Count)
                .ThenBy(d => d.Username)
                .Take(10)
                .ToList();

            string jsonString = JsonConvert.SerializeObject(employees, Formatting.Indented);

            return jsonString;
        }
    }
}