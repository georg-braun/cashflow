using AutoMapper;
using budget_backend.domain;

namespace budget_backend.mapping;

// used by auto mapper
// ReSharper disable once UnusedType.Global
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Account
        CreateMap<domain.Account, data.dbDto.Account>();
        CreateMap<data.dbDto.Account, domain.Account>();
        
        // Transaction
        CreateMap<domain.Transaction, data.dbDto.Transaction>()
            .ForMember(dtoTransaction => dtoTransaction.AccountId,
                opt => opt.MapFrom<TransactionAccountToAccountIdResolver>());
        CreateMap<data.dbDto.Transaction, domain.Transaction>();
    }
}

/// <summary>
///     Resolve the linked account of the transaction to a account id in the transaction dto.
/// </summary>
public class TransactionAccountToAccountIdResolver : IValueResolver<Transaction, data.dbDto.Transaction, Guid>
{
    public Guid Resolve(Transaction source, data.dbDto.Transaction destination, Guid destMember, ResolutionContext context)
    {
        return source.Account.Id;
    }
}