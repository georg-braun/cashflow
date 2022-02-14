using System;
using budget_backend;
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
        var account = AccountFactory.Create("DKB");
        account.AddTransaction(DateOnlyExtensions.Today(), 50);
        account.AddTransaction(DateOnlyExtensions.Today(), 50);
        account.AddTransaction(DateOnlyExtensions.Today(), -20);

        // Act
        var totalBalance = account.Balance;

        // Assert
        totalBalance.Should().Be(80);
        
    }
}