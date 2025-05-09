using System.Data.Entity;
using System.Net;
using backend.Exceptions;
using backend.Models;
using common.Dtos.Request;
using common.Dtos.Response;

namespace backend.Services.Impl;

public class WarehouseService(StorageManagerDbContext dbContext) : IWarehouseService
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
        var equipment = await dbContext.Equipments
            .FirstOrDefaultAsync(x => x.Id.Equals(equipmentId));

        if (equipment == null)
            throw new HttpResponseException(
                (int)HttpStatusCode.NotFound,
                new HttpErrorMessageResponse($"Equipment with id {equipmentId} not found.", HttpStatusCode.NotFound)
            );
        
        var warehouse = await dbContext.Warehouses
            .FirstOrDefaultAsync(x => x.Id.Equals(warehouseId));
        
        if (warehouse == null)
            throw new HttpResponseException(
                (int)HttpStatusCode.NotFound,
                new HttpErrorMessageResponse($"Warehouse with id {warehouseId} not found.", HttpStatusCode.NotFound)
            );
        
        var remain = await dbContext.EquipmentRemains
                         .FirstOrDefaultAsync(x => x.Equipment.Id.Equals(equipmentId) && x.Warehouse.Id.Equals(warehouseId)) ?? 
                     CreateEmptyRemain(equipment, warehouse);

        if (getter(remain) + count < 0)
        {
            throw new HttpResponseException(
                (int) HttpStatusCode.BadRequest,
                new HttpErrorMessageResponse("Remains count cannot be less than zero.", HttpStatusCode.BadRequest)
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
}