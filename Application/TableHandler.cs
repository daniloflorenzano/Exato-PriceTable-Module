using Application.Abstractions;
using Domain;
using Domain.Abstractions;

namespace Application;

public class TableHandler
{
    private readonly IRepositoryFactory _repositoryFactory;

    public TableHandler(IRepositoryFactory repositoryFactory)
    {
        _repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
    }

    public async Task<Table> CreateTable(Table table)
    {
        var repository = _repositoryFactory.Create();
        var newTable = await repository.CreateTable(table);
        
        return newTable;
    }
    
}