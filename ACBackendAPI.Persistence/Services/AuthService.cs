using ACBackendAPI.Application.Common.Responses.ACBackendAPI.Application.Common.Responses;
using ACBackendAPI.Application.Dtos;
using ACBackendAPI.Application.Interfaces.IServices;
using ACBackendAPI.Domain.Entities;
using ACBackendAPI.Domain.Enum;
using Microsoft.AspNetCore.Identity;

namespace ACBackendAPI.Persistence.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;

        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager, AppDbContext context, JwtService jwtService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<BaseResponse<AdminDto>> RegisterAdmin(AdminDto adminDto)
        {
            var user = new ApplicationUser
            {
                UserName = adminDto.Email,
                Email = adminDto.Email,
                Surname = adminDto.Surname,
                Gender = adminDto.Gender,
                Address = adminDto.Address,
                LastName = adminDto.Name,
                Avatar = adminDto.Avatar,
                Nationality = adminDto.Nationality
            };

            var result = await _userManager.CreateAsync(user, adminDto.Password);
            if (!result.Succeeded)
            {
                return new BaseResponse<AdminDto>
                {
                    Success = false,
                    Message = "Admin Registration failed",
                    Data = null,
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }
            await _userManager.AddToRoleAsync(user, Role.Admin);

            var admin = new Admin
            {
                ApplicationUserId = user.Id,
                Email = adminDto.Email,
                Name = adminDto.Name,
                Avatar = adminDto.Avatar,
                Gender = adminDto.Gender,
                PhoneNumber = adminDto.PhoneNumber,
                Address = adminDto.Address
            };

            _context.Admins.Add(admin);
            await _context.SaveChangesAsync();

            return new BaseResponse<AdminDto>
            {
                Success = true,
                Message = "Admin registered successfully",
                Data = adminDto
            };
        }

        public async Task<BaseResponse<StudentDto>> RegisterStudent(StudentDto studentDto)
        {
            var user = new ApplicationUser
            {
                UserName = studentDto.Email,
                Email = studentDto.Email,
                Gender = studentDto.Gender,
                Address = studentDto.Address,
                Surname = studentDto.Surname,
                LastName = studentDto.LastName,
                Avatar = studentDto.Avatar,
                DateOfBirth = DateOnly.FromDateTime(studentDto.Dob),
                Nationality = studentDto.Nationality,
                PhoneNumber = studentDto.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, studentDto.Password);
            if (!result.Succeeded)
            {
                return new BaseResponse<StudentDto>
                {
                    Success = false,
                    Message = "Student Registration failed",
                    Data = null,
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }

            await _userManager.AddToRoleAsync(user, Role.Student);

            var guadian = new Guardian
            {
                Name = studentDto.GuardianInformation.Name,
                PhoneNumber = studentDto.GuardianInformation.PhoneNumber,
                Email = studentDto.GuardianInformation.Email,
                Address = studentDto.GuardianInformation.Address,
                RelationShip = studentDto.GuardianInformation.Relationship,
            };

            _context.Guardians.Add(guadian);
            await _context.SaveChangesAsync();


            var student = new Student
            {
                Avatar = studentDto.Avatar,
                Surname = studentDto.Surname,
                LastName = studentDto.LastName,
                Email = studentDto.Email,
                Gender = studentDto.Gender,
                PhoneNumber = studentDto.PhoneNumber,
                Dob = studentDto.Dob,
                Nationality = studentDto.Nationality,
                Address = studentDto.Address,
                GuardianId = guadian.Id,
                ApplicationUserId = user.Id
            };
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            var academicInfo = new AcademicInformation
            {
                StudentId = student.Id,
                Department = studentDto.AcademicInformation.Department,
                DepartmentDesc = studentDto.AcademicInformation.Department.ToString(),
                CourseOfStudy = studentDto.AcademicInformation.CourseOfStudy,
                ProgrammeId = studentDto.AcademicInformation.ProgrammeId,
            };

            _context.AcademicInformations.Add(academicInfo);
            await _context.SaveChangesAsync();

            return new BaseResponse<StudentDto>
            {
                Success = true,
                Message = "Student registered successfully",
                Data = studentDto,
                Errors = result.Errors.Select(e => e.Description).ToList()
            };
        }

        public async Task<BaseResponse<LoginResponseDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
                return new BaseResponse<LoginResponseDto>
                {
                    Success = false,
                    Message = "Invalid login attempt",
                    Data = null
                };

            var roles = await _userManager.GetRolesAsync(user);
            var tokens = _jwtService.GenerateToken(user, roles.ToList());

            return new BaseResponse<LoginResponseDto>
            {
                Success = true,
                Message = "Login successful",
                Data = new LoginResponseDto
                {
                    Email = user.Email,
                    Token = tokens,
                    Roles = roles.ToList()
                }
            };
        }

    }
}

