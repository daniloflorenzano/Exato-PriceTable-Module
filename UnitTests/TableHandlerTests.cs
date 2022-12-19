using Application;
using Domain;
using Domain.Abstractions;
using Domain.Enums;
using FakeItEasy;
using FluentAssertions;

namespace UnitTests;

public class TableHandlerTests
{
    private readonly IRepositoryFactory _repositoryFactory;

    public TableHandlerTests()
    {
        _repositoryFactory = A.Fake<IRepositoryFactory>();
    }
    
    [Test]
    public async Task ListTables_Is_Calling_Repository_ListTables()
    {
        // Arrange
        var table = A.Fake<Table>();
        var repository = A.Fake<IRepository>();
        A.CallTo(() => _repositoryFactory.Create()).Returns(repository);
        var tableHandler = new TableHandler(_repositoryFactory);

        // Act
        await tableHandler.ListTables();

        // Assert
        A.CallTo(() => repository.ListTables())
            .MustHaveHappened(1, Times.Exactly);
    }
    
    [Test]
    public async Task GetTableByExternalId_Is_Calling_Repository_GetTableByExternalId()
    {
        // Arrange
        var table = A.Fake<Table>();
        var repository = A.Fake<IRepository>();
        A.CallTo(() => _repositoryFactory.Create()).Returns(repository);
        var tableHandler = new TableHandler(_repositoryFactory);

        // Act
        await tableHandler.GetTableByExternalId(table.ExternalId);

        // Assert
        A.CallTo(() => repository.GetTableByExternalId(table.ExternalId))
            .MustHaveHappened(1, Times.Exactly);
    }

    [Test]
    public async Task GetTableByExternalId_Throws_Error_When_Table_Does_Not_Exists()
    {
        // Arrange
        var externalId = Guid.NewGuid();
        var repository = A.Fake<IRepository>();
        A.CallTo(() => _repositoryFactory.Create()).Returns(repository);
        A.CallTo(() => repository.GetTableByExternalId(externalId)).Throws<Exception>();
        var tableHandler = new TableHandler(_repositoryFactory);
        
        // Act
        Func<Task> func = async () => await tableHandler.GetTableByExternalId(externalId);

        // Assert  
        await func.Should()
            .ThrowAsync<Exception>();
    }
    
    [Test]
    public async Task CreateTable_Is_Calling_Repository_CreateTable()
    {
        // Arrange
        var table = A.Fake<Table>();
        var repository = A.Fake<IRepository>();
        A.CallTo(() => _repositoryFactory.Create()).Returns(repository);
        var tableHandler = new TableHandler(_repositoryFactory);

        // Act
        await tableHandler.CreateTable(table);

        // Assert
        A.CallTo(() => repository.CreateTable(table))
            .MustHaveHappened(1, Times.Exactly);
    }

    [Test]
    public async Task CreateTable_Throws_Exception_When_Trying_Create_Identical_Table()
    {
        // Arrange
        var repository = A.Fake<IRepository>();
        A.CallTo(() => _repositoryFactory.Create()).Returns(repository);
        var tableHandler = new TableHandler(_repositoryFactory);
        var table = new Table("Tabela", TableType.FixedPrice);
        A.CallTo(() => repository.ListTables()).Returns(new Table[] { table });
        
        // Act
        var tableWithSameNameAndType = new Table("Tabela", TableType.FixedPrice);
        Func<Task> func = async () => await tableHandler.CreateTable(tableWithSameNameAndType);

        // Assert  
        await func.Should()
            .ThrowAsync<Exception>()
            .WithMessage($"Already exists a table named {tableWithSameNameAndType.Name} with this same type");
    }

    [Test]
    
    public async Task CreateTable_Throws_Exception_When_Trying_Create_With_Existing_Name_But_Different_Type()
    {
        // Arrange
        var repository = A.Fake<IRepository>();
        A.CallTo(() => _repositoryFactory.Create()).Returns(repository);
        var tableHandler = new TableHandler(_repositoryFactory);
        var table = new Table("Tabela", TableType.CumulativePrice);
        A.CallTo(() => repository.ListTables()).Returns(new Table[] { table });
        
        // Act
        var tableWithSameNameButDifferentType = new Table("Tabela", TableType.FixedPrice);
        Func<Task> func = async () => await tableHandler.CreateTable(tableWithSameNameButDifferentType);

        // Assert  
        await func.Should()
            .ThrowAsync<Exception>()
            .WithMessage($"Already exists a table named {tableWithSameNameButDifferentType.Name}, but with a different type");
    }
    
    [Test]
    public async Task UpdateTable_Is_Calling_Repository_UpdateTable()
    {
        // Arrange
        var table = A.Fake<Table>();
        var repository = A.Fake<IRepository>();
        A.CallTo(() => _repositoryFactory.Create()).Returns(repository);
        var tableHandler = new TableHandler(_repositoryFactory);

        // Act
        await tableHandler.UpdateTable(table.ExternalId, table);

        // Assert
        A.CallTo(() => repository.UpdateTable(table.ExternalId, table))
            .MustHaveHappened(1, Times.Exactly);
    }

    [Test] public async Task DeleteTable_Is_Calling_Repository_DeleteTable()
    {
        // Arrange
        var table = A.Fake<Table>();
        var repository = A.Fake<IRepository>();
        A.CallTo(() => _repositoryFactory.Create()).Returns(repository);
        var tableHandler = new TableHandler(_repositoryFactory);

        // Act
        await tableHandler.DeleteTable(table.ExternalId);

        // Assert
        A.CallTo(() => repository.DeleteTable(table.ExternalId))
            .MustHaveHappened(1, Times.Exactly);
    }
}