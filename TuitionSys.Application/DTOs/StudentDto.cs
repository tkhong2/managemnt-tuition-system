using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuitionSys.Application.DTOs
{
    public class StudentDto
    {
        public string StudentId { get; set; } = null!;

        public string? FullName { get; set; }

        public string? Class { get; set; }

        public string? Department { get; set; }

        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
    }
}
