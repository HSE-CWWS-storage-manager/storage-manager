using System.Data.Entity;
using System.Net;
using backend.Exceptions;
using backend.Models;
using common.Dtos;
using common.Dtos.Request;
using common.Dtos.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace backend.Services.Impl;

public class EquipmentOperationService(IWarehouseService warehouseService, StorageManagerDbContext dbContext) : IEquipmentOperationService
{
    private const int PageSize = 10;

    public async Task<EquipmentOperationDto> Transfer(IdentityUser initiator, EquipmentTransferRequest request)
    {
        var warehouseId = request.WarehouseId;
        var equipmentId = request.EquipmentId;
        var recipientId = request.RecipientId;
        
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
        
        var student = dbContext.Students
            .FirstOrDefault(x => x.Id.Equals(recipientId));
        
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
        
        await warehouseService.ExtractEquipmentFromStock(changeRequest);
        await warehouseService.AddEquipmentOnLoan(changeRequest);

        var transfer = new EquipmentTransfer
        {
            Equipment = equipment,
            Initiator = initiator,
            IssueDate = DateTime.UtcNow,
            Recipient = student,
            From = warehouse
        };

        var entry = await dbContext.EquipmentTransfers.AddAsync(transfer);

        await dbContext.SaveChangesAsync();

        return new EquipmentOperationDto(
            entry.Entity.Id, 
            entry.Entity.From.Id, 
            entry.Entity.Equipment.Id, 
            Guid.Parse(entry.Entity.Initiator.Id),
            entry.Entity.IssueDate,
            EquipmentOperationType.Transfer,
            recipientId,
            ReturnDate: entry.Entity.ReturnDate
        );
    }

    public async Task<EquipmentOperationDto> WriteOff(IdentityUser initiator, EquipmentWriteOffRequest request)
    {
        var warehouseId = request.WarehouseId;
        var equipmentId = request.EquipmentId;
        
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
            Date = DateTime.UtcNow,
            Initiator = initiator,
            Quantity = request.Quantity
        };
        
        var entry = await dbContext.EquipmentWriteOffs.AddAsync(writeOff);

        await dbContext.SaveChangesAsync();
        
        return new EquipmentOperationDto(
            entry.Entity.Id, 
            entry.Entity.From.Id, 
            entry.Entity.Equipment.Id, 
            Guid.Parse(entry.Entity.Initiator.Id),
            entry.Entity.Date,
            EquipmentOperationType.WriteOff,
            Quantity: request.Quantity
        );
    }

    public async Task<EquipmentOperationDto> Return(EquipmentReturnRequest request)
    {
        var transfer = EntityFrameworkQueryableExtensions.Include(
                EntityFrameworkQueryableExtensions.Include(EntityFrameworkQueryableExtensions.Include(
                    EntityFrameworkQueryableExtensions
                        .Include(dbContext.EquipmentTransfers, equipmentTransfer => equipmentTransfer.From),
                    equipmentTransfer => equipmentTransfer.Equipment), equipmentTransfer => equipmentTransfer.Recipient),
                equipmentTransfer => equipmentTransfer.Initiator
            )
            .FirstOrDefault(x => x.Id.Equals(request.OperationId));
        
        if (transfer == null)
            throw new HttpResponseException(
                (int)HttpStatusCode.NotFound,
                new HttpErrorMessageResponse($"Operation with id {request.OperationId} not found.")
            );
        
        var changeRequest = new ChangeEquipmentUnitCountRequest(
            transfer.Equipment.Id,
            transfer.From.Id,
            1
        );

        await warehouseService.ExtractEquipmentFromLoan(changeRequest);
        await warehouseService.AddEquipmentOnStock(changeRequest);
        
        transfer.ReturnDate = request.ReturnDate ?? DateTime.UtcNow;

        dbContext.Update(transfer);

        await dbContext.SaveChangesAsync();
        
        return new EquipmentOperationDto(
            transfer.Id, 
            transfer.From.Id, 
            transfer.Equipment.Id, 
            Guid.Parse(transfer.Initiator.Id),
            transfer.IssueDate,
            EquipmentOperationType.Transfer,
            transfer.Recipient.Id,
            ReturnDate: transfer.ReturnDate
        );
    }

    public EquipmentOperationsFindResponse Find(EquipmentOperationsFindRequest request)
    {
        var listTransfers = dbContext.EquipmentTransfers
            .Where(x => (request.StartDate == null || x.IssueDate >= request.StartDate) &&
                        (request.EndDate == null || x.IssueDate <= request.EndDate) &&
                        (request.EquipmentId == null || x.Equipment.Id.Equals(request.EquipmentId)) &&
                        (request.WarehouseId == null || x.From.Id.Equals(request.WarehouseId)) &&
                        (!request.WithoutReturnDate || x.ReturnDate == null))
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
                null,
                x.ReturnDate
            ))
            .ToList();

        var listWriteOffs = dbContext.EquipmentWriteOffs
            .Where(x => (request.StartDate == null || x.Date >= request.StartDate) &&
                        (request.EndDate == null || x.Date <= request.EndDate) &&
                        (request.EquipmentId == null || x.Equipment.Id.Equals(request.EquipmentId)) &&
                        (request.WarehouseId == null || x.From.Id.Equals(request.WarehouseId)))
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
                x.Quantity,
                null
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