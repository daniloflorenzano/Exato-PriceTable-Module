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

    public async Task<List<Item>> ListAll()
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
    
    public async Task<List<Item>> ListItemsInDateRange(DateTime startDate, DateTime endDate)
    {
        try
        {
            if (startDate > DateTime.Now)
                throw new CannotUseFutureDateException(startDate);

            var repository = _repositoryFactory.Create(_schema);
            var result = await repository.ListItemsInDateRange(_tableExternalId, startDate, endDate);

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

    public async Task<Item> GetByExternalId(Guid itemExternalId)
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

    public async Task Create(Item item)
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

    public async Task Update(Guid itemExtenalId, Item item)
    {
        try
        {
            var repository = _repositoryFactory.Create(_schema);
            var table = await repository.GetTableByExternalId(_tableExternalId);
            var tableType = table.Type;

            var errMsg =
                "You cannot use this price configuration in a Fixed Price table. PriceSequence and AmountLimitsToApply must be null";
            if (tableType is DiscountType.FixedPrice && item.Price.PriceSequence is not null)
                throw new CannotUpdateItemException(errMsg);

            await repository.UpdateItem(itemExtenalId, item, _tableExternalId);
        }
        catch (TableNotFoundException e)
        {
            _logger.Error(e.Message);
            throw;
        }
        catch (CannotUpdateItemException e)
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
    
    public async Task Delete(Guid itemExternalId)
    {
        try
        {
            var repository = _repositoryFactory.Create(_schema);
            await repository.DeleteItem(itemExternalId, _tableExternalId);
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