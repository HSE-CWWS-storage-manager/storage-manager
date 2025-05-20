using System.ComponentModel.DataAnnotations;

namespace common.Dtos.Request;

public record StudentDeleteRequest(
    [Required(ErrorMessage = "StudentId is required")]
    Guid StudentId
);