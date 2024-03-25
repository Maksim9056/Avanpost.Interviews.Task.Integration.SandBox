
//using Avanpost.Interviews.Task.Integration.Data.DbCommon;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection.Emit;
//using System.Text;
//using System.Threading.Tasks;

namespace Avanpost.Interviews.Task.Integration.SandBox.Connector
{
    public class Dbcs : DbContext
    {

        public string SQL = "";
        public DbSet<User> Users => Set<User>();

        public DbSet<RequestRight> RequestRights => Set<RequestRight>();

        public DbSet<UserRequestRight> UserRequestRights => Set<UserRequestRight>();

        public DbSet<UserITRole> UserITRoles => Set<UserITRole>();

        public DbSet<ITRole> ITRoles => Set<ITRole>();

        public DbSet<Sequrity> Passwords => Set<Sequrity>();

        //public Dbcs(DbContextOptions<DataContext> options)
        //    : base(options)
        //{
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRequestRight>().HasKey("RightId", "UserId");
            modelBuilder.Entity<UserITRole>().HasKey("RoleId", "UserId");
            base.OnModelCreating(modelBuilder);
        }


        public Dbcs(string sqQL)
        {
            SQL = sqQL;
            // Database.EnsureDeleted(); // гарантируем, что бд удалена
            Database.EnsureCreated(); // гарантируем, что бд будет созд
                                      // Database.Migrate();  // миграция
                                      // Database.MigrateAsync(); // асинхронный метод для миграции
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=testDb;Username=postgres;Password=1");

        }
    }
}
