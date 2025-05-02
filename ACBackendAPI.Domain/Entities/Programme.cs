namespace ACBackendAPI.Domain.Entities
{
    public class Programme : BaseEntity
    {
        public string ProgrammeName { get; set; }
        public long ProgrammeFee { get; set; }

        public List<AcademicInformation> AcademicInformation { get; set; }
    }
}
