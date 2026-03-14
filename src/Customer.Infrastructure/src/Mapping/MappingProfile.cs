using AutoMapper;

namespace Customer.Infrastructure.src.Mapping;

public class CustomerMappingProfiles : Profile
{
    public CustomerMappingProfiles()
    {
        CreateMap<Core.src.CustomerAggregate.Customer,Persistance.Model.Customer>()
            .ForMember(d => d.Id,
                o => o.MapFrom(s => s.Id.Value)
            ).ForMember(d => d.FirstName, 
                o => o.MapFrom(s => s.Name.FirstName)
            ).ForMember(d => d.LastName, 
                o => o.MapFrom(s => s.Name.LastName)
            ).ForMember(d => d.Email, 
                o => o.MapFrom(s => s.Email.Value)
            ).ForMember(d => d.HouseNumber, 
                o => o.MapFrom(s => s.Address != null ? s.Address.HouseNumber : string.Empty)
            ).ForMember(d => d.ZipCode, 
                o => o.MapFrom(s => s.Address != null ? s.Address.ZipCode : string.Empty)
            ).ForMember(d => d.Street,
                o => o.MapFrom(s => s.Address != null ? s.Address.Street : string.Empty)
            );
    }
}