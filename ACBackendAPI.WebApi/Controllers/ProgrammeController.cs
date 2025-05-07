using ACBackendAPI.Application.Dtos;
using ACBackendAPI.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using FluentValidation.Results;

namespace ACBackendAPI.WebApi.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProgrammeController : ControllerBase
    {
        private readonly IProgrammeService _programmeService;
        private readonly IValidator<CreateProgrammeDto> _createProgrammeDtoValidator;

        public ProgrammeController(IProgrammeService programmeService, IValidator<CreateProgrammeDto> createProgrammeDtoValidator)
        {
            _programmeService = programmeService;
            _createProgrammeDtoValidator = createProgrammeDtoValidator;
        }

        [HttpPost("create-programme")]
        public async Task<IActionResult> CreateProgramme([FromForm] CreateProgrammeDto createProgrammeDto)
        {
            ValidationResult validationResult = await _createProgrammeDtoValidator.ValidateAsync(createProgrammeDto);

            if (!validationResult.IsValid)
            {
                foreach (var failure in validationResult.Errors)
                {
                    ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
                }
                return BadRequest(ModelState);
            }

            var response = await _programmeService.CreateProgramme(createProgrammeDto);
            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpGet("get-all-programmes")]
        public async Task<IActionResult> GetAllProgrammes()
        {
            var response = await _programmeService.GetAllProgrammes();
            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }

        [HttpGet("get-programme-by-id/{id}")]
        public async Task<IActionResult> GetProgrammeById(Guid id)
        {
            var response = await _programmeService.GetProgrammeById(id);
            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }

        [HttpPut("update-programme")]
        public async Task<IActionResult> UpdateProgramme([FromForm] UpdateProgrammeDto dto)
        {
            var response = await _programmeService.UpdateProgramme(dto);
            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }

        //[HttpDelete("delete-programme/{id}")]
        //public async Task<IActionResult> DeleteProgramme(Guid id)
        //{
        //    var response = await _programmeService.DeleteProgramme(id);
        //    if (!response.Success)
        //        return NotFound(response);
        //    return Ok(response);
        //}

        [HttpPatch("toggle-programme-status/{id}")]
        public async Task<IActionResult> ToggleProgrammeStatus(Guid id)
        {
            var response = await _programmeService.ToggleProgrammeStatus(id);
            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }
    }
}
