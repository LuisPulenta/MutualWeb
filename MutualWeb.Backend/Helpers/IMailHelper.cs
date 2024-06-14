using MutualWeb.Shared.Responses;

namespace MutualWeb.Backend.Helpers
{
    public interface IMailHelper
    {
        ActionResponse<string> SendMail(string toName, string toEmail, string subject, string body);
    }
}
