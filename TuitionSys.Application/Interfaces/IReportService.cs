using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuitionSys.Application.DTOs;

namespace TuitionSys.Application.Interfaces
{
    public interface IReportService
    {
        Task<IEnumerable<ReportDto>> GetTuitionReportAsync(string semester, string status);
    }
}
