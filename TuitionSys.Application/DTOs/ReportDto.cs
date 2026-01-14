using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuitionSys.Application.DTOs
{
    public class ReportDto
    {
        public string StudentId { get; set; }
        public string FullName { get; set; }
        public string ClassName { get; set; }
        public string Semester { get; set; }
        public decimal TotalFee { get; set; }
        public decimal PaidFee { get; set; }
        public decimal Debt => TotalFee - PaidFee;
    }
}
