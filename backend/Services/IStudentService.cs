using common.Dtos;
using common.Dtos.Request;

namespace backend.Services;

public interface IStudentService
{
    Task<StudentDto> Create(StudentCreateRequest request);
    Task<StudentDto> Find(StudentFindRequest request);
    Task<StudentDto> Update(StudentUpdateRequest request);
    Task<StudentDto> Delete(StudentDeleteRequest request);
}