using Microsoft.AspNetCore.Mvc;
using TuitionSys.Application.Interfaces;

namespace TuitionSys.Web.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;

        }
        public async Task<IActionResult> Index()
        {
            var studentId = HttpContext.Session.GetString("StudentId");

            if (string.IsNullOrEmpty(studentId))
            {
                TempData["Error"] = "Không xác định được sinh viên.";
                return RedirectToAction("Index");
            }

            var allPayments = await _paymentService.GetAllPaymentsAsync();
            var studentPayments = allPayments
                .Where(p => p.StudentId == studentId)
                .OrderByDescending(p => p.PaymentDate);

            return View("Index", studentPayments);
        }
    }
}
