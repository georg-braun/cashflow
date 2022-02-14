using System;
using System.Linq;
using AutoMapper;
using budget_backend.domain;
using FluentAssertions;
using Xunit;

namespace budget_backend_unit_tests.mappings;

public class MappingTests
{
    [Fact]
    public void Test_AccountToAccountDbDto_Mapping()
    {
        var configuration = new MapperConfiguration(cfg =>
            cfg.CreateMap<Account, budget_backend.data.dbDto.Account>());

        configuration.AssertConfigurationIsValid();
    }
    
    [Fact]
    public void Test_TransactionToTransactionDbDto_Mapping()
    {
        var configuration = new MapperConfiguration(cfg =>
            cfg.CreateMap<Transaction, budget_backend.data.dbDto.Transaction>());
        
        configuration.AssertConfigurationIsValid();
    }

    [Fact]
    public void Transaction_LinkedAccountIsMappedToAccountId()
    {
        // Arrange
        var account = AccountFactory.Create("DKB");
        account.AddTransaction(DateOnly.FromDateTime(DateTime.Now), 50);
        var transaction = account.GetTransactions().First();

        var configuration = new MapperConfiguration(cfg =>
            cfg.CreateMap<Transaction, budget_backend.data.dbDto.Transaction>());

        var mapper = configuration.CreateMapper();

        // Act
        var transactionDto = mapper.Map<Transaction, budget_backend.data.dbDto.Transaction>(transaction);
        
        // Assert
        transactionDto.AccountId.Should().Be(account.Id);
    }
}