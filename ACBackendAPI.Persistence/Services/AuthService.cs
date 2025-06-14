﻿using ACBackendAPI.Application.Common.Responses;
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
    private readonly ICloudinaryService _cloudinaryService;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        AppDbContext context,
        IJwtService jwtService, ICloudinaryService cloudinaryService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
        _jwtService = jwtService;
        _cloudinaryService = cloudinaryService;
    }

    public async Task<BaseResponse<AdminRegistrationDto>> RegisterAdmin(AdminRegistrationDto adminDto)
    {
        var avatarUrl = adminDto.Avatar != null
            ? await _cloudinaryService.UploadImageAsync(adminDto.Avatar)
            : null;
        var user = new ApplicationUser
        {
            Avatar = avatarUrl,
            UserName = adminDto.Email,
            Email = adminDto.Email,
            Surname = adminDto.Surname,
            Gender = adminDto.Gender,
            Address = adminDto.Address,
            FirstName = adminDto.FirstName,
            LastName = adminDto.LastName,
            Nationality = adminDto.Nationality
        };

        var result = await _userManager.CreateAsync(user, adminDto.Password);
        if (!result.Succeeded)
        {
            return new BaseResponse<AdminRegistrationDto>
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
                Surname = adminDto.Surname,
                FirstName = adminDto.FirstName,                
                LastName= adminDto.LastName,
                Avatar = avatarUrl,
                Gender = adminDto.Gender,
                GenderDesc = adminDto.Gender.ToString(),
                PhoneNumber = adminDto.PhoneNumber,
                Address = adminDto.Address
            };

            _context.Admins.Add(admin);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return new BaseResponse<AdminRegistrationDto>
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

            return new BaseResponse<AdminRegistrationDto>
            {
                Success = false,
                Message = "Admin registration failed. Transaction rolled back.",
                Errors = new List<string> { ex.Message }
            };
        }
    }

    public async Task<BaseResponse<StudentRegistrationDto>> RegisterStudent(StudentRegistrationDto studentDto)
    {
        var avatarUrl = studentDto.Avatar != null
            ? await _cloudinaryService.UploadImageAsync(studentDto.Avatar)
            : null;
        var user = new ApplicationUser
        {
            UserName = studentDto.Email,
            Email = studentDto.Email,
            Gender = studentDto.Gender,
            Address = studentDto.Address,
            Surname = studentDto.Surname,
            LastName = studentDto.LastName,
            Avatar = avatarUrl,
            DateOfBirth = DateOnly.FromDateTime(studentDto.Dob),
            Nationality = studentDto.Nationality,
            PhoneNumber = studentDto.PhoneNumber
        };

        var result = await _userManager.CreateAsync(user, studentDto.Password);
        if (!result.Succeeded)
        {
            return new BaseResponse<StudentRegistrationDto>
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
                Surname = studentDto.GuardianInformation.Surname,
                FirstName = studentDto.GuardianInformation.FirstName,
                LastName = studentDto.GuardianInformation.LastName,
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
                Avatar = avatarUrl,
                Surname = studentDto.Surname,
                FirstName = studentDto.FirstName,   
                LastName = studentDto.LastName,
                Email = studentDto.Email,
                Gender = studentDto.Gender,
                GenderDesc = studentDto.Gender.ToString(),
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

            return new BaseResponse<StudentRegistrationDto>
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

            return new BaseResponse<StudentRegistrationDto>
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
                Surname = user.Surname,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Avatar = user.Avatar,
                Email = user.Email,
                Roles = roles.ToList(),
                Token = token
            }
        };
    }
}
