using System.ComponentModel.DataAnnotations;

namespace common.Dtos.Request;

public record WarehouseCreateRequest(
    [Required(ErrorMessage = "Name is required")]
    string Name
);