using grpcSample.Server.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace grpcSample.Server.Data {

    public class SchoolContext : DbContext {

        public SchoolContext(DbContextOptions options) : base(options) { }

        public DbSet<Student> Students { get; set; }

    }

}