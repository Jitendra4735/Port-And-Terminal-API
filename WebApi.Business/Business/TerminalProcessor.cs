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
    // The TerminalProcessor class implements the ITerminalProcessor interface and handles the business logic for managing terminals.
    public class TerminalProcessor : ITerminalProcessor
    {
        private readonly ILogger<TerminalProcessor> _logger; // Logger instance to log information, warnings, and errors.
        private readonly IApplicationDbContext _applicationDbContext; // Interface for the application's database context.
        private readonly IMapper _mapper; // AutoMapper instance to map between different object models.

        /// <summary>
        /// Constructor with dependency injection for logger, database context, and AutoMapper.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="applicationDbContext"></param>
        /// <param name="mapper"></param>
        public TerminalProcessor(ILogger<TerminalProcessor> logger, IApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _logger = logger;
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Method to retrieve all terminals along with their ports.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TerminalViewModel>> GetTerminals()
        {
            _logger.LogInformation("Business Layer -> GetTerminals method to retrieve all terminals **STARTS**");
            // Retrieves the list of terminals from the database, including related port data.
            List<TerminalDto> terminalData = await _applicationDbContext.Terminals.Include(t => t.Port).ToListAsync();

            // Maps the list of TerminalDto objects to a list of TerminalViewModel objects using AutoMapper.
            IEnumerable<TerminalViewModel> terminalViewModel = _mapper.Map<IEnumerable<TerminalViewModel>>(terminalData);
            _logger.LogInformation($"Business Layer -> GetTerminals method to retrieve all terminals **END** with IEnumerable<TerminalViewModel> = {terminalViewModel.ToString()}");
            return terminalViewModel;
        }

        /// <summary>
        /// Method to retrieve a specific terminal by its ID, including its port.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="HttpClientException"></exception>
        public async Task<TerminalViewModel> GetTerminal(int id)
        {
            _logger.LogInformation($"Business Layer -> GetTerminal method to retrieve a specific terminal by its ID **STARTS** with Id = {id}");

            //Retrieves a single terminal by its ID from the database, including related Port data.
            TerminalDto terminalData = await _applicationDbContext.Terminals.Include(t => t.Port).FirstOrDefaultAsync(t => t.Id == id);

            // Throws an exception if no terminal is found with the specified ID.
            if (terminalData == null)
            {
                throw new HttpClientException(System.Net.HttpStatusCode.NotFound, "Requested resource not found.");
            }
            TerminalViewModel terminalViewModel = _mapper.Map<TerminalViewModel>(terminalData); // Maps the retrieved TerminalDto object to a TerminalViewModel object using AutoMapper.
            _logger.LogInformation($"Business Layer -> GetTerminal method to retrieve a specific terminal by its ID **END** with TerminalViewModel = {terminalViewModel.ToString()}");
            return terminalViewModel;
        }

        /// <summary>
        /// Method to create a new terminal.
        /// </summary>
        /// <param name="terminalToCreate"></param>
        /// <returns></returns>
        /// <exception cref="HttpClientException"></exception>
        public async Task<TerminalViewModel> CreateTerminal(Terminal terminalToCreate)
        {
            _logger.LogInformation($"Business Layer -> CreateTerminal method to create a new terminal **STARTS** with Terminal = {terminalToCreate.ToString()}");

            // Checks if a terminal with the same name already exists for the specified port
            if (_applicationDbContext.Terminals.Any(t => t.Name == terminalToCreate.Name && t.PortId == terminalToCreate.PortId))
            {
                throw new HttpClientException(System.Net.HttpStatusCode.BadRequest, "Terminal name must be unique for the port.");
            }
            TerminalDto createdTerminal = _mapper.Map<TerminalDto>(terminalToCreate); // Maps the provided Terminal object to a TerminalDto object using AutoMapper.
            _applicationDbContext.Terminals.Add(createdTerminal); // Adds the new terminal to the database context.
            await _applicationDbContext.SaveChangesAsync();// Saves the changes to the database.

            // Retrieves the created TerminalViewModel for the response.
            TerminalViewModel terminalResponse = await GetTerminal(createdTerminal.Id);
            _logger.LogInformation($"Business Layer -> CreateTerminal method to create a new terminal **END** with TerminalViewModel = {terminalResponse.ToString()}");
            return terminalResponse;
        }

        /// <summary>
        /// Method to update an existing terminal.
        /// </summary>
        /// <param name="terminalToUpdate"></param>
        /// <returns></returns>
        /// <exception cref="HttpClientException"></exception>
        public async Task UpdateTerminal(TerminalEditModel terminalToUpdate)
        {
            _logger.LogInformation($"Business Layer -> UpdateTerminal method to update an existing terminal **STARTS** with TerminalEditModel = {terminalToUpdate.ToString()}");

            // Retrieves the existing terminal by its ID.
            var existingTerminal = await _applicationDbContext.Terminals.FindAsync(terminalToUpdate.Id);
            // Throws an exception if no terminal is found with the specified ID.
            if (existingTerminal == null)
            {
                throw new HttpClientException(System.Net.HttpStatusCode.NotFound, "Request resource not found to update");
            }
            // Checks if a terminal with the same name already exists for the specified port, excluding the current terminal.
            if (_applicationDbContext.Terminals.Any(t => t.Name == terminalToUpdate.Name && t.PortId == terminalToUpdate.PortId && t.Id != terminalToUpdate.Id))
            {
                throw new HttpClientException(System.Net.HttpStatusCode.BadRequest, "Terminal name must be unique for the port.");
            }

            // Maps the existing terminal data to a TerminalDto object using AutoMapper.
            TerminalDto updatedTerminal = _mapper.Map<TerminalDto>(existingTerminal);
            // Updates the properties of the terminal with the provided data.
            updatedTerminal.Name = terminalToUpdate.Name;
            updatedTerminal.PortId = terminalToUpdate.PortId;
            updatedTerminal.Latitude = terminalToUpdate.Latitude;
            updatedTerminal.Longitude = terminalToUpdate.Longitude;
            updatedTerminal.IsActive = terminalToUpdate.IsActive;
            updatedTerminal.LastEditedDate = DateTime.UtcNow;

            // Updates the terminal in the database context.
            _applicationDbContext.Terminals.Update(updatedTerminal);
            // Saves the changes to the database.
            await _applicationDbContext.SaveChangesAsync();
            _logger.LogInformation($"Business Layer -> UpdateTerminal method to update an existing terminal **END**");
        }

        /// <summary>
        /// Method to delete an existing terminal by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="HttpClientException"></exception>
        public async Task DeleteTerminal(int id)
        {
            _logger.LogInformation($"Business Layer -> DeleteTerminal method to delete a terminal by its ID **STARTS** with Id = {id}");

            // Retrieves the terminal to be deleted by its ID.
            var terminalToRemove = await _applicationDbContext.Terminals.FindAsync(id);
            // Throws an exception if no terminal is found with the specified ID.
            if (terminalToRemove == null)
            {
                throw new HttpClientException(System.Net.HttpStatusCode.NotFound, "Requested resource not found to delete");
            }
            // Removes the terminal from the database context.
            _applicationDbContext.Terminals.Remove(terminalToRemove);
            // Saves the changes to the database.
            await _applicationDbContext.SaveChangesAsync();

            _logger.LogInformation($"Business Layer -> DeleteTerminal method to delete a terminal by its ID **END**");
        }
    }
}
