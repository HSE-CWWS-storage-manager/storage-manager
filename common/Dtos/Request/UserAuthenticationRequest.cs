using System.ComponentModel.DataAnnotations;

namespace backend.Dtos.Request;

public class UserAuthenticationRequest
{
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}