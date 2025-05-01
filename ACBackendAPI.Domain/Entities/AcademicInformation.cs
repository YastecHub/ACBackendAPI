using ACBackendAPI.Domain.Enum;

namespace ACBackendAPI.Domain.Entities
{
    public class AcademicInformation : BaseEntity
    {
        public Department Department { get; set; } 
        public string DepartmentDesc { get; set; }

        public LessonType LessonType { get; set; }
        public string LessonTypeDesc { get; set; }

        public string CourseOfStudy { get; set; }

        public Guid ProgrammeId { get; set; }
        public Programme Programme { get; set; }

        public Guid StudentId { get; set; }
        public Student Student { get; set; }

        public AcademicInformation()
        {
            DepartmentDesc = Department.ToString();
            LessonTypeDesc = LessonType.ToString();
        }
    }
}
