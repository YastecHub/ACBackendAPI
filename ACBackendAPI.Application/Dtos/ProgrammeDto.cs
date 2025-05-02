namespace ACBackendAPI.Application.Dtos
{
    public class ProgrammeDto
    {
        public Guid Id { get; set; }
        public string ProgrammeName { get; set; }
        public long ProgrammeFee { get; set; }
    }


    public class UpdateProgrammeDto
    {
        public Guid Id { get; set; }
        public string ProgrammeName { get; set; }
        public long ProgrammeFee { get; set; }
    }
    public class CreateProgrammeDto
    {
        public string ProgrammeName { get; set; }
        public long ProgrammeFee { get; set; }
    }

    public class ProgrammeResponseDto
    {
        public Guid Id { get; set; }
        public string ProgrammeName { get; set; }
        public long ProgrammeFee { get; set; }
    }
}

