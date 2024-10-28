using Xunit;
using Moq;
using FinanceManager.BL.Interfaces;
using FinanceManager.BL.Models;

namespace FinanceManager.Tests.UnitTests;

public class DebitTransactionTests
{
    [Fact]
    public void Constructor_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        var transactionId = Guid.NewGuid();
        var clientId = Guid.NewGuid();
        var dateTime = DateTime.UtcNow;
        var amount = 100.0m;

        // Act
        var transaction = new DebitTransaction(transactionId, clientId, dateTime, amount);

        // Assert
        Assert.Equal(transactionId, transaction.Id);
        Assert.Equal(clientId, transaction.ClientId);
        Assert.Equal(dateTime, transaction.DateTime);
        Assert.Equal(amount, transaction.Amount);
        Assert.Null(transaction.RevertedAt);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenDateIsInFuture()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() =>
            new DebitTransaction(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow.AddDays(1), 100.0m));
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenAmountIsZeroOrNegative()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() =>
            new DebitTransaction(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, 0.0m));

        Assert.Throws<ArgumentException>(() =>
            new DebitTransaction(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, -100.0m));
    }

    [Fact]
    public void Process_ShouldDecreaseBalance()
    {
        // Arrange
        var financeManagerMock = new Mock<IFinanceManager>();
        financeManagerMock.Setup(fm => fm.Balance).Returns(100.0m);

        var transaction = new DebitTransaction(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, 50.0m);

        // Act
        transaction.Process(financeManagerMock.Object);

        // Assert
        financeManagerMock.Verify(fm => fm.DecreaseBalance(50.0m), Times.Once);
    }

    [Fact]
    public void Revert_ShouldIncreaseBalanceAndSetRevertedAt()
    {
        // Arrange
        var financeManagerMock = new Mock<IFinanceManager>();
        var transaction = new DebitTransaction(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, 50.0m);

        // Act
        transaction.Revert(financeManagerMock.Object);

        // Assert
        financeManagerMock.Verify(fm => fm.IncreaseBalance(50.0m), Times.Once);
        Assert.NotNull(transaction.RevertedAt);
    }
}
