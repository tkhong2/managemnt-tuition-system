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
    public class PaymentService : IPaymentService
    {
        private readonly IRepository<Payment> _paymentRepository;
        private readonly IRepository<Student> _studentRepository;
        private readonly IRepository<Course> _courseRepository;
        public PaymentService(IRepository<Payment> paymentRepository, IRepository<Student> studentRepository, IRepository<Course> courseRepository)
        {
            _paymentRepository = paymentRepository;
            _studentRepository = studentRepository;
            _courseRepository = courseRepository;
        }
        public async Task<IEnumerable<PaymentDto>> GetAllPaymentsAsync()
        {
            var payments = await _paymentRepository.GetAllAsync();
            return payments.Select(p => new PaymentDto()
            {
                PaymentId = p.PaymentId,
                
               
                StudentId = p.StudentId,
                CourseId = p.CourseId,Amount = p.Amount, PaymentDate = p.PaymentDate,
                Status = p.Status,
                CourseName = p.CourseName,
                StudentName = p.StudentName,

            });

        }
        public async Task<PaymentDto> GetPaymentByIdAsync(string id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);
            if (payment == null) return null;

            // Lấy thông tin sinh viên và học phần
            var student = await _studentRepository.GetByIdAsync(payment.StudentId);
            var course = await _courseRepository.GetByIdAsync(payment.CourseId);

            return new PaymentDto
            {
                PaymentId = payment.PaymentId,
                StudentId = payment.StudentId,
                CourseId = payment.CourseId,
                Amount = payment.Amount,
                PaymentDate = payment.PaymentDate,
                Status = payment.Status,
                StudentName = student?.FullName,
                CourseName = course?.CourseName
            };
        }
        public async Task AddPaymentAsync(PaymentDto paymentDto)
        {

            var payment = new Payment
            {
                PaymentId = Guid.NewGuid().ToString(),
                StudentId = paymentDto.StudentId,
                CourseId = paymentDto.CourseId,
                CourseName = paymentDto.CourseName,
                Amount = paymentDto.Amount,
                PaymentDate = DateTime.Now,
                Status = "Chưa thanh toán"
            };
            await _paymentRepository.InsertAsync(payment);
        }

        public async Task InsertAsync(PaymentDto payment)
        {
            var entity = new Payment
            {
                PaymentId = payment.PaymentId,
                StudentId = payment.StudentId,
                CourseName = payment.CourseName,
                CourseId = payment.CourseId,
                Amount = payment.Amount ?? 0,
                PaymentDate = payment.PaymentDate ?? DateTime.Now,
                Status = payment.Status,
                StudentName = payment.StudentName,
                
            };

            await _paymentRepository.AddAsync(entity);
            await _paymentRepository.SaveChangesAsync();
        }

      

    }
}
