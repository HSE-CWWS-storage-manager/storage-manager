using System.ComponentModel.DataAnnotations;

namespace common.Dtos.Request;

public record StudentFindRequest(
    [Required(ErrorMessage = "Name is required")]
    string Name, 
    string? Group
);