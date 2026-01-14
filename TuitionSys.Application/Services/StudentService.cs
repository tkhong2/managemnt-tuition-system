using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TuitionSys.Application.DTOs;
using TuitionSys.Application.Interfaces;
using TuitionSys.Domain.Interfaces;
using TuitionSys.Infrastructure;

namespace TuitionSys.Application.Services
{
    public class StudentService : IStudentService
    {
        private readonly IRepository<Student> _studentRepository;
        private readonly IRepository<Registration> _registrationRepository;
        private readonly IRepository<Payment> _paymentRepository;

        public StudentService(
            IRepository<Student> studentRepository,
            IRepository<Registration> registrationRepository,
            IRepository<Payment> paymentRepository)

        {
            _registrationRepository = registrationRepository;
            _paymentRepository = paymentRepository;
            _studentRepository = studentRepository;
        }



        public async Task<IEnumerable<StudentDto>> GetAllStudentAsync()
        {
            var students = await _studentRepository.GetAllAsync();
            return students.Select(s => new StudentDto()
            {
                StudentId = s.StudentId,
                FullName = s.FullName,
                Class = s.Class,
                Department = s.Department,
                PhoneNumber = s.PhoneNumber,
                Email = s.Email
            });
        }

        public async Task<StudentDto> GetStudentByIdAsync(string id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null) return null;
            return new StudentDto()
            {
                StudentId = student.StudentId,
                FullName = student.FullName,
                Class = student.Class,
                Department = student.Department,
                PhoneNumber = student.PhoneNumber,
                Email = student.Email
            };
        }

        public async Task AddStudentAsync(StudentDto studentDto)
        {
            var student = new Student()
            {
                StudentId = studentDto.StudentId,
                FullName = studentDto.FullName,
                Class = studentDto.Class,
                Department = studentDto.Department,
                PhoneNumber = studentDto.PhoneNumber,
                Email = studentDto.Email
            };
            await _studentRepository.AddAsync(student);
            await _studentRepository.SaveChangesAsync();
        }

        public async Task UpdateStudentAsync(StudentDto studentDto)
        {
            var student = new Student()
            {
                StudentId = studentDto.StudentId,
                FullName = studentDto.FullName,
                Class = studentDto.Class,
                Department = studentDto.Department,
                PhoneNumber = studentDto.PhoneNumber,
                Email = studentDto.Email
            };
            _studentRepository.Update(student);
            await _studentRepository.SaveChangesAsync();
        }

        public async Task DeleteStudentAsync(string id)
        {
            var registrations = await _registrationRepository.FindAsync(r => r.StudentId == id);

            var payments = await _paymentRepository.FindAsync(p => p.StudentId == id);

            _registrationRepository.RemoveRange(registrations);
            _paymentRepository.RemoveRange(payments);
            var student = await _studentRepository.GetByIdAsync(id);
            if (student != null)
            {
                _studentRepository.Delete(student);
                await _studentRepository.SaveChangesAsync();
            }
        }
        


    }
}
