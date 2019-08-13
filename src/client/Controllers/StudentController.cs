using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using grpcSample.contract;
using Microsoft.AspNetCore.Mvc;
using static grpcSample.contract.Students;

namespace grpcSample.client.Controllers {
    
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase {
        private readonly StudentsClient client;

        public StudentController(StudentsClient client) => this.client = client;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetTask() {
            using var call = client.GetAllStudents(new Empty());
            var responseStream = call.ResponseStream;
            var results = new List<StudentDto>();

            while (await responseStream.MoveNext())
            {
                var student = responseStream.Current;
                results.Add(student);
            }
            return Ok(results);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<string>> Get(int id) {
            var result = await client.GetStudentAsync(new GetStudentRequest { Id = id });
            return Ok(result.Name);
        }
        
        [HttpPost]
        public async Task<ActionResult<int>> Post([FromBody] string value) {
            var result = await client.AddStudentAsync(new NewStudentRequest {
                Name = value
            });
            return Ok(result.Id);
        }
    }
}