using ACBackendAPI.Domain.Enum;

namespace ACBackendAPI.Domain.Entities
{
    public class Programme : BaseEntity
    {
        public string? ProgrammeImage { get; set; }
        public string ProgrammeName { get; set; }
        public decimal ProgrammeFee { get; set; }
        public string ProgrammeDescription { get; set; }
        public ProgrammeStatus ProgrammeStatus { get; set; } = ProgrammeStatus.Active;
        public string ProgrammeStatusDesc { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<AcademicInformation> AcademicInformation { get; set; }

        public Programme()
        {
            ProgrammeStatusDesc = ProgrammeStatus.ToString();
        }
    }
}
