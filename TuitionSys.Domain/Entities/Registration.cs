using System;
using System.Collections.Generic;

namespace TuitionSys.Infrastructure;

public partial class Registration
{
    public string RegistrationId { get; set; } = null!;

    public string? StudentId { get; set; }

    public string? CourseId { get; set; }

    public DateTime? RegistrationDate { get; set; }

    public virtual Course? Course { get; set; }

    public virtual Student? Student { get; set; }
}
