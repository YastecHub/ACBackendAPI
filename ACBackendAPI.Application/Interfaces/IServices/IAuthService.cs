using ACBackendAPI.Application.Common.Responses.ACBackendAPI.Application.Common.Responses;
using ACBackendAPI.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACBackendAPI.Application.Interfaces.IServices
{
    public interface IAuthService
    {
        Task<BaseResponse<AdminDto>> RegisterAdmin(AdminDto adminDto);
        Task<BaseResponse<StudentDto>> RegisterStudent(StudentDto studentDto);
        Task<BaseResponse<LoginResponseDto>> Login(LoginDto loginDto);
    }
}
