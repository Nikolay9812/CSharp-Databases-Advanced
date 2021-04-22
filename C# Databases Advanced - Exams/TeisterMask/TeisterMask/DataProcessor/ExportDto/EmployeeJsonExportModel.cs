using System;
using System.Collections.Generic;
using System.Text;

namespace TeisterMask.DataProcessor.ExportDto
{
    public class EmployeeJsonExportModel
    {
        public string Username { get; set; }

        public ICollection<EmploeeTaskJsonExportModel> Tasks { get; set; }
    }
}
