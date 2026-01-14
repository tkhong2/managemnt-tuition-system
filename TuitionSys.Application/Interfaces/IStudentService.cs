using System.Collections.Generic;
using System.Threading.Tasks;
using TuitionSys.Application.DTOs;

namespace TuitionSys.Application.Interfaces
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentDto>> GetAllStudentAsync();
        Task<StudentDto> GetStudentByIdAsync(string id);
        Task AddStudentAsync(StudentDto studentDto);
        Task UpdateStudentAsync(StudentDto studentDto);
        Task DeleteStudentAsync(string id);

    }
}
