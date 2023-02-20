﻿using Domain.Entities;

namespace Domain.Abstractions;

public interface IRepository
{
    public Task CreateSchema();
    public Task CreateTablesTable();
    public Task CreateTable(Table table);
    public Task<List<Table?>> ListTables();
    public Task<Table?> GetTableByExternalId(Guid externalId);
    public Task UpdateTable(Guid externalId, Table table);
    public Task DeleteTable(Guid externalId);

    public Task CreateItem(Item item, Guid tableExternalId);
    public Task<List<Item>> ListItems(Guid tableExternalId);
    public Task<Item> GetItemByExternalId(Guid itemExternalId, Guid tableExternalId);
    public Task UpdateItem(Guid itemExternalId, Item item, Guid tableExternalId);
    public Task DeleteItem(Guid itemExternalId, Guid tableExternalId);
}