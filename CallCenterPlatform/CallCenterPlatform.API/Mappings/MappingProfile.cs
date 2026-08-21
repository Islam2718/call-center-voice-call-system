using AutoMapper;
using CallCenterPlatform.Domain.Entities;
using CallCenterPlatform.Application.DTOs;

namespace CallCenterPlatform.API.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Client, ClientDto>()
            .ForMember(dest => dest.CompanyType, 
                opt => opt.MapFrom(src => src.CompanyType.ToString()));
        
        // If needed for other mappings
        // CreateMap<CreateClientRequestDto, Client>();
        // CreateMap<UpdateClientRequestDto, Client>();
    }
}