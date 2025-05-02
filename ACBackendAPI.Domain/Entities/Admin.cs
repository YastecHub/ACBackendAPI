using ACBackendAPI.Domain.Entities;

public class Admin : BaseEntity 
{
    public string Email { get; set; }
    public string Avatar { get; set; }
    public string Name { get; set; }
    public string Gender { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }

    public Guid ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }
}