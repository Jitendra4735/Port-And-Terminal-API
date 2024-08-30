using AutoMapper;
using WebApi.Business.Models;
using WebApi.Data.Models;

namespace WebApi.Business.Mapper
{
    public class MappingProfile : Profile
    {
        // Constructor to configure mapping profiles using AutoMapper.
        public MappingProfile()
        {
            // Creates a bidirectional mapping between Port and PortDto.
            CreateMap<Port, PortDto>()
                .ReverseMap(); // Maps from Port to PortDto and vice versa.

            // Creates a bidirectional mapping between PortDto and PortViewModel.
            CreateMap<PortDto, PortViewModel>()
                .ReverseMap(); // Maps from PortDto to PortViewModel and vice versa.

            // Creates a bidirectional mapping between Terminal and TerminalDto.
            CreateMap<Terminal, TerminalDto>()
                .ReverseMap(); // Maps from Terminal to TerminalDto and vice versa.

            // Creates a bidirectional mapping between TerminalDto and TerminalViewModel.
            CreateMap<TerminalDto, TerminalViewModel>()
                .ReverseMap(); // Maps from TerminalDto to TerminalViewModel and vice versa.
        }
    }
}
