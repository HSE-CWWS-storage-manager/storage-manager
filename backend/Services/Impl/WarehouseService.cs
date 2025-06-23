using System.Data.Entity;
using System.Net;
using AutoMapper;
using backend.Exceptions;
using backend.Models;
using common.Dtos;
using common.Dtos.Request;
using common.Dtos.Response;

namespace backend.Services.Impl;

public class WarehouseService(IMapper mapper, StorageManagerDbContext dbContext) : IWarehouseService
{
    
    private const int PageSize = 10;
    
    public EquipmentRemainsResponse GetRemains(EquipmentRemainsRequest request)
    {
        if (request.WarehouseId != null)
        {
            return new EquipmentRemainsResponse(
                dbContext.EquipmentRemains
                    .Where(x => x.Equipment.Id.Equals(request.EquipmentId) && x.Warehouse.Id.Equals(request.WarehouseId))
                    .Select(x => new EquipmentRemainData(x.Warehouse.Id, x.OnStock, x.OnLoan))
                    .Skip((request.Page - 1) * PageSize)
                    .Take(PageSize)
                    .ToList()
                );
        }
        
        return new EquipmentRemainsResponse(
            dbContext.EquipmentRemains
                .Where(x => x.Equipment.Id.Equals(request.EquipmentId))
                .Select(x => new EquipmentRemainData(x.Warehouse.Id, x.OnStock, x.OnLoan))
                .Skip((request.Page - 1) * PageSize)
                .Take(PageSize)
                .ToList()
        );
    }

    private EquipmentRemain CreateEmptyRemain(Equipment equipment, Warehouse warehouse)
    {
        var remain = new EquipmentRemain();
        
        remain.OnStock = 0;
        remain.OnLoan = 0;
        remain.Equipment = equipment;
        remain.Warehouse = warehouse;

        return remain;
    }

    private async Task<EquipmentRemainsResponse> UpdateEquipmentUnitCount(
        Guid equipmentId, Guid warehouseId, int count, Action<EquipmentRemain, int> setter, Func<EquipmentRemain, int> getter)
    {
        var equipment = dbContext.Equipments
            .FirstOrDefault(x => x.Id.Equals(equipmentId));

        if (equipment == null)
            throw new HttpResponseException(
                (int)HttpStatusCode.NotFound,
                new HttpErrorMessageResponse($"Equipment with id {equipmentId} not found.")
            );
        
        var warehouse = dbContext.Warehouses
            .FirstOrDefault(x => x.Id.Equals(warehouseId));
        
        if (warehouse == null)
            throw new HttpResponseException(
                (int)HttpStatusCode.NotFound,
                new HttpErrorMessageResponse($"Warehouse with id {warehouseId} not found.")
            );
        
        var remain = dbContext.EquipmentRemains
                         .FirstOrDefault(x => x.Equipment.Id.Equals(equipmentId) && x.Warehouse.Id.Equals(warehouseId)) ?? 
                     CreateEmptyRemain(equipment, warehouse);

        if (getter(remain) + count < 0)
        {
            throw new HttpResponseException(
                (int) HttpStatusCode.BadRequest,
                new HttpErrorMessageResponse("Remains count cannot be less than zero.")
            );
        }

        setter(remain, count);
        
        var entry = dbContext.EquipmentRemains.Update(remain);
        await dbContext.SaveChangesAsync();

        return new EquipmentRemainsResponse(
            [new(warehouse.Id, entry.Entity.OnStock, entry.Entity.OnLoan)]
        );
    }

    public async Task<EquipmentRemainsResponse> AddEquipmentOnStock(ChangeEquipmentUnitCountRequest request)
    {
        return await UpdateEquipmentUnitCount(
            request.EquipmentId,
            request.WarehouseId,
            request.Count,
            (x, y) => x.OnStock += y,
            x => x.OnStock
        );
    }

    public async Task<EquipmentRemainsResponse> ExtractEquipmentFromStock(ChangeEquipmentUnitCountRequest request)
    {
        return await UpdateEquipmentUnitCount(
            request.EquipmentId,
            request.WarehouseId,
            -request.Count,
            (x, y) => x.OnStock += y,
            x => x.OnStock
        );
    }

    public async Task<EquipmentRemainsResponse> AddEquipmentOnLoan(ChangeEquipmentUnitCountRequest request)
    {
        return await UpdateEquipmentUnitCount(
            request.EquipmentId,
            request.WarehouseId,
            request.Count,
            (x, y) => x.OnLoan += y,
            x => x.OnLoan
        );
    }

    public async Task<EquipmentRemainsResponse> ExtractEquipmentFromLoan(ChangeEquipmentUnitCountRequest request)
    {
        return await UpdateEquipmentUnitCount(
            request.EquipmentId,
            request.WarehouseId,
            -request.Count,
            (x, y) => x.OnLoan += y,
            x => x.OnLoan
        );
    }

    public async Task<WarehouseDto> CreateWarehouse(WarehouseCreateRequest request)
    {
        var warehouse = new Warehouse();
        warehouse.Name = request.Name;
        var entry = await dbContext.Warehouses.AddAsync(warehouse);
        await dbContext.SaveChangesAsync();
        return mapper.Map<WarehouseDto>(entry.Entity);
    }

    public async Task<WarehouseDto> UpdateWarehouse(WarehouseUpdateRequest request)
    {
        var warehouse = dbContext.Warehouses.FirstOrDefault(x => x.Id.Equals(request.WarehouseId));

        if (warehouse == null)
            throw new HttpResponseException(
                (int) HttpStatusCode.NotFound,
                new HttpErrorMessageResponse($"Warehouse with id {request.WarehouseId} not found!")
            );
        
        warehouse.Name = request.Name;
        var entry = dbContext.Warehouses.Update(warehouse);
        await dbContext.SaveChangesAsync();
        
        return mapper.Map<WarehouseDto>(entry.Entity);
    }

    public async Task<WarehouseDto> DeleteWarehouse(WarehouseDeleteRequest request)
    {
        var warehouse = dbContext.Warehouses.FirstOrDefault(x => x.Id.Equals(request.WarehouseId));

        if (warehouse != null)
        {
            dbContext.Warehouses.Remove(warehouse);
            await dbContext.SaveChangesAsync();
            return mapper.Map<WarehouseDto>(warehouse);
        }

        var empty = new Warehouse();
        empty.Id = Guid.Empty;
        empty.Name = "not found";
        
        return mapper.Map<WarehouseDto>(empty);
    }

    public List<WarehouseDto> ListWarehouses(WarehouseListRequest request)
    {
        return dbContext.Warehouses
            .Skip((request.Page - 1) * PageSize)
            .Take(PageSize)
            .Select(x => mapper.Map<WarehouseDto>(x))
            .ToList();
    }
}