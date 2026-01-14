using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuitionSys.Application.DTOs;
using TuitionSys.Application.Interfaces;
using TuitionSys.Domain.Interfaces;
using TuitionSys.Infrastructure;

namespace TuitionSys.Application.Services
{
    public class CourseService : ICourseService
    {
        private readonly IRepository<Course> _courseRepository;
        private readonly IRepository<Registration> _registrationRepository;
        private readonly IRepository<Payment> _paymentRepository;

        public CourseService(
            IRepository<Course> courseRepository,
            IRepository<Registration> registrationRepository,
            IRepository<Payment> paymentRepository)
        {
            _courseRepository = courseRepository;
            _registrationRepository = registrationRepository;
            _paymentRepository = paymentRepository;
        }

        public async Task<IEnumerable<CourseDto>> GetAllCourseAsync()
        {
            var courses = await _courseRepository.GetAllAsync();
            return courses.Select(s => new CourseDto
            {
                CourseId = s.CourseId,
                CourseName = s.CourseName,
                Credits = s.Credits,
                Price = s.Price,
                Semester = s.Semester,

                StartDate = s.StartDate,
                EndDate = s.EndDate
            });
        }
        public async Task<CourseDto> GetCourseByIdAsync(string id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null) return null;
            return new CourseDto
            {
                CourseId = course.CourseId,
                CourseName = course.CourseName,
                Credits = course.Credits,
                Price = course.Price,
                Semester = course.Semester,

                StartDate = course.StartDate,
                EndDate = course.EndDate
            };
        }
        public async Task AddCourseAsync(CourseDto courseDto)
        {
            var course = new Course
            {
                CourseId = courseDto.CourseId,
                CourseName = courseDto.CourseName,
                Credits = courseDto.Credits,
                Price = courseDto.Price,
                Semester = courseDto.Semester,

                StartDate = courseDto.StartDate,
                EndDate = courseDto.EndDate
            };
            await _courseRepository.AddAsync(course);
            await _courseRepository.SaveChangesAsync();
        }
        public async Task UpdateCourseAsync(CourseDto courseDto)
        {
            var course = new Course()

            {
                CourseId = courseDto.CourseId,
                CourseName = courseDto.CourseName,
                Credits = courseDto.Credits,
                Price = courseDto.Price,
                Semester = courseDto.Semester,
                StartDate = courseDto.StartDate,
                EndDate = courseDto.EndDate

            };
            _courseRepository.Update(course);
            await _courseRepository.SaveChangesAsync();
        }

        public async Task DeleteCourseAsync(string courseId)
        {
            var registrations = await _registrationRepository.FindAsync(r => r.CourseId == courseId);
            var payments = await _paymentRepository.FindAsync(p => p.CourseId == courseId);

            _registrationRepository.RemoveRange(registrations);
            _paymentRepository.RemoveRange(payments);

            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course != null)
            {
                _courseRepository.Delete(course);
            }

            await _courseRepository.SaveChangesAsync();
        }



    }
}
