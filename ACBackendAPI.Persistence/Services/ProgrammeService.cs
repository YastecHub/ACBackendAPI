using ACBackendAPI.Application.Common.Responses;
using ACBackendAPI.Application.Dtos;
using ACBackendAPI.Application.Interfaces.IRepositories;
using ACBackendAPI.Application.Interfaces.IServices;
using ACBackendAPI.Domain.Entities;
using ACBackendAPI.Domain.Enum;

namespace ACBackendAPI.Persistence.Services
{
    public class ProgrammeService : IProgrammeService
    {
        private readonly IAsyncRepository<Programme, Guid> _programmeRepository;
        private readonly ICloudinaryService _cloudinaryService;

        public ProgrammeService(IAsyncRepository<Programme, Guid> programmeRepository, ICloudinaryService cloudinaryService)
        {
            _programmeRepository = programmeRepository;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<BaseResponse<ProgrammeDto>> CreateProgramme(CreateProgrammeDto createProgrammeDto)
        {
            var programmeImageUrl = createProgrammeDto.ProgrammeImage != null
                ? await _cloudinaryService.UploadImageAsync(createProgrammeDto.ProgrammeImage)
                : null;
            var programme = new Programme
            {
                Id = Guid.NewGuid(),
                ProgrammeImage = programmeImageUrl,
                ProgrammeName = createProgrammeDto.ProgrammeName,
                ProgrammeDescription = createProgrammeDto.ProgrammeDescription,
                ProgrammeFee = createProgrammeDto.ProgrammeFee,
                ProgrammeStatus = ProgrammeStatus.Active,
                ProgrammeStatusDesc = ProgrammeStatus.Active.ToString(),
                StartDate = DateTimeOffset.FromUnixTimeSeconds(createProgrammeDto.StartDate).UtcDateTime,
                EndDate = DateTimeOffset.FromUnixTimeSeconds(createProgrammeDto.EndDate).UtcDateTime,
                CreatedOn = DateTime.UtcNow
            };

            await _programmeRepository.AddAsync(programme);
            await _programmeRepository.SaveChangesAsync();

            var data = new ProgrammeDto
            {
                Id = programme.Id,
                ProgrammeImage = programmeImageUrl,
                ProgrammeName = programme.ProgrammeName,
                ProgrammeDescription = programme.ProgrammeDescription,
                ProgrammeFee = programme.ProgrammeFee,
                ProgrammeStatus = programme.ProgrammeStatus,
                ProgrammeStatusDesc = programme.ProgrammeStatusDesc,
                StartDate = new DateTimeOffset(programme.StartDate).ToUnixTimeSeconds(),
                EndDate = new DateTimeOffset(programme.EndDate).ToUnixTimeSeconds()
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
                ProgrammeImage = p.ProgrammeImage,
                ProgrammeName = p.ProgrammeName,
                ProgrammeDescription = p.ProgrammeDescription,
                ProgrammeFee = p.ProgrammeFee,
                ProgrammeStatus = p.ProgrammeStatus,
                ProgrammeStatusDesc = p.ProgrammeStatusDesc,
                StartDate = new DateTimeOffset(p.StartDate).ToUnixTimeSeconds(),
                EndDate = new DateTimeOffset(p.EndDate).ToUnixTimeSeconds()
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
                ProgrammeImage = programme.ProgrammeImage,
                ProgrammeName = programme.ProgrammeName,
                ProgrammeDescription = programme.ProgrammeDescription,
                ProgrammeFee = programme.ProgrammeFee,
                ProgrammeStatus = programme.ProgrammeStatus,
                ProgrammeStatusDesc = programme.ProgrammeStatusDesc,
                StartDate = new DateTimeOffset(programme.StartDate).ToUnixTimeSeconds(),
                EndDate = new DateTimeOffset(programme.EndDate).ToUnixTimeSeconds()
            };

            return BaseResponse<ProgrammeDto>.Succes(data, "Programme retrieved successfully", 200);
        }

        public async Task<BaseResponse<ProgrammeDto>> UpdateProgramme(UpdateProgrammeDto updateProgrammeDto)
        {
            var programme = await _programmeRepository.GetByIdAsync(updateProgrammeDto.Id);
            if (programme == null)
                return BaseResponse<ProgrammeDto>.Failure("Programme not found", statusCode: 404);

            if (updateProgrammeDto.ProgrammeImage != null)
            {
                var newImageUrl = await _cloudinaryService.UploadImageAsync(updateProgrammeDto.ProgrammeImage);
                programme.ProgrammeImage = newImageUrl;
            }

            programme.ProgrammeName = updateProgrammeDto.ProgrammeName;
            programme.ProgrammeFee = updateProgrammeDto.ProgrammeFee;
            programme.ProgrammeDescription = updateProgrammeDto.ProgrammeDescription;
            programme.ProgrammeStatus = updateProgrammeDto.ProgrammeStatus;
            programme.StartDate = DateTimeOffset.FromUnixTimeSeconds(updateProgrammeDto.StartDate).UtcDateTime;
            programme.EndDate = DateTimeOffset.FromUnixTimeSeconds(updateProgrammeDto.EndDate).UtcDateTime;
            programme.ModifiedOn = DateTime.UtcNow;

            _programmeRepository.Update(programme);
            await _programmeRepository.SaveChangesAsync();

            var data = new ProgrammeDto
            {
                Id = programme.Id,
                ProgrammeImage = programme.ProgrammeImage,
                ProgrammeName = programme.ProgrammeName,
                ProgrammeDescription = programme.ProgrammeDescription,
                ProgrammeFee = programme.ProgrammeFee,
                ProgrammeStatus = programme.ProgrammeStatus,
                ProgrammeStatusDesc = programme.ProgrammeStatusDesc,
                StartDate = new DateTimeOffset(programme.StartDate).ToUnixTimeSeconds(),
                EndDate = new DateTimeOffset(programme.EndDate).ToUnixTimeSeconds(),
            };

            return BaseResponse<ProgrammeDto>.Succes(data, "Programme updated successfully", 200);
        }

        public async Task<BaseResponse<string>> ToggleProgrammeStatus(Guid id)
        {
            var programme = await _programmeRepository.GetByIdAsync(id);
            if (programme == null)
                return BaseResponse<string>.Failure("Programme not found", statusCode: 404);

            // Toggle logic
            programme.ProgrammeStatus = programme.ProgrammeStatus == ProgrammeStatus.Active
                ? ProgrammeStatus.Cancelled
                : ProgrammeStatus.Active;

            programme.ProgrammeStatusDesc = programme.ProgrammeStatus.ToString();
            programme.ModifiedOn = DateTime.UtcNow;

            _programmeRepository.Update(programme);
            await _programmeRepository.SaveChangesAsync();

            return BaseResponse<string>.Succes($"Programme status updated to {programme.ProgrammeStatus}", "Programme status toggled", 200);
        }

    }
}
