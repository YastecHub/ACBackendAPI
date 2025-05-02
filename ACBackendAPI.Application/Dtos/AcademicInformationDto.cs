using ACBackendAPI.Domain.Enum;
using System.Text.Json.Serialization;

public class AcademicInformationDto
{
    public Department Department { get; set; }
    [JsonPropertyName("courseOfStudy")]
    public string CourseOfStudy { get; set; }
    public Guid ProgrammeId { get; set; }
}