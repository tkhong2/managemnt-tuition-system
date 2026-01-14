using System;
using System.Collections.Generic;

namespace TuitionSys.Infrastructure;

public partial class Payment
{
    public string PaymentId { get; set; } = null!;

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
