using System.Net;
using AutoMapper;
using backend.Exceptions;
using backend.Models;
using common.Dtos;
using common.Dtos.Request;
using common.Dtos.Response;
using Microsoft.EntityFrameworkCore;

namespace backend.Services.Impl;

public class StudentService(IMapper mapper, StorageManagerDbContext dbContext) : IStudentService
{
    
    public async Task<StudentDto> Create(StudentCreateRequest request)
    {
        var student = new Student();
        student.Name = request.Name;
        student.Group = request.Group;
        var entry = await dbContext.Students.AddAsync(student);
        await dbContext.SaveChangesAsync();
        return mapper.Map<StudentDto>(entry.Entity);
    }

    public async Task<StudentDto> Find(StudentFindRequest request)
    {
        var studentDto = dbContext.Students
            .Where(x => x.Name.Equals(request.Name) && (request.Group == null || x.Group.Equals(request.Group)))
            .Select(x => mapper.Map<StudentDto>(x))
            .FirstOrDefault();

        if (studentDto == null)
            throw new HttpResponseException(
                (int) HttpStatusCode.NotFound,
                new HttpErrorMessageResponse($"Student with name {request.Name} not found.")
            );

        return studentDto;
    }

    public async Task<StudentDto> GetInfo(StudentGetInfoRequest request)
    {
        var studentDto = dbContext.Students
            .Where(x => x.Id.Equals(request.StudentId))
            .Select(x => mapper.Map<StudentDto>(x))
            .FirstOrDefault();

        if (studentDto == null)
            throw new HttpResponseException(
                (int) HttpStatusCode.NotFound,
                new HttpErrorMessageResponse($"Student with name {request.StudentId} not found.")
            );

        return studentDto;
    }

    public async Task<StudentDto> Update(StudentUpdateRequest request)
    {
        var student = dbContext.Students
            .FirstOrDefault(x => x.Id.Equals(request.StudentId));
        
        if (student == null)
            throw new HttpResponseException(
                (int) HttpStatusCode.NotFound,
                new HttpErrorMessageResponse($"Student with id {request.StudentId} not found.")
            );

        student.Name = request.Name ?? student.Name;
        student.Group = request.Group ?? student.Group;

        dbContext.Students.Update(student);
        await dbContext.SaveChangesAsync();

        return mapper.Map<StudentDto>(student);
    }

    public async Task<StudentDto> Delete(StudentDeleteRequest request)
    {
        var student = dbContext.Students
            .FirstOrDefault(x => x.Id.Equals(request.StudentId));
        
        if (student == null)
            throw new HttpResponseException(
                (int) HttpStatusCode.NotFound,
                new HttpErrorMessageResponse($"Student with id {request.StudentId} not found.")
            );

        dbContext.Students.Remove(student);
        await dbContext.SaveChangesAsync();

        return mapper.Map<StudentDto>(student);
    }
}