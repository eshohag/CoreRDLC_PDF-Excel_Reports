using AspNetCore.Reporting;
using CoreRDLC.Entity;
using CoreRDLC.Extension;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CoreRDLC_Reports.Controllers
{
    public class ReportController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ReportController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Print()
        {
            var studentsData = GetStudentList();
            string mimetype = "";
            int extension = 1;
            var path = $"{this._webHostEnvironment.WebRootPath}\\Reports\\Designer\\StudentReport.rdlc";
            var header = new ReportHeader()
            {
                Logo = $"{this._webHostEnvironment.WebRootPath}\\Images\\Logo\\logo.png",
                Title = "Student Information by RDLC Reports"
            };
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            //parameters.Add("title", "Student Information by RDLC Report");
            LocalReport lr = new LocalReport(path);

            lr.AddDataSource("ReportHeader", header.ToDataTable());
            lr.AddDataSource("Students", studentsData);

            var result = lr.Execute(RenderType.Pdf, extension, parameters, mimetype);
            return File(result.MainStream, "application/pdf");
        }

        public IActionResult Export()
        {
            var studentsData = GetStudentList();
            string mimetype = "";
            int extension = 1;
            var path = $"{this._webHostEnvironment.WebRootPath}/Reports/Designer/StudentReport.rdlc";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("title", "Student Information by RDLC Report");
            LocalReport lr = new LocalReport(path);
            lr.AddDataSource("Students", studentsData);
            var result = lr.Execute(RenderType.Excel, extension, parameters, mimetype);
            return File(result.MainStream, "application/msexcel", "Export.xls");
        }

        private List<Student> GetStudentList()
        {
            var students = new List<Student>();

            for (int i = 1; i < 100; i++)
            {
                var aStudent = new Student()
                {
                    Id = i,
                    Name = "StudentName-" + i,
                    Email = "studentemail" + i + "@gmail.com"
                };
                students.Add(aStudent);
            }
            return students;
        }
    }
}
