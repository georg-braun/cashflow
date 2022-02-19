using System;
using budget_backend;
using budget_backend.domain;
using FluentAssertions;
using Xunit;

namespace budget_backend_unit_tests;

public class AccountTests
{
    [Fact]
    public void Balance_AfterPayInAndMoneyMovements_IsCorrect()
    {
        // Arrange
        var accountCash = AccountFactory.Create("Cash");
        var accountDkb = AccountFactory.Create("Dkb");

        accountCash.AddEntry(100, DateOnlyExtensions.Today());

        var transaction = AccountTransactionFactory.Create(accountCash, accountDkb, 30, DateOnlyExtensions.Today());
        
        // Act
        var totalBalanceCash = accountCash.GetBalance();

        // Assert
        totalBalanceCash.Should().Be(70);
    }
}