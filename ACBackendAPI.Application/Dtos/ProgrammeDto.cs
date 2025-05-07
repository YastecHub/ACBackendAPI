using ACBackendAPI.Domain.Enum;
using Microsoft.AspNetCore.Http;

namespace ACBackendAPI.Application.Dtos
{
    public class ProgrammeDto
    {
        public Guid Id { get; set; }
        public string? ProgrammeImage { get; set; }
        public string ProgrammeName { get; set; }
        public string ProgrammeDescription { get; set; }
        public decimal ProgrammeFee { get; set; }
        public ProgrammeStatus ProgrammeStatus { get; set; }
        public string ProgrammeStatusDesc { get; set; }
        public long StartDate { get; set; }
        public long EndDate { get; set; }
        public ProgrammeDto()
        {
           ProgrammeStatusDesc = ProgrammeStatus.ToString();
        }
    }


    public class UpdateProgrammeDto
    {
        public Guid Id { get; set; }
        public IFormFile? ProgrammeImage { get; set; }
        public string ProgrammeName { get; set; }
        public string ProgrammeDescription { get; set; }
        public decimal ProgrammeFee { get; set; }
        public ProgrammeStatus ProgrammeStatus { get; set; }
        public long StartDate { get; set; }
        public long EndDate { get; set; }
    }
    public class CreateProgrammeDto
    {
        public IFormFile ProgrammeImage { get; set; }
        public string ProgrammeName { get; set; }
        public string ProgrammeDescription { get; set; }
        public decimal ProgrammeFee { get; set; }
        public long StartDate { get; set; }
        public long EndDate { get; set; }
    }
}

