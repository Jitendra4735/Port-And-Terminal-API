using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApi.Core.Business.Interface;
using WebApi.Infrastructure.Models;
using WebApi.Infrastructure.Repository.Interface;
using WebApi.Utilities.Models;

namespace WebApi.Core.Business
{
    public class UserAccountManager : IUserAccountManager
    {
        private readonly ILogger<UserAccountManager> _logger;
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IPasswordHasher<UserInfo> _passwordHasher;
        public UserAccountManager(ILogger<UserAccountManager> logger, IApplicationDbContext applicationDbContext, IPasswordHasher<UserInfo> passwordHasher)
        {
            _logger = logger;
            _applicationDbContext = applicationDbContext;
            _passwordHasher = passwordHasher;
        }

        /// <summary>
        /// Checks if a user with the given username or email already exists in the database.
        /// </summary>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        public async Task<bool> IsUserExists(UserAccount userAccount)
        {
            _logger.LogInformation($"IsUserExists method to Checks if a user with the given username or email already exists in the database **STARTS** with UserAccount = {userAccount.ToString()}");
            bool isExists = await _applicationDbContext.UserInfo.AnyAsync(u => u.Username == userAccount.Username || u.Email == userAccount.Email);
            _logger.LogInformation($"IsUserExists method to Checks if a user with the given username or email already exists in the database **END** with IsExists = {isExists}");
            return isExists;
        }

        /// <summary>
        /// Registers a new user by creating a new user record and saving it to the database.
        /// </summary>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        public async Task RegisterUser(UserAccount userAccount)
        {
            _logger.LogInformation($"RegisterUser method to Registers a new user by creating a new user record and saving it to the database **STARTS** with UserAccount = {userAccount.ToString()}");
            // Create a new user entity
            var user = new UserInfo
            {
                Username = userAccount.Username,
                Email = userAccount.Email
            };
            // Hash the password before saving it to database
            user.PasswordHash = _passwordHasher.HashPassword(user, userAccount.Password);
            _applicationDbContext.UserInfo.Add(user); // Add the new user to the database
            await _applicationDbContext.SaveChangesAsync(); // Saves the changes to the database.
            _logger.LogInformation($"RegisterUser method to Registers a new user by creating a new user record and saving it to the database **END**");
        }

        /// <summary>
        /// Verifies user credentials by checking the username and validating the password.
        /// </summary>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        public async Task<bool> VerifyUserCredentials(UserAccount userAccount)
        {
            _logger.LogInformation($"VerifyUserCredentials method to Verifies user credentials by checking the username and validating the password **STARTS** with UserAccount = {userAccount.ToString()}");

            var user = await _applicationDbContext.UserInfo.SingleOrDefaultAsync(u => u.Username == userAccount.Username);

            if (user == null || _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, userAccount.Password) == PasswordVerificationResult.Failed)
            {
                _logger.LogWarning($"User not found with Username: {userAccount.Username}");
                return false;
            }
            else
            {
                _logger.LogInformation($"VerifyUserCredentials method to Verifies user credentials by checking the username and validating the password **END**");
                return true;
            }
        }
    }
}
