using WebApi.Utilities.Models;

namespace WebApi.Core.Business.Interface
{
    public interface IUserAccountManager
    {
        Task<bool> IsUserExists(UserAccount userAccount);
        Task RegisterUser(UserAccount userAccount);
        Task<bool> VerifyUserCredentials(UserAccount userAccount);
    }
}
