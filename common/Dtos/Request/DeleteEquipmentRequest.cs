using System.ComponentModel.DataAnnotations;

namespace common.Dtos.Request;

public record DeleteEquipmentRequest(
    [Required(ErrorMessage = "EquipmentId is required")]
    Guid EquipmentId
);