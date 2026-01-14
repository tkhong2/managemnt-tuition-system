using Microsoft.AspNetCore.Mvc;
using TuitionSys.Application.DTOs;
using TuitionSys.Application.Interfaces;
using TuitionSys.Application.Services;

namespace TuitionSys.Web.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }
        public async Task<IActionResult> Index()
        {
            var courses = await _courseService.GetAllCourseAsync();
            return View(courses);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CourseDto courseDto)
        {
            if (ModelState.IsValid)
            {
                await _courseService.AddCourseAsync(courseDto);

                return RedirectToAction("Index");
            }
            return View(courseDto);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CourseDto courseDto)
        {
            if (ModelState.IsValid)
            {
                await _courseService.UpdateCourseAsync(courseDto);
                return RedirectToAction("Index");
            }
            return View(courseDto);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            await _courseService.DeleteCourseAsync(id);
            return RedirectToAction("Index");
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
                        filteredCourses = allCourses.Where(c => c.Semester != null && c.Semester.Contains("1"));
                        break;
                    case "hk2":
                        filteredCourses = allCourses.Where(c => c.Semester != null && c.Semester.Contains("2"));
                        break;
                    default:
                        break;
                }
            }

            return View("Index", filteredCourses);
        }

    }
}
