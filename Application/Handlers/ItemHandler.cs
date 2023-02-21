using Application.Abstractions;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Exceptions;

namespace Application.Handlers;

public class ItemHandler
{
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly ILogger _logger;
    private readonly Guid _tableExternalId;
    private string _schema;

    public ItemHandler(IRepositoryFactory repositoryFactory, ILogger logger, Guid tableExternalId, string schema)
    {
        _repositoryFactory = repositoryFactory;
        _logger = logger;
        _tableExternalId = tableExternalId;
        _schema = schema;
    }

    public async Task<List<Item>> ListAllItemsInTable()
    {
        try
        {
            var repository = _repositoryFactory.Create(_schema);
            var result = await repository.ListItems(_tableExternalId);

            return result;
        }
        catch (TableNotFoundException e)
        {
            _logger.Error(e.Message);
            throw;
        }
        catch (Exception e)
        {
            _logger.Error("Unknown errror: " + e.Message);
            throw;
        }
    }
    
    public async Task<List<Item>> ListItemsInTableSinceDate(DateTime date)
    {
        try
        {
            if (date > DateTime.Now)
                throw new CannotUseFutureDateException(date);

            var repository = _repositoryFactory.Create(_schema);
            var result = await repository.ListItemsSinceDate(_tableExternalId, date);

            return result;
        }
        catch (CannotUseFutureDateException e)
        {
            _logger.Error(e.Message);
            throw;
        }
        catch (TableNotFoundException e)
        {
            _logger.Error(e.Message);
            throw;
        }
        catch (Exception e)
        {
            _logger.Error("Unknown errror: " + e.Message);
            throw;
        }
    }

    public async Task<Item> GetItemByExternalIdInTable(Guid itemExternalId)
    {
        try
        {
            var repository = _repositoryFactory.Create(_schema);
            var result = await repository.GetItemByExternalId(_tableExternalId, itemExternalId);

            return result;
        }
        catch (TableNotFoundException e)
        {
            _logger.Error(e.Message);
            throw;
        }
        catch (ItemNotFoundException e)
        {
            _logger.Error(e.Message);
            throw;
        }
        catch (Exception e)
        {
            _logger.Error("Unknown errror: " + e.Message);
            throw;
        }
    }

    public async Task CreateItemInTable(Item item)
    {
        try
        {
            var repository = _repositoryFactory.Create(_schema);
            var table = await repository.GetTableByExternalId(_tableExternalId);

            var tableType = table.Type;

            if (item.Price.PriceSequence is not null && tableType == DiscountType.FixedPrice)
                throw new CannotInserItemException("You cannot insert this item in a Fixed Price table.");

            await repository.CreateItem(item, _tableExternalId);
        }
        catch (TableNotFoundException e)
        {
            _logger.Error(e.Message);
            throw;
        }
        catch (CannotInserItemException e)
        {
            _logger.Error(e.Message);
            throw;
        }
        catch (Exception e)
        {
            _logger.Error("Unknown errror: " + e.Message);
            throw;
        }
    }

    public async Task UpdateItemInTable(Guid itemExtenalId, Item item)
    {
        try
        {
            var repository = _repositoryFactory.Create(_schema);

            await repository.UpdateItem(itemExtenalId, item, _tableExternalId);
        }
        catch (Exception e)
        {
            _logger.Error("Unknown errror: " + e.Message);
            throw;
        }
    }

    public List<List<Item>> SegregateItems(List<Item> items)
    {
        var descriptions = items.Select(i => i.Description).Distinct().ToList();
        var list = new List<List<Item>>();

        for (int i = 0; i < descriptions.Count; i++)
        {
            var groupOfItems = items.Where(item => item.Description == descriptions[i]).ToList();
            list.Add(groupOfItems);
        }

        return list;
    }
}