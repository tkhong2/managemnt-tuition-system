using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuitionSys.Infrastructure;

namespace TuitionSys.Application.DTOs
{
    public class PaymentDto
    {
        public string PaymentId { get; set; }
        public string? StudentId { get; set; }
        public string? CourseId { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string? Status { get; set; }
        public string? StudentName { get; set; }  
        public string? CourseName { get; set; }
        public virtual Course? Course { get; set; }

        public virtual Student? Student { get; set; }
    }
}
