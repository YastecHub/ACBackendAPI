namespace ACBackendAPI.Application.Dtos
{
    public class ProgrammeDto
    {
        public Guid Id { get; set; }
        public string ProgrammeName { get; set; }
        public string ProgrammeDescription { get; set; }
        public decimal ProgrammeFee { get; set; }
        public long StartDate { get; set; }
        public long EndDate { get; set; }
    }


    public class UpdateProgrammeDto
    {
        public Guid Id { get; set; }
        public string ProgrammeName { get; set; }
        public string ProgrammeDescription { get; set; }
        public decimal ProgrammeFee { get; set; }
        public long StartDate { get; set; }
        public long EndDate { get; set; }
    }
    public class CreateProgrammeDto
    {
        public string ProgrammeName { get; set; }
        public string ProgrammeDescription { get; set; }
        public decimal ProgrammeFee { get; set; }
        public long StartDate { get; set; }
        public long EndDate { get; set; }
    }
}

