namespace ACBackendAPI.Application.Dtos
{
    public class ProgrammeDto
    {
        public Guid Id { get; set; }
        public string ProgrammeName { get; set; }
        public string ProgrammeDescription { get; set; }
        public long ProgrammeFee { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }


    public class UpdateProgrammeDto
    {
        public Guid Id { get; set; }
        public string ProgrammeName { get; set; }
        public string ProgrammeDescription { get; set; }
        public long ProgrammeFee { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
    public class CreateProgrammeDto
    {
        public string ProgrammeName { get; set; }
        public long ProgrammeFee { get; set; }
        public string ProgrammeDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}

