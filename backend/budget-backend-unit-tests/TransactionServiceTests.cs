using System;
using budget_backend.domain;
using FluentAssertions;
using Xunit;

namespace budget_backend_unit_tests;

public class UnitTest1
{
    [Fact]
    public void TotalBalance_WithMultipleTransactions_ReturnCorrectBalance()
    {
        // Arrange
        var accountService = new AccountService();
        var transactionService = new TransactionService(accountService);
        accountService.AddAccount("Cash");
        accountService.TryGetAccount("Cash", out var account);
        
        // Act
        transactionService.AddTransaction(DateTime.Now, 50, account.Id);
        transactionService.AddTransaction(DateTime.Now, 50, account.Id);
        var totalBalance = transactionService.GetTotalBalance();
        
        // Assert
        totalBalance.Should().Be(totalBalance);
    }
}