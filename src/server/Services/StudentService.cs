using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using grpcSample.contract;
using grpcSample.Server.Data;
using grpcSample.Server.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Socratic.DataAccess.Abstractions;

namespace grpcSample.Server.Services {

    public class StudentService : Students.StudentsBase {

        private readonly IUnitOfWork<SchoolContext> uow;

        public StudentService(IUnitOfWork<SchoolContext> uow) => this.uow = uow;

        public override async Task<StudentDto> AddStudent(NewStudentRequest request, ServerCallContext context) {

            var newStudent = new Student {
                Name = request.Name
            };

            uow.Context<Student>().Add(newStudent);
            await uow.CommitAsync();

            return new StudentDto {
                Id = newStudent.Id,
                Name = newStudent.Name
            };

        }

        public override async Task<StudentDto> GetStudent(GetStudentRequest request, ServerCallContext context) {

            var result = await uow.Context<Student>().GetAll()
                .SingleOrDefaultAsync(x => x.Id == request.Id);

            return new StudentDto {
                Id = result.Id,
                Name = result.Name
            };

        }

        public override async Task GetAllStudents(Empty request, IServerStreamWriter<StudentDto> responseStream
            , ServerCallContext context) {

            var students = uow.Context<Student>().GetAll();

            foreach (var student in students)
                await responseStream.WriteAsync(new StudentDto { Id = student.Id, Name = student.Name });

        }

    }

}