using ACBackendAPI.Application.Dtos;
using ACBackendAPI.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ACBackendAPI.WebApi.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProgrammeController : ControllerBase
    {
        private readonly IProgrammeService _programmeService;

        public ProgrammeController(IProgrammeService programmeService)
        {
            _programmeService = programmeService;
        }

        [HttpPost("create-programme")]
        public async Task<IActionResult> CreateProgramme([FromBody] CreateProgrammeDto createProgrammeDto)
        {
            var response = await _programmeService.CreateProgramme(createProgrammeDto);
            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpGet("get-all-programme")]
        public async Task<IActionResult> GetAllProgrammes()
        {
            var response = await _programmeService.GetAllProgrammes();
            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProgrammeById(Guid id)
        {
            var response = await _programmeService.GetProgrammeById(id);
            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateProgramme([FromBody] UpdateProgrammeDto dto)
        {
            var response = await _programmeService.UpdateProgramme(dto);
            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteProgramme(Guid id)
        {
            var response = await _programmeService.DeleteProgramme(id);
            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }
    }
}
