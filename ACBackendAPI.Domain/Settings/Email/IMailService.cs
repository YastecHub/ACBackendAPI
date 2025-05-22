
namespace ACBackendAPI.Domain.Settings.Email
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
