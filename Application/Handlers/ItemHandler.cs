﻿using Domain.Abstractions;
using Domain.Entities;

namespace Application.Handlers;

public class ItemHandler
{
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly Guid _tableExternalId;

    public ItemHandler(IRepositoryFactory repositoryFactory, Guid tableExternalId)
    {
        _repositoryFactory = repositoryFactory;
        _tableExternalId = tableExternalId;
    }

    public async Task<Item[]> ListAllItemsInTable()
    {
        try
        {
            var repository = _repositoryFactory.Create();
            var result = await repository.ListItems(_tableExternalId);

            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Item> GetItemByExternalIdInTable(Guid itemExternalId)
    {
        try
        {
            var repository = _repositoryFactory.Create();
            var result = await repository.GetItemByExternalId(itemExternalId, _tableExternalId);

            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task CreateItemInTable(Item item)
    {
        try
        {
            var repository = _repositoryFactory.Create();
            await repository.CreateItem(item, _tableExternalId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task UpdateItemInTable(Guid itemExtenalId, Item item)
    {
        try
        {
            var repository = _repositoryFactory.Create();

            await repository.UpdateItem(itemExtenalId, item, _tableExternalId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<Item>> ListItemsInTableInDateRange(DateTime initialDate, DateTime limitDate)
    {
        throw new NotImplementedException();
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