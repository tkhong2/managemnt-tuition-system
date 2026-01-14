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
    public class RegistrationService : IRegistrationService
    {
        private readonly IRepository<Registration> _registrationRepo;

        public RegistrationService(IRepository<Registration> registrationRepo)
        {
            _registrationRepo = registrationRepo;
        }

        public async Task AddRegistrationAsync(RegistrationDto registrationDto)
        {
            
            var registration = new Registration
            {
                RegistrationId = registrationDto.RegistrationId,
                StudentId = registrationDto.StudentId,
                CourseId = registrationDto.CourseId,
                RegistrationDate = registrationDto.RegistrationDate
            };

            await _registrationRepo.AddAsync(registration);
            await _registrationRepo.SaveChangesAsync();
        }

        public async Task RegisterCourseAsync(string studentId, string courseId)
        {
            var existing = await _registrationRepo.GetAsync(r => r.StudentId == studentId && r.CourseId == courseId);
            if (existing != null) return;

            var registrationDto = new RegistrationDto
            {
                RegistrationId = Guid.NewGuid().ToString(),
                StudentId = studentId,
                CourseId = courseId,
                RegistrationDate = DateTime.Now
            };

            await AddRegistrationAsync(registrationDto);
        }

        public async Task<bool> IsRegisteredAsync(string studentId, string courseId)
        {
            var existing = await _registrationRepo.GetAsync(r => r.StudentId == studentId && r.CourseId == courseId);
            return existing != null;
        }
    }

}
