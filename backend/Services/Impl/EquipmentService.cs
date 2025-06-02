using System.Net;
using AutoMapper;
using backend.Exceptions;
using backend.Models;
using common.Dtos;
using common.Dtos.Request;
using common.Dtos.Response;
using Microsoft.EntityFrameworkCore;

namespace backend.Services.Impl;

public class EquipmentService(IMapper mapper, StorageManagerDbContext dbContext) : IEquipmentService
{
    private const int PageSize = 10;
    
    public async Task<EquipmentDto> AddEquipment(AddEquipmentRequest request)
    {
        var newEquipment = mapper.Map<Equipment>(request);
        var entry = await dbContext.Equipments.AddAsync(newEquipment);
        await dbContext.SaveChangesAsync();
        return mapper.Map<EquipmentDto>(entry.Entity);
    }

    public async Task<EquipmentDto> UpdateEquipment(UpdateEquipmentRequest request)
    {
        var equipment = await dbContext.Equipments.FirstAsync(equipment => equipment.Id.Equals(request.EquipmentId));
        equipment.Model = request.Model ?? equipment.Model;
        equipment.SerialNumber = request.SerialNumber ?? equipment.SerialNumber;
        equipment.InventoryNumber = request.InventoryNumber ?? equipment.InventoryNumber;
        equipment.Name = request.Name ?? equipment.Name;
        var entry = dbContext.Equipments.Update(equipment);
        await dbContext.SaveChangesAsync();
        return mapper.Map<EquipmentDto>(entry.Entity);
    }

    public EquipmentFindResponse FindEquipment(EquipmentFindRequest request)
    {
        if (request.EquipmentId != null)
            return new EquipmentFindResponse(
                dbContext.Equipments.Where(x => x.Id.Equals(request.EquipmentId))
                    .Select(x => mapper.Map<EquipmentDto>(x))
                    .ToList()
            );

        if (request.Query == null)
            return new EquipmentFindResponse(
                dbContext.Equipments.Select(x => mapper.Map<EquipmentDto>(x))
                    .Skip(PageSize * (request.Page - 1))
                    .Take(PageSize)
                    .ToList()
            );
        
        var query = request.Query.ToLower();
            
        return new EquipmentFindResponse(
            dbContext.Equipments
                .Where(x => x.Name.ToLower().StartsWith(query))
                .Select(x => mapper.Map<EquipmentDto>(x))
                .Skip(PageSize * (request.Page - 1))
                .Take(PageSize)
                .ToList()
        );
    }

    public async Task DeleteEquipment(DeleteEquipmentRequest request)
    {
        try
        {
            var equipment = await dbContext.Equipments
                .FirstOrDefaultAsync(equipment => equipment.Id.Equals(request.EquipmentId));

            if (equipment != null)
            {
                dbContext.Equipments.Remove(equipment);
                await dbContext.SaveChangesAsync();
            }
        }
        catch (Exception)
        {
            throw new HttpResponseException((int)HttpStatusCode.InternalServerError, new HttpErrorMessageResponse("Unexpected error"));
        }
    }
}