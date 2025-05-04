namespace ACBackendAPI.Domain.Entities
{
    public class Guardian : BaseEntity
    {
        public string FistName { get; set; }
        public string LastName { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string RelationShip { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public List<Student> Students { get; set; }
    }
}
