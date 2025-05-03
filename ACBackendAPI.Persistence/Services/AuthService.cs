using ACBackendAPI.Application.Common.Responses;
using ACBackendAPI.Application.Dtos;
using ACBackendAPI.Application.Interfaces.IServices;
using ACBackendAPI.Domain.Entities;
using ACBackendAPI.Domain.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ACBackendAPI.Persistence.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly AppDbContext _context;
    private readonly IJwtService _jwtService;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        AppDbContext context,
        IJwtService jwtService)
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
                Message = "Admin registration failed",
                Errors = result.Errors.Select(e => e.Description).ToList()
            };
        }

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
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

            await transaction.CommitAsync();

            return new BaseResponse<AdminDto>
            {
                Success = true,
                Message = "Admin registered successfully",
                Data = adminDto
            };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            await _userManager.DeleteAsync(user);

            return new BaseResponse<AdminDto>
            {
                Success = false,
                Message = "Admin registration failed. Transaction rolled back.",
                Errors = new List<string> { ex.Message }
            };
        }
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
                Message = "Student registration failed",
                Errors = result.Errors.Select(e => e.Description).ToList()
            };
        }

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            await _userManager.AddToRoleAsync(user, Role.Student);

            var existingGuardian = await _context.Guardians
                .FirstOrDefaultAsync(g => g.Email == studentDto.GuardianInformation.Email);

            Guardian guardian = existingGuardian ?? new Guardian
            {
                Name = studentDto.GuardianInformation.Name,
                PhoneNumber = studentDto.GuardianInformation.PhoneNumber,
                Email = studentDto.GuardianInformation.Email,
                Address = studentDto.GuardianInformation.Address,
                RelationShip = studentDto.GuardianInformation.Relationship
            };

            if (existingGuardian == null)
            {
                _context.Guardians.Add(guardian);
                await _context.SaveChangesAsync();
            }

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
                GuardianId = guardian.Id,
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
                ProgrammeId = studentDto.AcademicInformation.ProgrammeId
            };

            _context.AcademicInformations.Add(academicInfo);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return new BaseResponse<StudentDto>
            {
                Success = true,
                Message = "Student registered successfully",
                Data = studentDto
            };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            await _userManager.DeleteAsync(user);

            return new BaseResponse<StudentDto>
            {
                Success = false,
                Message = "Student registration failed. Transaction rolled back.",
                Errors = new List<string> { ex.Message }
            };
        }
    }

    public async Task<BaseResponse<LoginResponseDto>> Login(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
        {
            return new BaseResponse<LoginResponseDto>
            {
                Success = false,
                Message = "Invalid login attempt"
            };
        }

        var roles = await _userManager.GetRolesAsync(user);
        var token = _jwtService.GenerateToken(user, roles.ToList());

        return new BaseResponse<LoginResponseDto>
        {
            Success = true,
            Message = "Login successful",
            Data = new LoginResponseDto
            {
                Email = user.Email,
                Token = token,
                Roles = roles.ToList()
            }
        };
    }
}
