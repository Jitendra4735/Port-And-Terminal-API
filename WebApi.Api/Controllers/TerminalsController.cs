using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Business.Business.Interface;
using WebApi.Business.Models;

namespace WebApi.Api.Controllers
{
    [Authorize] // This attribute ensures that only authenticated users can access the actions in this controller.
    [ApiController] // This attribute indicates that the controller responds to web API requests and automatically performs model validation.
    [Route("api/[controller]/[action]")] // Defines the route template for the controller. It maps the URL to the actions based on the controller name and action name.
    public class TerminalsController : ControllerBase
    {
        private readonly ILogger<TerminalsController> _logger; // Logger instance to log information, warnings, and errors related to this controller.
        private readonly ITerminalProcessor _terminalProcessor; // Interface for handling business logic related to terminals.

        /// <summary>
        /// Constructor injection of the logger and the terminal processor.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="portProcessor"></param>
        public TerminalsController(ILogger<TerminalsController> logger, ITerminalProcessor terminalProcessor)
        {
            _logger = logger;
            _terminalProcessor = terminalProcessor;
        }

        /// <summary>
        /// Action method to retrieve all terminals.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TerminalViewModel>>> GetTerminals()
        {
            _logger.LogInformation("API Layer -> GetTerminals action to retrieve all terminals **STARTS**");
            IEnumerable<TerminalViewModel> terminals = await _terminalProcessor.GetTerminals(); // Calls the GetTerminals method from the terminal processor to retrieve the list of terminals.
            _logger.LogInformation($"API Layer -> GetTerminals action to retrieve all terminals **END** with IEnumerable<TerminalViewModel> = {terminals.ToString()}");
            return Ok(terminals.ToList()); // Returns the list of terminals with a 200 OK status code.
        }

        /// <summary>
        /// Action method to retrieve a specific terminal by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<TerminalViewModel>> GetTerminal(int id)
        {
            _logger.LogInformation($"API Layer -> GetTerminal action to retrieve a specific terminal by its ID **STARTS** with Id = {id}");
            TerminalViewModel terminal = await _terminalProcessor.GetTerminal(id); // Calls the GetTerminal method from the terminal processor to retrieve a terminal by its ID.
            _logger.LogInformation($"API Layer -> GetTerminal action to retrieve a specific terminal by its ID **END** with TerminalViewModel = {terminal.ToString()}");
            return Ok(terminal); // Returns the terminal details with a 200 OK status code.
        }

        /// <summary>
        /// Action method to create a new terminal.
        /// </summary>
        /// <param name="terminalToCreate"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<TerminalViewModel>> CreateTerminal(Terminal terminalToCreate)
        {
            _logger.LogInformation($"API Layer -> CreateTerminal action to create a new terminal **STARTS** with Terminal = {terminalToCreate.ToString()}");
            TerminalViewModel terminal = await _terminalProcessor.CreateTerminal(terminalToCreate);// Calls the CreateTerminal method from the terminal processor to create a new terminal.
            _logger.LogInformation($"API Layer -> CreateTerminal action to create a new terminal **END** with TerminalViewModel = {terminal.ToString()}");
            return CreatedAtAction(nameof(GetTerminal), new { id = terminal.Id }, terminal);// Returns the created terminal with a 201 Created status code.
        }

        /// <summary>
        ///  Action method to update an existing terminal.
        /// </summary>
        /// <param name="terminalToUpdate"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateTerminal(TerminalEditModel terminalToUpdate)
        {
            _logger.LogInformation($"API Layer -> UpdateTerminal action to update an existing terminal **STARTS** with TerminalEditModel = {terminalToUpdate.ToString()}");
            await _terminalProcessor.UpdateTerminal(terminalToUpdate);// Calls the UpdateTerminal method from the terminal processor to update an existing terminal.
            _logger.LogInformation($"API Layer -> UpdateTerminal action to update an existing terminal **END**");
            return Ok("Resource updated successfully");// Returns a 200 OK status code with a success message.
        }

        /// <summary>
        /// Action method to delete a terminal by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTerminal(int id)
        {
            _logger.LogInformation($"API Layer -> DeleteTerminal action to delete a terminal by its ID **STARTS** with Id = {id}");
            await _terminalProcessor.DeleteTerminal(id);// Calls the DeleteTerminal method from the terminal processor to delete a terminal by its ID.
            _logger.LogInformation($"API Layer -> DeleteTerminal action to delete a terminal by its ID **END**");
            return Ok("Resource removed successfully");// Returns a 200 OK status code with a success message.
        }
    }
}
