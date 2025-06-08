namespace common.Dtos.Request;

public enum UserRoleModifyOperation
{
    Add = 1,
    Remove
}

public record UserRoleModifyRequest(Guid UserId, UserRoleModifyOperation Operation, string Role);