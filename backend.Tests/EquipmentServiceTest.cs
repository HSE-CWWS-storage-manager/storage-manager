using System.Security.Cryptography;
using backend.Services;
using backend.Services.Impl;
using common.Dtos.Request;

namespace backend.Tests;

public class EquipmentServiceTest : IDisposable, IAsyncDisposable
{
    private readonly StorageManagerDbContext _context;
    private readonly IEquipmentService _service;

    public EquipmentServiceTest()
    {
        var context = TestUtils.CreateContext();
        _context = context;
        _service = new EquipmentService(TestUtils.CreateAutoMapper(), context);
    }

    [Fact]
    public async Task AddEquipment_ValidData_ReturnsNewEquipmentDto()
    {
        var request = new AddEquipmentRequest("A", "B", "C", "D", "1", DateTime.UtcNow);
        var actual = await _service.AddEquipment(request);
        
        Assert.Equal("A", actual.Model);
        Assert.Equal("B", actual.Name);
        Assert.Equal("C", actual.SerialNumber);
        Assert.Equal("D", actual.InventoryNumber);
    }
    
    [Fact]
    public async Task UpdateEquipment_ValidData_ReturnsUpdatedEquipmentDto()
    {
        var newEquipment = await _service.AddEquipment(new AddEquipmentRequest("A", "B", "C", "D", "1", DateTime.UtcNow));
        
        var request = new UpdateEquipmentRequest(newEquipment.Id, "D", "C", "B", "A", "1", DateTime.UtcNow);
        var actual = await _service.UpdateEquipment(request);
        
        Assert.Equal("D", actual.Model);
        Assert.Equal("C", actual.Name);
        Assert.Equal("B", actual.SerialNumber);
        Assert.Equal("A", actual.InventoryNumber);
    }
    
    [Fact]
    public async Task FindEquipment_ValidData_ReturnsAllNeededData()
    {
        await _service.AddEquipment(new AddEquipmentRequest("S", "O", "R", "P", "1", DateTime.UtcNow));

        var request = new EquipmentFindRequest(Query: "O");
        var actual = _service.FindEquipment(request);
        
        Assert.Single(actual.Equipments);

        var first = actual.Equipments.First();
        
        Assert.Equal("O", first.Name);
        Assert.Equal("S", first.Model);
        Assert.Equal("R", first.SerialNumber);
        Assert.Equal("P", first.InventoryNumber);
    }
    
    [Fact]
    public async Task DeleteEquipment_ValidData_ReturnsAllNeededData()
    {
        var newEquipment = await _service.AddEquipment(new AddEquipmentRequest("S", "O", "R", "P", "1", DateTime.UtcNow));

        var request = new DeleteEquipmentRequest(newEquipment.Id);
        await _service.DeleteEquipment(request);
        
        Assert.Empty(_service.FindEquipment(new EquipmentFindRequest(newEquipment.Id)).Equipments);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }
}