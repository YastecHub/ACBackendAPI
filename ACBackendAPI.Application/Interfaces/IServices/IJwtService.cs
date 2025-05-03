using ACBackendAPI.Domain.Entities;

namespace ACBackendAPI.Application.Interfaces.IServices
{
    public interface IJwtService
    {
        string GenerateToken(ApplicationUser user, IList<string> roles);
    }
}
