namespace common.Dtos;

public record UserDto(Guid Id, string Email, IList<string> Roles);