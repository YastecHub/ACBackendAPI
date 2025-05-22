using ACBackendAPI.Application.Common.Responses;
using ACBackendAPI.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACBackendAPI.Application.Interfaces.IServices
{
    public interface IStudentService
    {
        Task<BaseResponse<StudentDto>> CreateStudentAsync(CreateStudentDto studentDto);
        Task<BaseResponse<StudentDto>> GetStudentByIdAsync(Guid studentId);
        Task<BaseResponse<List<StudentDto>>> ListStudentsAsync();
        Task<BaseResponse<StudentDto>> UpdateStudentAsync(Guid studentId, StudentDto studentDto);
        Task<BaseResponse<bool>> DeleteStudentAsync(Guid studentId);
    }
}
