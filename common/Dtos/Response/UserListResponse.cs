namespace common.Dtos.Response;

public record UserListResponse(IEnumerable<UserDto> users);