using ACBackendAPI.Application.Common.Responses;
using ACBackendAPI.Application.Dtos;

namespace ACBackendAPI.Application.Interfaces.IServices
{
    public interface IAuthService
    {
        Task<BaseResponse<AdminRegistrationDto>> RegisterAdmin(AdminRegistrationDto adminDto);
        Task<BaseResponse<StudentRegistrationDto>> RegisterStudent(StudentRegistrationDto studentDto);
        Task<BaseResponse<LoginResponseDto>> Login(LoginDto loginDto);
    }
}
