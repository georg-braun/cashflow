using budget_backend;
using budget_backend.domain;
using budget_backend.domain.budget;
using FluentAssertions;
using Xunit;

namespace budget_backend_unit_tests;

public class BudgetTests
{
    // [Fact]
    // public void BudgetBalance_IsCorrectAfterAssignmentsAndSpendings()
    // {
    //     // Arrange
    //     var carInsuranceBudget = BudgetFactory.Create("car insuarance");
    //     carInsuranceBudget.AddChange(100, DateOnlyExtensions.Today());
    //     carInsuranceBudget.AddChange(-20, DateOnlyExtensions.Today());
    //    
    //     
    //     var account = AccountFactory.Create("cash");
    //     account.Spend(DateOnlyExtensions.Today(), 30, carInsuranceBudget);
    //     
    //     // Act
    //     var remaining = carInsuranceBudget.Remaining();
    //     
    //     // Assert
    //     remaining.Should().Be(50);
    // }
}