using AutoMapper;

namespace budget_backend.mapping;

// used by auto mapper
// ReSharper disable once UnusedType.Global
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<domain.Account, data.dbDto.Account>();
        CreateMap<data.dbDto.Account, domain.Account>();
    }
}