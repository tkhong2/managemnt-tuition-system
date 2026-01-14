using Microsoft.AspNetCore.Mvc;
using TuitionSys.Application.DTOs;
using TuitionSys.Application.Interfaces;
using TuitionSys.Application.Services;
using TuitionSys.Infrastructure;

namespace TuitionSys.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IStudentService studentService;
        public AccountController(IUserService userService, IStudentService studentService)
        {
            _userService = userService;
            this.studentService = studentService;
        }
        [HttpGet]
        public IActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Email and Password are required.");
                return View();
            }
            var user = await _userService.AuthenticateUserAsync(email, password);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View();
            }
            HttpContext.Session.SetString("StudentId", user.UserId);
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserRole", user.Role);


            if (user.Role == "Quản lý")
            {
                

                return RedirectToAction("Index", "Home", new { area = "Administrator" });
            }
            
            else if (user.Role == "Sinh viên")
            {
                var allStudents = await studentService.GetAllStudentAsync();
                if (!allStudents.Any())
                {
                    TempData["Error"] = "Hệ thống không tìm thấy danh sách sinh viên.";
                    return View();
                }

                var student = allStudents.FirstOrDefault(s => s.Email == user.Email);
                if (student == null)
                {
                    TempData["Error"] = "Không tìm thấy sinh viên với email: " + user.Email;
                    return View();
                }

                HttpContext.Session.SetString("StudentId", student.StudentId);
                HttpContext.Session.SetString("FullName", student.FullName);
                return RedirectToAction("Index", "Home", new { area = "Student" });
                
                
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(string email, string password, string role, string className, string phoneNumber, string department, string fullname)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Email và Mật khẩu là bắt buộc.");
                return View();
            }

            if (!await _userService.IsEmailUniqueAsync(email))
            {
                ModelState.AddModelError("", "Email đã được sử dụng.");
                return View();
            }

            role = string.IsNullOrEmpty(role) ? "Sinh viên" : role;

            var allUsers = await _userService.GetAllUsersAsync();
            int nextId = allUsers.Count() + 1;
            string newUserId = "User" + nextId;

            var userDto = new UserDto
            {
                UserId = newUserId,
                Email = email,
                Password = password,
                Role = role
            };
            var studentDto = new StudentDto
            {
                StudentId = newUserId,
                Email = email,
                Class = className,
                PhoneNumber = phoneNumber,
                Department = department,
                FullName = "" 
            };

            await studentService.AddStudentAsync(studentDto);
            await _userService.AddUserAsync(userDto);
            return RedirectToAction("Login");
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

    }
}
