using AutoMapper;
using Customer.Application.Customer.Dtos;

namespace Customer.Application.MappingProfiles;

public class CustomerMappingProfile : Profile
{
    public CustomerMappingProfile()
    {
        CreateMap<Core.src.CustomerAggregate.Customer, CustomerDto>()
            .ForMember(d => d.Firstname,
                o => o.MapFrom(s => s.Name.FirstName))
            .ForMember(d => d.Lastname,
                o => o.MapFrom(s => s.Name.LastName))
            .ForMember(d => d.Email,
                o => o.MapFrom(s => s.Email.Value))
            .ForMember(d => d.Street,
                o => o.MapFrom(s => s.Address != null ? s.Address.Street : string.Empty))
            .ForMember(d => d.City,
                o => o.MapFrom(s => s.Address != null ? s.Address.HouseNumber : string.Empty))
            .ForMember(d => d.Country,
                o => o.MapFrom(s => s.Address != null ? s.Address.ZipCode : string.Empty));
    }
}