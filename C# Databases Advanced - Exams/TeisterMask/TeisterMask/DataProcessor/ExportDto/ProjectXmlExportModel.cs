using System.Collections.Generic;
using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ExportDto
{
    [XmlType("Project")]
    public class ProjectXmlExportModel
    {
        [XmlAttribute("TasksCount")]
        public int TasksCount { get; set; }

        public string ProjectName { get; set; }

        public string HasEndDate { get; set; }

        [XmlArray("Tasks")]
        public List<TaskXmlExportModel> Tasks { get; set; }


    }
}
