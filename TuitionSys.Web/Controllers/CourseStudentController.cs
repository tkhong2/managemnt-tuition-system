using Microsoft.AspNetCore.Mvc;
using TuitionSys.Application.DTOs;
using TuitionSys.Application.Interfaces;

namespace TuitionSys.Web.Controllers
{
    public class CourseStudentController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly IRegistrationService _registrationService;
        private readonly IStudentService _studentService;
        private readonly IPaymentService _paymentService;

        public CourseStudentController(
            ICourseService courseService,
            IRegistrationService registrationService,
            IStudentService studentService,
            IPaymentService paymentService)
        {
            _courseService = courseService;
            _registrationService = registrationService;
            _studentService = studentService;
            _paymentService = paymentService;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _courseService.GetAllCourseAsync();
            return View(courses);
        }

        [HttpGet]
        public async Task<IActionResult> Sort(string sortOption)
        {
            var courses = await _courseService.GetAllCourseAsync();

            switch (sortOption)
            {
                case "asc":
                    courses = courses.OrderBy(c => c.Price ?? 0);
                    break;
                case "desc":
                    courses = courses.OrderByDescending(c => c.Price ?? 0);
                    break;
                case "gte1m":
                    courses = courses.Where(c => (c.Price ?? 0) >= 1_000_000);
                    break;
                case "gte5m":
                    courses = courses.Where(c => (c.Price ?? 0) >= 5_000_000);
                    break;
            }

            return View("Index", courses);
        }

        [HttpGet]
        public async Task<IActionResult> Sort2(string sortOption)
        {
            var allCourses = await _courseService.GetAllCourseAsync();
            IEnumerable<CourseDto> filteredCourses = allCourses;

            if (!string.IsNullOrEmpty(sortOption))
            {
                switch (sortOption)
                {
                    case "hk1":
                        filteredCourses = allCourses.Where(c => c.Semester?.Contains("1") == true);
                        break;
                    case "hk2":
                        filteredCourses = allCourses.Where(c => c.Semester?.Contains("2") == true);
                        break;
                }
            }

            return View("Index", filteredCourses);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterCourse(string courseId)
        {
            var studentId = HttpContext.Session.GetString("StudentId");
            if (string.IsNullOrEmpty(studentId))
            {
                TempData["Error"] = "Không thể xác định sinh viên.";
                return RedirectToAction("Index");
            }

            var studentInfo = (await _studentService.GetAllStudentAsync())
                .FirstOrDefault(s => s.StudentId == studentId);

            var course = await _courseService.GetCourseByIdAsync(courseId);

            if (studentInfo == null || course == null)
            {
                TempData["Error"] = "Không thể đăng ký học phần. Dữ liệu không hợp lệ.";
                return RedirectToAction("Index");
            }

            // Kiểm tra đã đăng ký chưa
            if (await _registrationService.IsRegisteredAsync(studentInfo.StudentId, course.CourseId))
            {
                TempData["Error"] = "Bạn đã đăng ký học phần này rồi.";
                return RedirectToAction("Index");
            }

            var registrationId = Guid.NewGuid().ToString().Substring(0, 10);
            var registrationDto = new RegistrationDto
            {
                RegistrationId = registrationId,
                StudentId = studentInfo.StudentId,
                CourseId = course.CourseId,
                RegistrationDate = DateTime.Now
            };
            await _registrationService.AddRegistrationAsync(registrationDto);
            var paymentId = "PAY" + new Random().Next(10000, 99999);

            // Ghi thông tin giao dịch
            var payment = new PaymentDto
            {
                PaymentId = paymentId,
                StudentId = studentInfo.StudentId,
                StudentName = studentInfo.FullName,
                CourseId = course.CourseId,
                CourseName = course.CourseName,
                Amount = course.Price ?? 0,
                PaymentDate = DateTime.Now,
                Status = "Đã thanh toán"
            };
            await _paymentService.InsertAsync(payment);

            return View("RegistrationResult", registrationDto);
        }
    }
}
