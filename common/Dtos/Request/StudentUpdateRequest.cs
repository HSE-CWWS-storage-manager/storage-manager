using System.ComponentModel.DataAnnotations;

namespace common.Dtos.Request;

public record StudentUpdateRequest(
    [Required(ErrorMessage = "StudentId is required")]
    Guid StudentId,
    string? Name,
    string? Group
);