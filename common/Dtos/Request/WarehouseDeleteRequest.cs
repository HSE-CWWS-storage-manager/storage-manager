using System.ComponentModel.DataAnnotations;

namespace common.Dtos.Request;

public record WarehouseDeleteRequest(
    [Required(ErrorMessage = "WarehouseId is required")]
    Guid WarehouseId  
);