using Microsoft.AspNetCore.Mvc;
using TuitionSys.Application.Interfaces;

namespace TuitionSys.Web.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        public async Task<IActionResult> Index(string semester, string status)
        {
            var reportList = await _reportService.GetTuitionReportAsync(semester, status);

            ViewBag.TotalStudents = reportList.Count();
            ViewBag.TotalPaid = reportList.Sum(r => r.PaidFee);
            ViewBag.TotalFee = reportList.Sum(r => r.TotalFee);
            ViewBag.TotalDebt = reportList.Sum(r => r.TotalFee - r.PaidFee);
            ViewBag.Status = status;

            return View(reportList);
        }
    }
}
