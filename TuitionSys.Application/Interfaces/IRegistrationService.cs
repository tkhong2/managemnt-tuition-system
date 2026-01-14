using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuitionSys.Application.DTOs;

namespace TuitionSys.Application.Interfaces
{
    public interface IRegistrationService
    {
        //Task<IEnumerable<PaymentDto>> GetAllRegistrationAsync();
        //Task<PaymentDto> GetRegistrationByIdAsync(string id);
        Task RegisterCourseAsync(string studentId, string courseId);
        Task<bool> IsRegisteredAsync(string studentId, string courseId);
        Task AddRegistrationAsync(RegistrationDto registrationDto);


    }
}
