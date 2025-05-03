using ACBackendAPI.Application.Common.Responses;
using ACBackendAPI.Application.Dtos;

namespace ACBackendAPI.Application.Interfaces.IServices
{
    public interface IProgrammeService
    {
        Task<BaseResponse<ProgrammeDto>> CreateProgramme(CreateProgrammeDto createProgrammeDto);
        Task<BaseResponse<List<ProgrammeDto>>> GetAllProgrammes();
        Task<BaseResponse<ProgrammeDto>> GetProgrammeById(Guid id);
        Task<BaseResponse<ProgrammeDto>> UpdateProgramme(UpdateProgrammeDto updateProgrammeDto);
        Task<BaseResponse<bool>> DeleteProgramme(Guid id);
    }
}