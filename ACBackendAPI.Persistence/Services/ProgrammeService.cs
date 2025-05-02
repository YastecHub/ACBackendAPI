using ACBackendAPI.Application.Common.Responses.ACBackendAPI.Application.Common.Responses;
using ACBackendAPI.Application.Dtos;
using ACBackendAPI.Application.Interfaces.IRepositories;
using ACBackendAPI.Application.Interfaces.IServices;
using ACBackendAPI.Domain.Entities;

namespace ACBackendAPI.Persistence.Services
{
    public class ProgrammeService : IProgrammeService
    {
        private readonly IAsyncRepository<Programme, Guid> _programmeRepository;

        public ProgrammeService(IAsyncRepository<Programme, Guid> programmeRepository)
        {
            _programmeRepository = programmeRepository;
        }
        public async Task<BaseResponse<ProgrammeDto>> CreateProgramme(CreateProgrammeDto createProgrammeDto)
        {
            var programme = new Programme
            {
                Id = Guid.NewGuid(),
                ProgrammeName = createProgrammeDto.ProgrammeName,
                ProgrammeFee = createProgrammeDto.ProgrammeFee
            };

            await _programmeRepository.AddAsync(programme);
            await _programmeRepository.SaveChangesAsync();

            return new BaseResponse<ProgrammeDto>
            {
                Data = new ProgrammeDto
                {
                    Id = programme.Id,
                    ProgrammeName = programme.ProgrammeName,
                    ProgrammeFee = programme.ProgrammeFee
                },
                Success = true,
                Message = "Programme created successfully"
            };
        }

        public async Task<BaseResponse<bool>> DeleteProgramme(Guid id)
        {
            var programme = await _programmeRepository.GetByIdAsync(id);
            if (programme == null)
                return new BaseResponse<bool>
                {
                    Success = false,
                };

            _programmeRepository.Delete(programme);
            await _programmeRepository.SaveChangesAsync();
            return new BaseResponse<bool>
            {
                Success = true,
                Message = "Programme deleted successfully",
                StatusCode = 200,
                Data = true
            };
        }

        public async Task<BaseResponse<List<ProgrammeDto>>> GetAllProgrammes()
        {
            var programmes = await _programmeRepository.ListAllAsync();


            var dtoList = programmes.Select(p => new ProgrammeDto
            {
                Id = p.Id,
                ProgrammeName = p.ProgrammeName,
                ProgrammeFee = p.ProgrammeFee
            }).ToList();


            if (dtoList == null || dtoList.Count == 0)
            {
                return new BaseResponse<List<ProgrammeDto>>
                {
                    Success = false,
                    Message = "No programmes found.",
                    StatusCode = 404
                };
            }


            return new BaseResponse<List<ProgrammeDto>>
            {
                Data = dtoList,
                Success = true,
                Message = "Programmes retrieved successfully",
                StatusCode = 200
            };

        }

        public async Task<BaseResponse<ProgrammeDto>> GetProgrammeById(Guid id)
        {
            var programme = await _programmeRepository.GetByIdAsync(id);
            if (programme == null)
            {
                return new BaseResponse<ProgrammeDto>
                {
                    Success = false,
                    Message = "",
                    Data = null
                };
            }

            return new BaseResponse<ProgrammeDto>
            {
                Success = true,
                Message = "Programme retrieved successfully",
                StatusCode = 200,
                Data = new ProgrammeDto
                {
                    Id = programme.Id,
                    ProgrammeName = programme.ProgrammeName,
                    ProgrammeFee = programme.ProgrammeFee
                }
            };
        }

        public async Task<BaseResponse<ProgrammeDto>> UpdateProgramme(UpdateProgrammeDto updateProgrammeDto)
        {
            var programme = await _programmeRepository.GetByIdAsync(updateProgrammeDto.Id);
            if (programme == null)
            {
                return new BaseResponse<ProgrammeDto>
                {
                    Success = false,
                    Message = "Programme not found.",
                    StatusCode = 404
                };
            }

            programme.ProgrammeName = updateProgrammeDto.ProgrammeName;
            programme.ProgrammeFee = updateProgrammeDto.ProgrammeFee;

            _programmeRepository.Update(programme);
            await _programmeRepository.SaveChangesAsync();

            return new BaseResponse<ProgrammeDto>
            {
                Success = true,
                Message = "Programme updated successfully",
                StatusCode = 200,
                Data = new ProgrammeDto
                {
                    Id = programme.Id,
                    ProgrammeName = programme.ProgrammeName,
                    ProgrammeFee = programme.ProgrammeFee
                }
            };
        }
    }
}
