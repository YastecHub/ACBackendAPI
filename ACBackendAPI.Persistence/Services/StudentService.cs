using ACBackendAPI.Application.Common.Responses;
using ACBackendAPI.Application.Dtos;
using ACBackendAPI.Application.Interfaces.IServices;
using ACBackendAPI.Application.Interfaces.IRepositories;
using ACBackendAPI.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ACBackendAPI.Application.Services
{
    public class StudentService : IStudentService
    {
        private readonly IAsyncRepository<Student, Guid> _studentRepository;
        private readonly IAsyncRepository<Guardian, Guid> _guardianRepository;
        private readonly IAsyncRepository<AcademicInformation, Guid> _academicInformationRepository;
        private readonly ICloudinaryService _cloudinary;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _dbContext;

        public StudentService(IAsyncRepository<Student, Guid> studentRepository, IAsyncRepository<Guardian, Guid> guardianRepository, IAsyncRepository<AcademicInformation, Guid> academicInformationRepository, ICloudinaryService cloudinary, UserManager<ApplicationUser> userManager, AppDbContext dbContext)
        {
            _studentRepository = studentRepository;
            _guardianRepository = guardianRepository;
            _academicInformationRepository = academicInformationRepository;
            _cloudinary = cloudinary;
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<BaseResponse<StudentDto>> CreateStudentAsync(CreateStudentDto studentDto)
        {
            var response = new BaseResponse<StudentDto>();

            // Validate guardian object
            if (studentDto.Guardian == null)
            {
                response.Success = false;
                response.Message = "Guardian information is required.";
                return response;
            }

            string avatarUrl = null;

            try
            {
                avatarUrl = studentDto.Avatar != null
                    ? await _cloudinary.UploadImageAsync(studentDto.Avatar)
                    : null;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Image upload failed: {ex.Message}";
                return response;
            }

            // Create user
            var newUser = new ApplicationUser
            {
                UserName = studentDto.Email,
                Email = studentDto.Email,
                FirstName = studentDto.FirstName,
                Surname = studentDto.Surname,
                LastName = studentDto.LastName,
                PhoneNumber = studentDto.PhoneNumber
            };

            var result = await _userManager.CreateAsync(newUser, "DefaultPassword123!");
            if (!result.Succeeded)
            {
                response.Success = false;
                response.Message = "Failed to create application user: " + string.Join(", ", result.Errors.Select(e => e.Description));
                return response;
            }


            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                // Map and persist Guardian
                var guardian = new Guardian
                {
                    FirstName = studentDto.Guardian.FirstName,
                    LastName = studentDto.Guardian.LastName,
                    Surname = studentDto.Guardian.Surname,
                    Email = studentDto.Guardian.Email,
                    RelationShip = studentDto.Guardian.RelationShip,
                    PhoneNumber = studentDto.Guardian.PhoneNumber,
                    Address = studentDto.Guardian.Address,
                    Students = new List<Student>()
                };

                await _guardianRepository.AddAsync(guardian);
                await _guardianRepository.SaveChangesAsync();

                // Map Student
                var student = new Student
                {
                    Avatar = avatarUrl,
                    Surname = studentDto.Surname,
                    LastName = studentDto.LastName,
                    FirstName = studentDto.FirstName,
                    Gender = studentDto.Gender,
                    Dob = studentDto.Dob,
                    Nationality = studentDto.Nationality,
                    Address = studentDto.Address,
                    PhoneNumber = studentDto.PhoneNumber,
                    Email = studentDto.Email,
                    GuardianId = guardian.Id,
                    ApplicationUserId = newUser.Id
                };

                var programmeExists = await _dbContext.Programmes
                                    .AnyAsync(p => p.Id == studentDto.AcademicInformation.ProgrammeId);

                if (!programmeExists)
                {
                    response.Success = false;
                    response.Message = $"Programme with ID {studentDto.AcademicInformation.ProgrammeId} does not exist.";
                    return response;
                }

                // Map Academic Info
                var academicInfo = new AcademicInformation
                {
                    Department = studentDto.AcademicInformation.Department,
                    CourseOfStudy = studentDto.AcademicInformation.CourseOfStudy,
                    ProgrammeId = studentDto.AcademicInformation.ProgrammeId,
                    Student = student
                };
                student.AcademicInformation = academicInfo;

                await _studentRepository.AddAsync(student);
                await _studentRepository.SaveChangesAsync();

                await transaction.CommitAsync();

                // Manual mapping to DTO
                response.Data = new StudentDto
                {
                    Avatar = student.Avatar,
                    Surname = student.Surname,
                    LastName = student.LastName,
                    FirstName = student.FirstName,
                    Gender = student.Gender,
                    Dob = student.Dob,
                    Nationality = student.Nationality,
                    Address = student.Address,
                    PhoneNumber = student.PhoneNumber,
                    Email = student.Email,
                    Guardian = new GuardianDto
                    {
                        FirstName = guardian.FirstName,
                        LastName = guardian.LastName,
                        Surname = guardian.Surname,
                        Email = guardian.Email,
                        RelationShip = guardian.RelationShip,
                        PhoneNumber = guardian.PhoneNumber,
                        Address = guardian.Address
                    },
                    AcademicInformation = new AcademicInformationDto
                    {
                        Department = academicInfo.Department,
                        CourseOfStudy = academicInfo.CourseOfStudy,
                        ProgrammeId = academicInfo.ProgrammeId,
                    }
                };

                response.Success = true;
                response.Message = "Student created successfully.";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                response.Data = null;
                response.Success = false;
                response.Message = $"Error creating student: {ex.Message}";
            }

            return response;
        }

        public async Task<BaseResponse<StudentDto>> GetStudentByIdAsync(Guid studentId)
        {
            var response = new BaseResponse<StudentDto>();

            try
            {
                // Including the Guardian and AcademicInformation entities
                var student = await _studentRepository.GetByAsync(
                    x => x.Id == studentId,
                    s => s.Guardian,
                    s => s.AcademicInformation);

                if (student == null)
                {
                    response.Success = false;
                    response.Message = "Student not found.";
                    return response;
                }

                var result = new StudentDto
                {
                    Avatar = student.Avatar,
                    Surname = student.Surname,
                    LastName = student.LastName,
                    FirstName = student.FirstName,
                    Gender = student.Gender,
                    Dob = student.Dob,
                    Nationality = student.Nationality,
                    Address = student.Address,
                    PhoneNumber = student.PhoneNumber,
                    Email = student.Email,
                    Guardian = new GuardianDto
                    {
                        FirstName = student.Guardian.FirstName,
                        LastName = student.Guardian.LastName,
                        Surname = student.Guardian.Surname,
                        Email = student.Guardian.Email,
                        RelationShip = student.Guardian.RelationShip,
                        PhoneNumber = student.Guardian.PhoneNumber,
                        Address = student.Guardian.Address,
                    },
                    AcademicInformation = new AcademicInformationDto
                    {
                        Department = student.AcademicInformation.Department,
                        CourseOfStudy = student.AcademicInformation.CourseOfStudy,
                        ProgrammeId = student.AcademicInformation.ProgrammeId
                    }
                };

                response.Data = result;
                response.Success = true;
                response.Message = "Student retrieved successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error retrieving student: {ex.Message}";
            }

            return response;
        }

        public async Task<BaseResponse<List<StudentDto>>> ListStudentsAsync()
        {
            var response = new BaseResponse<List<StudentDto>>();

            try
            {
                // Retrieve all students and include Guardian and AcademicInformation
                var students = await _studentRepository.ListAllAsync(s => s.Guardian, s => s.AcademicInformation);

                var result = students.Select(student => new StudentDto
                {
                    Avatar = student.Avatar,
                    Surname = student.Surname,
                    LastName = student.LastName,
                    FirstName = student.FirstName,
                    Gender = student.Gender,
                    Dob = student.Dob,
                    Nationality = student.Nationality,
                    Address = student.Address,
                    PhoneNumber = student.PhoneNumber,
                    Email = student.Email,
                    Guardian = new GuardianDto
                    {
                        FirstName = student.Guardian.FirstName,
                        LastName = student.Guardian.LastName,
                        Surname = student.Guardian.Surname,
                        Email = student.Guardian.Email,
                        RelationShip = student.Guardian.RelationShip,
                        PhoneNumber = student.Guardian.PhoneNumber,
                        Address = student.Guardian.Address,
                    },
                    AcademicInformation = new AcademicInformationDto
                    {
                        Department = student.AcademicInformation.Department,
                        CourseOfStudy = student.AcademicInformation.CourseOfStudy,
                        ProgrammeId = student.AcademicInformation.ProgrammeId
                    }
                }).ToList();

                response.Data = result;
                response.Success = true;
                response.Message = "Students listed successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error listing students: {ex.Message}";
            }

            return response;
        }

        public async Task<BaseResponse<StudentDto>> UpdateStudentAsync(Guid studentId, StudentDto studentDto)
        {
            var response = new BaseResponse<StudentDto>();

            try
            {
                var student = await _studentRepository.GetByAsync(
                    x => x.Id == studentId,
                    s => s.Guardian,
                    s => s.AcademicInformation);

                if (student == null)
                {
                    response.Success = false;
                    response.Message = "Student not found.";
                    return response;
                }

                // Update student properties
                student.Avatar = studentDto.Avatar;
                student.Surname = studentDto.Surname;
                student.LastName = studentDto.LastName;
                student.FirstName = studentDto.FirstName;
                student.Gender = studentDto.Gender;
                student.Dob = studentDto.Dob;
                student.Nationality = studentDto.Nationality;
                student.Address = studentDto.Address;
                student.PhoneNumber = studentDto.PhoneNumber;
                student.Email = studentDto.Email;

                // Update related Guardian details
                student.Guardian.FirstName = studentDto.Guardian.FirstName;
                student.Guardian.LastName = studentDto.Guardian.LastName;
                student.Guardian.Surname = studentDto.Guardian.Surname;
                student.Guardian.Email = studentDto.Guardian.Email;
                student.Guardian.RelationShip = studentDto.Guardian.RelationShip;
                student.Guardian.PhoneNumber = studentDto.Guardian.PhoneNumber;
                student.Guardian.Address = studentDto.Guardian.Address;

                // Update Academic Information
                student.AcademicInformation.Department = studentDto.AcademicInformation.Department;
                student.AcademicInformation.CourseOfStudy = studentDto.AcademicInformation.CourseOfStudy;
                student.AcademicInformation.ProgrammeId = studentDto.AcademicInformation.ProgrammeId;

                _studentRepository.Update(student);
                await _studentRepository.SaveChangesAsync();

                // Map the updated entity back to StudentDto
                var updatedStudentDto = new StudentDto
                {
                    Avatar = student.Avatar,
                    Surname = student.Surname,
                    LastName = student.LastName,
                    FirstName = student.FirstName,
                    Gender = student.Gender,
                    Dob = student.Dob,
                    Nationality = student.Nationality,
                    Address = student.Address,
                    PhoneNumber = student.PhoneNumber,
                    Email = student.Email,
                    Guardian = new GuardianDto
                    {
                        FirstName = student.Guardian.FirstName,
                        LastName = student.Guardian.LastName,
                        Surname = student.Guardian.Surname,
                        Email = student.Guardian.Email,
                        RelationShip = student.Guardian.RelationShip,
                        PhoneNumber = student.Guardian.PhoneNumber,
                        Address = student.Guardian.Address,
                    },
                    AcademicInformation = new AcademicInformationDto
                    {
                        Department = student.AcademicInformation.Department,
                        CourseOfStudy = student.AcademicInformation.CourseOfStudy,
                        ProgrammeId = student.AcademicInformation.ProgrammeId
                    }
                };

                response.Data = updatedStudentDto;
                response.Success = true;
                response.Message = "Student updated successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error updating student: {ex.Message}";
            }
            return response;
        }

        public async Task<BaseResponse<bool>> DeleteStudentAsync(Guid studentId)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var student = await _studentRepository.GetByIdAsync(studentId);
                if (student == null)
                {
                    response.Success = false;
                    response.Message = "Student not found.";
                    response.Data = false;
                    return response;
                }

                _studentRepository.Delete(student);
                await _studentRepository.SaveChangesAsync();

                response.Data = true;
                response.Success = true;
                response.Message = "Student deleted successfully.";
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Success = false;
                response.Message = $"Error deleting student: {ex.Message}";
            }
            return response;
        }
    }
}
