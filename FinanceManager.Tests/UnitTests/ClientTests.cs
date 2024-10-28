using Xunit;
using FinanceManager.BL.Models;

namespace FinanceManager.Tests.UnitTests;

public class ClientTests
{
    [Fact]
    public void Constructor_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var initialBalance = 100.0m;

        // Act
        var client = new Client(clientId, initialBalance);

        // Assert
        Assert.Equal(clientId, client.Id);
        Assert.Equal(initialBalance, client.Balance);
    }

    [Fact]
    public void IncreaseBalance_ShouldIncreaseClientBalance()
    {
        // Arrange
        var client = new Client(Guid.NewGuid(), 50.0m);

        // Act
        client.IncreaseBalance(25.0m);

        // Assert
        Assert.Equal(75.0m, client.Balance);
    }

    [Fact]
    public void IncreaseBalance_ShouldThrowArgumentException_WhenAmountIsNegative()
    {
        // Arrange
        var client = new Client(Guid.NewGuid(), 50.0m);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => client.IncreaseBalance(-10.0m));
    }

    [Fact]
    public void DecreaseBalance_ShouldDecreaseClientBalance()
    {
        // Arrange
        var client = new Client(Guid.NewGuid(), 50.0m);

        // Act
        client.DecreaseBalance(20.0m);

        // Assert
        Assert.Equal(30.0m, client.Balance);
    }

    [Fact]
    public void DecreaseBalance_ShouldThrowInvalidOperationException_WhenInsufficientBalance()
    {
        // Arrange
        var client = new Client(Guid.NewGuid(), 20.0m);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => client.DecreaseBalance(50.0m));
    }

    [Fact]
    public void DecreaseBalance_ShouldThrowArgumentException_WhenAmountIsNegative()
    {
        // Arrange
        var client = new Client(Guid.NewGuid(), 50.0m);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => client.DecreaseBalance(-10.0m));
    }
}
