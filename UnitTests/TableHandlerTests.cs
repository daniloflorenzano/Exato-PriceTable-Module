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
        A.CallTo(() => repository.CreateTable(table)).MustHaveHappened(1, Times.Exactly);
    }

    [Test]
    public async Task CreateTable_Returns_Exisiting_Table_If_Name_And_Type_Are_Equal()
    {
        // Arrange
        var repository = A.Fake<IRepository>();
        A.CallTo(() => _repositoryFactory.Create()).Returns(repository);
        var tableHandler = new TableHandler(_repositoryFactory);
        var table = new Table("Tabela", TableType.CumulativePrice);
        A.CallTo(() => repository.ListTables()).Returns(new Table[] { table });
        
        // Act
        var table1 = await tableHandler.CreateTable(table);
        var table2 = await tableHandler.CreateTable(table);
        
        // Assert
        table1.ExternalId.Should().Be(table2.ExternalId);
    }

    [Test]
    
    public async Task CreateTable_Throws_Error_When_Trying_Create_With_Existing_Name_But_Different_Type()
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
}