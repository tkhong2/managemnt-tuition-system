using Microsoft.AspNetCore.Mvc;
using TuitionSys.Application.DTOs;
using TuitionSys.Application.Interfaces;

namespace TuitionSys.Web.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public async Task<IActionResult> Index()
        {
            var students = await _studentService.GetAllStudentAsync();
            return View(students);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(StudentDto studentDto)
        {
            if (ModelState.IsValid)
            {
                await _studentService.AddStudentAsync(studentDto);
                return RedirectToAction("Index");
            }
            return View(studentDto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(StudentDto studentDto)
        {
            if (ModelState.IsValid)
            {
                await _studentService.UpdateStudentAsync(studentDto);
                return RedirectToAction("Index");
            }
            return View(studentDto);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            await _studentService.DeleteStudentAsync(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Search(string search)
        {
            var students = await _studentService.GetAllStudentAsync();
            if (!string.IsNullOrEmpty(search))
            {
                students = students
                    .Where(s => !string.IsNullOrEmpty(s.FullName) && s.FullName.Contains(search, StringComparison.OrdinalIgnoreCase));
            }
            return View("Index", students);
        }
    }
}
