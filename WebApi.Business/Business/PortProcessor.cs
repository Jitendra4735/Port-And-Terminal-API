using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApi.Business.Business.Interface;
using WebApi.Business.Models;
using WebApi.Data.Models;
using WebApi.Infrastructure.Repository.Interface;
using WebApi.Utilities;

namespace WebApi.Business.Business
{
    // The PortProcessor class implements the IPortProcessor interface and handles the business logic for managing ports.
    public class PortProcessor : IPortProcessor
    {
        private readonly ILogger<PortProcessor> _logger; // Logger instance to log information, warnings, and errors.
        private readonly IApplicationDbContext _applicationDbContext; // Interface for the application's database context.
        private readonly IMapper _mapper; // AutoMapper instance to map between different object models.

        /// <summary>
        /// Constructor with dependency injection for logger, database context, and AutoMapper.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="applicationDbContext"></param>
        /// <param name="mapper"></param>
        public PortProcessor(ILogger<PortProcessor> logger, IApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _logger = logger;
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Method to retrieve all ports along with their terminals.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<PortViewModel>> GetPorts()
        {
            _logger.LogInformation("Business Layer -> GetPorts method to retrieve all ports **STARTS**");

            // Retrieves the list of ports from the database, including related terminal data.
            List<PortDto> portData = await _applicationDbContext.Ports.Include(p => p.Terminals).ToListAsync();

            // Maps the list of PortDto objects to a list of PortViewModel objects.
            IEnumerable<PortViewModel> portViewModel = _mapper.Map<IEnumerable<PortViewModel>>(portData);

            _logger.LogInformation($"Business Layer -> GetPorts method to retrieve all ports **END** with IEnumerable<PortViewModel> = {portViewModel.ToString()}");
            return portViewModel;
        }

        /// <summary>
        /// Method to retrieve a specific port by its ID, including its terminals.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="HttpClientException"></exception>
        public async Task<PortViewModel> GetPort(int id)
        {
            _logger.LogInformation($"Business Layer -> GetPort method to retrieve a specific port by its ID **STARTS** with Id = {id}");

            // Retrieves the port from the database based on its ID, including related terminal data.
            PortDto portData = await _applicationDbContext.Ports.Include(p => p.Terminals).FirstOrDefaultAsync(p => p.Id == id);

            // Throws an exception if the port is not found.
            if (portData == null)
            {
                throw new HttpClientException(System.Net.HttpStatusCode.NotFound, "Requested resource not found.");
            }

            PortViewModel portViewModel = _mapper.Map<PortViewModel>(portData); // Maps the PortDto object to a PortViewModel object.

            _logger.LogInformation($"Business Layer -> GetPort method to retrieve a specific port by its ID **END** with PortViewModel = {portViewModel.ToString()}");
            return portViewModel;
        }

        /// <summary>
        /// Method to create a new port.
        /// </summary>
        /// <param name="portToCreate"></param>
        /// <returns></returns>
        /// <exception cref="HttpClientException"></exception>
        public async Task<PortViewModel> CreatePort(Port portToCreate)
        {
            _logger.LogInformation($"Business Layer -> CreatePort method to create a new port **STARTS** with Port = {portToCreate.ToString()}");

            // Checks if a port with the same code already exists, ensuring uniqueness of the port code.
            if (_applicationDbContext.Ports.Any(p => p.Code == portToCreate.Code))
            {
                throw new HttpClientException(System.Net.HttpStatusCode.BadRequest, "Port code must be unique.");
            }

            PortDto createdPort = _mapper.Map<PortDto>(portToCreate); // Maps the Port object to a PortDto object for database insertion.
            _applicationDbContext.Ports.Add(createdPort); // Adds the new port to the database.
            await _applicationDbContext.SaveChangesAsync(); // Saves the changes to the database.
            PortViewModel portViewModel = _mapper.Map<PortViewModel>(createdPort); // Maps the created PortDto object to a PortViewModel object.
            portViewModel.Id = createdPort.Id; // Assigns the ID of the created port.

            _logger.LogInformation($"Business Layer -> CreatePort method to create a new port **END** with PortViewModel = {portViewModel.ToString()}");
            return portViewModel;
        }

        /// <summary>
        /// Method to update an existing port.
        /// </summary>
        /// <param name="portToUpdate"></param>
        /// <returns></returns>
        /// <exception cref="HttpClientException"></exception>
        public async Task UpdatePort(PortEditModel portToUpdate)
        {
            _logger.LogInformation($"Business Layer -> UpdatePort method to update an existing port **STARTS** with PortEditModel = {portToUpdate.ToString()}");

            // Retrieves the existing port from the database based on its ID.
            var existingPort = await _applicationDbContext.Ports.FindAsync(portToUpdate.Id);
            if (existingPort == null)
            {
                throw new HttpClientException(System.Net.HttpStatusCode.NotFound, "Request resource not found to update");
            }

            // Checks if a port with the same code (but different ID) already exists, ensuring uniqueness of the port code.
            if (_applicationDbContext.Ports.Any(p => p.Code == portToUpdate.Code && p.Id != portToUpdate.Id))
            {
                throw new HttpClientException(System.Net.HttpStatusCode.BadRequest, "Port code must be unique.");
            }

            PortDto updatedPort = _mapper.Map<PortDto>(existingPort); // Maps the existing PortDto object to the updated PortDto object.
            updatedPort.Name = portToUpdate.Name; // Updates the port name.
            updatedPort.Code = portToUpdate.Code; // Updates the port code.
            updatedPort.LastEditedDate = DateTime.UtcNow; // Updates the last edited date.

            // Updates the port in the database.
            _applicationDbContext.Ports.Update(updatedPort);
            await _applicationDbContext.SaveChangesAsync(); // Saves the changes to the database.

            _logger.LogInformation($"Business Layer -> UpdatePort method to update an existing port **END**");
        }

        /// <summary>
        /// Method to delete an existing port by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="HttpClientException"></exception>
        public async Task DeletePort(int id)
        {
            _logger.LogInformation($"Business Layer -> DeletePort method to delete a port by its ID **STARTS** with Id = {id}");

            // Retrieves the port to be removed from the database.
            var portToRemove = await _applicationDbContext.Ports.FindAsync(id);
            if (portToRemove == null)
            {
                throw new HttpClientException(System.Net.HttpStatusCode.NotFound, "Requested resource not found to delete");
            }

            // Removes the port from the database.
            _applicationDbContext.Ports.Remove(portToRemove);
            await _applicationDbContext.SaveChangesAsync(); // Saves the changes to the database.

            _logger.LogInformation($"Business Layer -> DeletePort method to delete a port by its ID **END**");
        }
    }
}
