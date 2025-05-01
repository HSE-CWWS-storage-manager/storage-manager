using System.ComponentModel.DataAnnotations;

namespace common.Dtos.Request;

public record EquipmentFindRequest(Guid? EquipmentId, string? Query, [Range(1, Int32.MaxValue)] int Page = 1);