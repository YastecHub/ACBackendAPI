using ACBackendAPI.Application.Common.Responses;
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

            var data = new ProgrammeDto
            {
                Id = programme.Id,
                ProgrammeName = programme.ProgrammeName,
                ProgrammeFee = programme.ProgrammeFee
            };

            return BaseResponse<ProgrammeDto>.Succes(data, "Programme created successfully", 201);
        }

        public async Task<BaseResponse<bool>> DeleteProgramme(Guid id)
        {
            var programme = await _programmeRepository.GetByIdAsync(id);
            if (programme == null)
                return BaseResponse<bool>.Failure("Programme not found", statusCode: 404);

            _programmeRepository.Delete(programme);
            await _programmeRepository.SaveChangesAsync();

            return BaseResponse<bool>.Succes(true, "Programme deleted successfully", 200);
        }

        public async Task<BaseResponse<List<ProgrammeDto>>> GetAllProgrammes()
        {
            var programmes = await _programmeRepository.ListAllAsync();

            var programmeDtos = programmes.Select(p => new ProgrammeDto
            {
                Id = p.Id,
                ProgrammeName = p.ProgrammeName,
                ProgrammeFee = p.ProgrammeFee
            }).ToList();

            if (!programmeDtos.Any())
                return BaseResponse<List<ProgrammeDto>>.Failure("No programmes found", statusCode: 404);

            return BaseResponse<List<ProgrammeDto>>.Succes(programmeDtos, "Programmes retrieved successfully", 200);
        }

        public async Task<BaseResponse<ProgrammeDto>> GetProgrammeById(Guid id)
        {
            var programme = await _programmeRepository.GetByIdAsync(id);
            if (programme == null)
                return BaseResponse<ProgrammeDto>.Failure("Programme not found", statusCode: 404);

            var data = new ProgrammeDto
            {
                Id = programme.Id,
                ProgrammeName = programme.ProgrammeName,
                ProgrammeFee = programme.ProgrammeFee
            };

            return BaseResponse<ProgrammeDto>.Succes(data, "Programme retrieved successfully", 200);
        }

        public async Task<BaseResponse<ProgrammeDto>> UpdateProgramme(UpdateProgrammeDto updateProgrammeDto)
        {
            var programme = await _programmeRepository.GetByIdAsync(updateProgrammeDto.Id);
            if (programme == null)
                return BaseResponse<ProgrammeDto>.Failure("Programme not found", statusCode: 404);

            programme.ProgrammeName = updateProgrammeDto.ProgrammeName;
            programme.ProgrammeFee = updateProgrammeDto.ProgrammeFee;

            _programmeRepository.Update(programme);
            await _programmeRepository.SaveChangesAsync();

            var data = new ProgrammeDto
            {
                Id = programme.Id,
                ProgrammeName = programme.ProgrammeName,
                ProgrammeFee = programme.ProgrammeFee
            };

            return BaseResponse<ProgrammeDto>.Succes(data, "Programme updated successfully", 200);
        }
    }
}
