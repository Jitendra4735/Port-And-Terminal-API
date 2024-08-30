using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Business.Business.Interface;
using WebApi.Business.Models;

namespace WebApi.Api.Controllers
{
    [Authorize] // This attribute ensures that only authenticated users can access the actions in this controller.
    [ApiController] // This attribute indicates that the controller responds to web API requests and automatically performs model validation.
    [Route("api/[controller]/[action]")] // Defines the route template for the controller. It maps the URL to the actions based on the controller name and action name.
    public class PortController : ControllerBase
    {
        private readonly ILogger<PortController> _logger; // Logger instance to log information, warnings, and errors related to this controller.
        private readonly IPortProcessor _portProcessor; // Interface for handling business logic related to ports.

        /// <summary>
        /// Constructor injection of the logger and the port processor.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="portProcessor"></param>
        public PortController(ILogger<PortController> logger, IPortProcessor portProcessor)
        {
            _logger = logger;
            _portProcessor = portProcessor;
        }

        /// <summary>
        /// Action method to retrieve all ports.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PortViewModel>>> GetPorts()
        {
            _logger.LogInformation("API Layer -> GetPorts action to retrieve all ports **STARTS**");
            IEnumerable<PortViewModel> ports = await _portProcessor.GetPorts(); // Calls the GetPorts method from the port processor to retrieve the list of ports.
            _logger.LogInformation($"API Layer -> GetPorts action to retrieve all ports **END** with IEnumerable<PortViewModel> = {ports.ToString()}");
            return Ok(ports.ToList()); // Returns the list of ports with a 200 OK status code.
        }

        /// <summary>
        /// Action method to retrieve a specific port by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<PortViewModel>> GetPort(int id)
        {
            _logger.LogInformation($"API Layer -> GetPort action to retrieve a specific port by its ID **STARTS** with Id = {id}");
            PortViewModel port = await _portProcessor.GetPort(id); // Calls the GetPort method from the port processor to retrieve a port by its ID.
            _logger.LogInformation($"API Layer -> GetPort action to retrieve a specific port by its ID **END** with PortViewModel = {port.ToString()}");
            return Ok(port); // Returns the port details with a 200 OK status code.
        }

        /// <summary>
        /// Action method to create a new port.
        /// </summary>
        /// <param name="portToCreate"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<PortViewModel>> CreatePort(Port portToCreate)
        {
            _logger.LogInformation($"API Layer -> CreatePort action to create a new port **STARTS** with Port = {portToCreate.ToString()}");
            PortViewModel port = await _portProcessor.CreatePort(portToCreate); // Calls the CreatePort method from the port processor to create a new port.
            _logger.LogInformation($"API Layer -> CreatePort action to create a new port **END** with PortViewModel = {port.ToString()}");
            return CreatedAtAction(nameof(GetPort), new { id = port.Id }, port); // Returns the created port with a 201 Created status code.
        }

        /// <summary>
        /// Action method to update an existing port.
        /// </summary>
        /// <param name="portToUpdate"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdatePort(PortEditModel portToUpdate)
        {
            _logger.LogInformation($"API Layer -> UpdatePort action to update an existing port **STARTS** with PortEditModel = {portToUpdate.ToString()}");
            await _portProcessor.UpdatePort(portToUpdate); // Calls the UpdatePort method from the port processor to update an existing port.
            _logger.LogInformation($"API Layer -> UpdatePort action to update an existing port **END**");
            return Ok("Resource updated successfully"); // Returns a 200 OK status code with a success message.
        }

        /// <summary>
        /// Action method to delete a port by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePort(int id)
        {
            _logger.LogInformation($"API Layer -> DeletePort action to delete a port by its ID **STARTS** with Id = {id}");
            await _portProcessor.DeletePort(id); // Calls the DeletePort method from the port processor to delete a port by its ID.
            _logger.LogInformation($"API Layer -> DeletePort action to delete a port by its ID **END**");
            return Ok("Resource removed successfully"); // Returns a 200 OK status code with a success message.
        }
    }
}
