using System;
using System.Linq;
using budget_backend;
using budget_backend.application;
using budget_backend.data.dbDto;
using budget_backend.domain;
using FluentAssertions;
using Xunit;

namespace budget_backend_unit_tests.dbDtoMappings;

public class MappingTests
{
    [Fact]
    public void Account_IsMapped_ToAccountDbDto()
    {
        // Arrange
        var account = AccountFactory.Create("cash");
        
        // Act
        var dto = account.ToDbDto();

        // Assert
        dto.Id.Should().Be(account.Id);
        dto.Name.Should().Be(account.Name);
    }
    [Fact]
    public void AccountWithTransactions_IsMapped_ToTransactionDbDtos()
    {
        // // Arrange
        // var account = AccountFactory.Create("cash");
        // account.AddTransaction(new DateOnly(2020,1,1), 30);
        // account.AddTransaction(new DateOnly(2020,1,2), -40);
        //
        // // Act
        // var transactionDtos = account.GetTransactions().Select(_ => _.ToDbDto()).ToList();
        //
        // // Assert
        // transactionDtos.Should().HaveCount(2);
        // transactionDtos.Should().Contain(_ => _.Timestamp.Equals(new DateOnly(2020,1,1)) && _.Amount == 30);
        // transactionDtos.Should().Contain(_ => _.Timestamp.Equals(new DateOnly(2020,1,2)) && _.Amount == -40);
    }
    
    [Fact]
    public void AccountDbDto_IsMapped_ToAccount()
    {
        // // Arrange
        // var dto = new AccountDto()
        // {
        //     Id = Guid.NewGuid(),
        //     Name = "cash"
        // };
        //
        // // Act
        // var account = dto.ToDomain(Array.Empty<AccountTransactionDto>());
        //
        // // Assert
        // account.Id.Should().Be(dto.Id);
        // account.Name.Should().Be(dto.Name);
    }
    
    
}