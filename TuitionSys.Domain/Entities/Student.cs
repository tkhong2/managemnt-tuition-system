using System;
using System.Collections.Generic;

namespace TuitionSys.Infrastructure;

public partial class Student
{
    public string StudentId { get; set; } = null!;

    public string? FullName { get; set; }

    public string? Class { get; set; }

    public string? Department { get; set; }

    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }


    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();
}
