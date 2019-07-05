using Socratic.DataAccess.Abstractions;

namespace grpcSample.Server.Data.Entities {

    public class Student : IDbEntity {

        public int Id { get; set; }
        public string Name { get; set; }

    }

}