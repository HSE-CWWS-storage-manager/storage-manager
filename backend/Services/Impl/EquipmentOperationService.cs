using System.Data.Entity;
using System.Net;
using backend.Exceptions;
using backend.Models;
using common.Dtos;
using common.Dtos.Request;
using common.Dtos.Response;
using Microsoft.AspNetCore.Identity;

namespace backend.Services.Impl;

public class EquipmentOperationService(IWarehouseService warehouseService, StorageManagerDbContext dbContext) : IEquipmentOperationService
{
    private const int PageSize = 10;

    public async Task<EquipmentOperationDto> Transfer(IdentityUser initiator, EquipmentTransferRequest request)
    {
        var warehouseId = request.WarehouseId;
        var equipmentId = request.EquipmentId;
        var recipientId = request.RecipientId;
        
        var equipment = await dbContext.Equipments
            .FirstOrDefaultAsync(x => x.Id.Equals(equipmentId));

        if (equipment == null)
            throw new HttpResponseException(
                (int)HttpStatusCode.NotFound,
                new HttpErrorMessageResponse($"Equipment with id {equipmentId} not found.")
            );
        
        var warehouse = await dbContext.Warehouses
            .FirstOrDefaultAsync(x => x.Id.Equals(warehouseId));
        
        if (warehouse == null)
            throw new HttpResponseException(
                (int)HttpStatusCode.NotFound,
                new HttpErrorMessageResponse($"Warehouse with id {warehouseId} not found.")
            );
        
        var student = await dbContext.Students
            .FirstOrDefaultAsync(x => x.Id.Equals(recipientId));
        
        if (student == null)
            throw new HttpResponseException(
                (int)HttpStatusCode.NotFound,
                new HttpErrorMessageResponse($"Student with id {recipientId} not found.")
            );

        var changeRequest = new ChangeEquipmentUnitCountRequest(
                equipmentId,
                warehouseId,
                1
        );

        await warehouseService.AddEquipmentOnLoan(changeRequest);
        await warehouseService.ExtractEquipmentFromStock(changeRequest);

        var transfer = new EquipmentTransfer
        {
            Equipment = equipment,
            Initiator = initiator,
            IssueDate = DateTime.Now,
            Recipient = student,
            From = warehouse
        };

        var entry = await dbContext.EquipmentTransfers.AddAsync(transfer);

        return new EquipmentOperationDto(
            entry.Entity.Id, 
            entry.Entity.From.Id, 
            entry.Entity.Equipment.Id, 
            Guid.Parse(entry.Entity.Initiator.Id),
            entry.Entity.IssueDate,
            EquipmentOperationType.Transfer,
            recipientId
        );
    }

    public async Task<EquipmentOperationDto> WriteOff(IdentityUser initiator, EquipmentWriteOffRequest request)
    {
        var warehouseId = request.WarehouseId;
        var equipmentId = request.EquipmentId;
        
        var equipment = await dbContext.Equipments
            .FirstOrDefaultAsync(x => x.Id.Equals(equipmentId));

        if (equipment == null)
            throw new HttpResponseException(
                (int)HttpStatusCode.NotFound,
                new HttpErrorMessageResponse($"Equipment with id {equipmentId} not found.")
            );
        
        var warehouse = await dbContext.Warehouses
            .FirstOrDefaultAsync(x => x.Id.Equals(warehouseId));
        
        if (warehouse == null)
            throw new HttpResponseException(
                (int)HttpStatusCode.NotFound,
                new HttpErrorMessageResponse($"Warehouse with id {warehouseId} not found.")
            );
        
        var changeRequest = new ChangeEquipmentUnitCountRequest(
            equipmentId,
            warehouseId,
            request.Quantity
        );
        
        await warehouseService.ExtractEquipmentFromStock(changeRequest);

        var writeOff = new EquipmentWriteOff
        {
            Equipment = equipment,
            From = warehouse,
            Date = DateTime.Now,
            Initiator = initiator,
            Quantity = request.Quantity
        };
        
        var entry = await dbContext.EquipmentWriteOffs.AddAsync(writeOff);
        
        return new EquipmentOperationDto(
            entry.Entity.Id, 
            entry.Entity.From.Id, 
            entry.Entity.Equipment.Id, 
            Guid.Parse(entry.Entity.Initiator.Id),
            entry.Entity.Date,
            EquipmentOperationType.Transfer,
            Quantity: request.Quantity
        );
    }

    public EquipmentOperationsFindResponse Find(EquipmentOperationsFindRequest request)
    {
        var listTransfers = dbContext.EquipmentTransfers
            .Where(x => (request.StartDate == null || x.IssueDate >= request.StartDate) &&
                        (request.EndDate == null || x.IssueDate <= request.EndDate))
            .OrderByDescending(x => x.IssueDate)
            .Skip((request.Page - 1) * PageSize)
            .Take(PageSize)
            .Select(x => new EquipmentOperationDto(
                x.Id,
                x.From.Id,
                x.Equipment.Id,
                Guid.Parse(x.Initiator.Id),
                x.IssueDate,
                EquipmentOperationType.Transfer,
                x.Recipient.Id,
                null
            ))
            .ToList();

        var listWriteOffs = dbContext.EquipmentWriteOffs
            .Where(x => (request.StartDate == null || x.Date >= request.StartDate) &&
                        (request.EndDate == null || x.Date <= request.EndDate))
            .OrderByDescending(x => x.Date)
            .Skip((request.Page - 1) * PageSize)
            .Take(PageSize)
            .Select(x => new EquipmentOperationDto(
                x.Id,
                x.From.Id,
                x.Equipment.Id,
                Guid.Parse(x.Initiator.Id),
                x.Date,
                EquipmentOperationType.WriteOff,
                null,
                x.Quantity
            ))
            .ToList();

        return request.Type switch
        {
            EquipmentOperationType.Transfer => new EquipmentOperationsFindResponse(listTransfers),
            EquipmentOperationType.WriteOff => new EquipmentOperationsFindResponse(listWriteOffs),
            _ => new EquipmentOperationsFindResponse(listWriteOffs.Union(listTransfers).ToList())
        };
    }
}