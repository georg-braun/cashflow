using System;
using budget_backend.domain;
using FluentAssertions;
using Xunit;

namespace budget_backend_unit_tests;

public class AccountTests
{
    [Fact]
    public void Balance_WithMultipleTransactions_ReturnCorrectBalance()
    {
        // Arrange
        var accountService = new AccountService();
        accountService.Add("Cash");
        accountService.TryGet("Cash", out var account);
        account.AddTransaction(TransactionFactory.Create(DateTime.Now, 50));
        account.AddTransaction(TransactionFactory.Create(DateTime.Now, 50));
        account.AddTransaction(TransactionFactory.Create(DateTime.Now, -20));
        
        // Act
        var totalBalance = account.Balance;
        
        // Assert
        totalBalance.Should().Be(80);
    }

}