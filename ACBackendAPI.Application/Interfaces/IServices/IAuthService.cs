using ACBackendAPI.Application.Common.Responses;
using ACBackendAPI.Application.Dtos;

namespace ACBackendAPI.Application.Interfaces.IServices
{
    public interface IAuthService
    {
        Task<BaseResponse<AdminDto>> RegisterAdmin(AdminDto adminDto);
        Task<BaseResponse<StudentDto>> RegisterStudent(StudentDto studentDto);
        Task<BaseResponse<LoginResponseDto>> Login(LoginDto loginDto);
    }
}
