namespace ACBackendAPI.Domain.Entities
{
    public class Programme : BaseEntity
    {
        public string ProgrammeName { get; set; }

        public List<AcademicInformation> AcademicInformation { get; set; }
    }
}
