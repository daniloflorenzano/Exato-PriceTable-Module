using Application;
using Domain;
using Domain.Abstractions;
using Domain.Enums;
using FakeItEasy;
using FluentAssertions;
using Infraestructure.Repositories;

namespace UnitTests;

public class TableTests
{
    private readonly IRepositoryFactory _repositoryFactory;

    public TableTests()
    {
        _repositoryFactory = A.Fake<IRepositoryFactory>();
    }

    [Test]
    public async Task CreateTable_Success()
    {
        // Arrange
        var table = new Table("Exemplo", null, null, TableType.FixedPrice);
        var tableHandler = new TableHandler(_repositoryFactory);

        // Act
        var result = await tableHandler.CreateTable(table);
        
        // Assert
        result.Should().NotBeNull();
    }
}