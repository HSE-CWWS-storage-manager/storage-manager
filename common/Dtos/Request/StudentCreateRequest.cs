using System.ComponentModel.DataAnnotations;

namespace common.Dtos.Request;

public record StudentCreateRequest(
    [Required(ErrorMessage = "Name is required")]
    string Name, 
    [Required(ErrorMessage = "Group is required")]
    string Group
);