namespace common.Models;

public class User
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string Group { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
}