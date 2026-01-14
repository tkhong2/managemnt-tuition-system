using System;
using System.Collections.Generic;

namespace TuitionSys.Infrastructure;

public partial class Course
{
    public string CourseId { get; set; } = null!;

    public string CourseName { get; set; } = null!;

    public int Credits { get; set; }

    public decimal? Price { get; set; }

    public string? Semester { get; set; }


    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();
}
