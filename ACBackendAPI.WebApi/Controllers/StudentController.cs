using ACBackendAPI.Application.Dtos;
using ACBackendAPI.Application.Interfaces.IServices;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ACBackendAPI.WebApi.Controllers
{
    //[Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly IValidator<CreateStudentDto> _createStudentDtoValidator;

        public StudentController(IStudentService studentService, IValidator<CreateStudentDto> createStudentDtoValidator)
        {
            _studentService = studentService;
            _createStudentDtoValidator = createStudentDtoValidator;
        }

        [HttpPost("create-student")]
        public async Task<IActionResult> CreateStudent([FromForm] CreateStudentDto createStudentDto)
        {
            // Validate the incoming DTO using FluentValidation
            ValidationResult validationResult = await _createStudentDtoValidator.ValidateAsync(createStudentDto);
            if (!validationResult.IsValid)
            {
                foreach (var failure in validationResult.Errors)
                {
                    ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
                }
                return BadRequest(ModelState);
            }

            var response = await _studentService.CreateStudentAsync(createStudentDto);
            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpGet("get-all-students")]
        public async Task<IActionResult> GetAllStudents()
        {
            var response = await _studentService.ListStudentsAsync();
            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }

        [HttpGet("get-student-by-id/{id}")]
        public async Task<IActionResult> GetStudentById(Guid id)
        {
            var response = await _studentService.GetStudentByIdAsync(id);
            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }

        [HttpPut("update-student/{id}")]
        public async Task<IActionResult> UpdateStudent(Guid id, [FromForm] StudentDto studentDto)
        {
            var response = await _studentService.UpdateStudentAsync(id, studentDto);
            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }

        [HttpDelete("delete-student/{id}")]
        public async Task<IActionResult> DeleteStudent(Guid id)
        {
            var response = await _studentService.DeleteStudentAsync(id);
            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }
    }
}
