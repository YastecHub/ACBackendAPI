using ACBackendAPI.Application.Dtos;
using ACBackendAPI.Application.Interfaces.IServices;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ACBackendAPI.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IValidator<AdminDto> _adminDtoValidator;
        private readonly IValidator<StudentDto> _studentDtoValidator;
        private readonly IValidator<LoginDto> _loginDtoValidator;

        public AuthController(
            IAuthService authService,
            IValidator<AdminDto> adminDtoValidator,
            IValidator<StudentDto> studentDtoValidator,
            IValidator<LoginDto> loginDtoValidator)
        {
            _authService = authService;
            _adminDtoValidator = adminDtoValidator;
            _studentDtoValidator = studentDtoValidator;
            _loginDtoValidator = loginDtoValidator;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromForm] AdminDto adminDto)
        {
            var validationResult = await _adminDtoValidator.ValidateAsync(adminDto);
            if (!validationResult.IsValid)
            {
                foreach (var failure in validationResult.Errors)
                {
                    ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
                }
                return BadRequest(ModelState);
            }

            var response = await _authService.RegisterAdmin(adminDto);
            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPost("register-student")]
        public async Task<IActionResult> RegisterStudent([FromForm] StudentDto studentDto)
        {
            var validationResult = await _studentDtoValidator.ValidateAsync(studentDto);
            if (!validationResult.IsValid)
            {
                foreach (var failure in validationResult.Errors)
                {
                    ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
                }
                return BadRequest(ModelState);
            }

            var response = await _authService.RegisterStudent(studentDto);
            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var validationResult = await _loginDtoValidator.ValidateAsync(loginDto);
            if (!validationResult.IsValid)
            {
                foreach (var failure in validationResult.Errors)
                {
                    ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
                }
                return BadRequest(ModelState);
            }

            var response = await _authService.Login(loginDto);
            if (!response.Success)
                return Unauthorized(response);

            return Ok(response);
        }
    }
}
