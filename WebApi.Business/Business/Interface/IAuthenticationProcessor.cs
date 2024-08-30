using WebApi.Utilities.Models;

namespace WebApi.Core.Business.Interface
{
    public interface IAuthenticationProcessor
    {
        Task<string> GenerateJwtToken(UserAccount userAccount);
    }
}
