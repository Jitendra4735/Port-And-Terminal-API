using Microsoft.AspNetCore.Mvc;
using WebApi.Core.Business.Interface;
using WebApi.Utilities.Models;

namespace WebApi.Api.Controllers
{
    [ApiController] // This attribute indicates that the controller responds to web API requests and automatically performs model validation.
    [Route("api/[controller]/[action]")] // Defines the route template for the controller. It maps the URL to the actions based on the controller name and action name.
    public class UserAccountController : ControllerBase
    {
        private readonly ILogger<UserAccountController> _logger; // Logger instance to log information, warnings, and errors related to this controller.
        private readonly IUserAccountManager _userAccountManager; // Interface for handling business logic related to User Account Manager.
        private readonly IAuthenticationProcessor _authenticationProcessor; // Interface for handling business logic related to Authentication.

        /// <summary>
        /// Constructor injection of the logger , UserAccountManager Processor and AuthenticationProcessor.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="userAccountManager"></param>
        /// <param name="authenticationProcessor"></param>
        public UserAccountController(ILogger<UserAccountController> logger, IUserAccountManager userAccountManager, IAuthenticationProcessor authenticationProcessor)
        {
            _logger = logger;
            _userAccountManager = userAccountManager;
            _authenticationProcessor = authenticationProcessor;
        }

        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> RegisterUser(UserAccount userAccount)
        {
            _logger.LogInformation($"RegisterUser method to Register new user in Database **STARTS** with UserAccount = {userAccount.ToString()}");
            if (ModelState.IsValid)
            {
                // Check if the username or email is already taken
                if (_userAccountManager.IsUserExists(userAccount).Result) // Calls the IsUserExists method from the UserAccountManager to check if user already exists.
                {
                    _logger.LogWarning("Registration failed: Username or email already exists.");
                    return BadRequest(new { message = "Username or email already exists" });
                }

                await _userAccountManager.RegisterUser(userAccount); // Calls the RegisterUser method from the UserAccountManager to register new user.
                _logger.LogInformation($"RegisterUser method to Register new user in Database **END** with User registered successfully message");
                return Ok(new { message = "User registered successfully" });
            }
            _logger.LogWarning("RegisterUser method to Register new user in Database **END** with Invalid model state for RegisterUser message.");
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Generates a JWT token for a user based on their credentials.
        /// </summary>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetToken(UserAccount userAccount)
        {
            _logger.LogInformation($"GetToken method to Generates a JWT token for a user based on their credentials **STARTS** with UserAccount = {userAccount.ToString()}");
            if (ModelState.IsValid)
            {
                if (_userAccountManager.VerifyUserCredentials(userAccount).Result) // Calls the VerifyUserCredentials method from the UserAccountManager to verify credential to generate token.
                {
                    var token = await _authenticationProcessor.GenerateJwtToken(userAccount); // Calls the GenerateJwtToken method from the AuthenticationProcessor to generate token.
                    _logger.LogInformation($"GetToken method to Generates a JWT token for a user based on their credentials **END** with Token = {token}");
                    return Ok(new { Token = token });

                }
                _logger.LogWarning("Token generation failed: Invalid username or password.");
                return Unauthorized(new { message = "Invalid username or password" });
            }
            _logger.LogWarning("GetToken method to Generates a JWT token for a user based on their credentials **END** with Invalid model state for GetToken message.");
            return BadRequest(ModelState);
        }
    }
}
