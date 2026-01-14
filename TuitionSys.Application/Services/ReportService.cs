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
    public class ReportService : IReportService
    {
        private readonly IRepository<Payment> _paymentRepo;
        private readonly IRepository<Student> _studentRepo;
        private readonly IRepository<Course> _courseRepo;

        public ReportService(
            IRepository<Payment> paymentRepo,
            IRepository<Student> studentRepo,
            IRepository<Course> courseRepo)
        {
            _paymentRepo = paymentRepo;
            _studentRepo = studentRepo;
            _courseRepo = courseRepo;
        }

        public async Task<IEnumerable<ReportDto>> GetTuitionReportAsync(string semester, string status)
        {
            var students = await _studentRepo.GetAllAsync();
            var payments = await _paymentRepo.GetAllAsync();
            var courses = await _courseRepo.GetAllAsync();

            var query = from s in students
                        join p in payments on s.StudentId equals p.StudentId
                        join c in courses on p.CourseId equals c.CourseId
                        where semester == null || c.Semester == semester
                        group new { s, p, c } by new { s.StudentId, s.FullName, s.Class } into payGroup
                        select new ReportDto
                        {
                            StudentId = payGroup.Key.StudentId,
                            FullName = payGroup.Key.FullName,
                            ClassName = payGroup.Key.Class,

                            TotalFee = payGroup.Select(x => new { x.c.CourseId, x.c.Price })
                                               .Distinct()
                                               .Sum(x => x.Price ?? 0),

                            PaidFee = payGroup.Sum(x => x.p.Amount ?? 0)
                        };

            var result = query.ToList();

            if (status == "\u0110\u00e3 \u0111\u00f3ng")
            {
                result = result.Where(r => r.TotalFee - r.PaidFee == 0).ToList();
            }
            else if (status == "Ch\u01b0a \u0111\u00f3ng")
            {
                result = result.Where(r => r.TotalFee - r.PaidFee > 0).ToList();
            }

            return result;
        }
    }
}
