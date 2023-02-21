using Application.Abstractions;
using Application.Handlers;
using Domain.Abstractions;
using Domain.Entities;
using FakeItEasy;
using FluentAssertions;

namespace UnitTests;

public class ItemHandlerTests
{
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly Guid _tableExternalGuid;
    private readonly ILogger _logger;

    public ItemHandlerTests()
    {
        _repositoryFactory = A.Fake<IRepositoryFactory>();
        _logger = A.Fake<ILogger>();
        _tableExternalGuid = new Guid();
    }

    [Test]
    public void SegregateItems_Should_Return_Three_Lists()
    {
        // Arrange
        ItemHandler itemHandler = new(_repositoryFactory, _logger, _tableExternalGuid, "");
        var listOfItems = new List<Item>
        {
            new Item("A", 10.0m),
            new Item("A", 10.0m),
            new Item("B", 10.0m),
            new Item("C", 10.0m),
            new Item("A", 10.0m),
            new Item("B", 10.0m),
            new Item("A", 10.0m),
            new Item("C", 10.0m),
        };
        
        // Act
        var groupsOfItems = itemHandler.SegregateItems(listOfItems);

        // Assert
        groupsOfItems.Count.Should().Be(3);
    }
}