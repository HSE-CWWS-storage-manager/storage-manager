using System.ComponentModel.DataAnnotations;

namespace common.Dtos.Request;

public record WarehouseUpdateRequest(
    [Required(ErrorMessage = "WarehouseId is required")]
    Guid WarehouseId, 
    [Required(ErrorMessage = "Name is required")]
    string Name
);