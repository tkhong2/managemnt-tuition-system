using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuitionSys.Application.DTOs
{
    public class CourseDto
    {
        public string CourseId { get; set; } = null!;

        public string CourseName { get; set; } = null!;

        public int Credits { get; set; }

        public decimal? Price { get; set; }

        public string? Semester { get; set; }



        public DateOnly? StartDate { get; set; }

        public DateOnly? EndDate { get; set; }
    }
}
