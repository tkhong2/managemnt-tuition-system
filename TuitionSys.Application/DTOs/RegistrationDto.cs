using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuitionSys.Infrastructure;

namespace TuitionSys.Application.DTOs
{
    public class RegistrationDto
    {
        [MaxLength(50)]
        public string RegistrationId { get; set; } = null!;

        public string? StudentId { get; set; }

        public string? CourseId { get; set; }

        public DateTime? RegistrationDate { get; set; }

        public virtual Course? Course { get; set; }

        public virtual Student? Student { get; set; }
    }
}
