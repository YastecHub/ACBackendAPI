using ACBackendAPI.Application.Common.Responses;

namespace ACBackendAPI.Application.Settings.Email
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
