using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnOff.Todo.Api.Domain.Entities;

namespace OnOff.Todo.Api.Infrastructure.Data
{
    // Este contexto incluye toda la estructura de Identity y la tabla de tareas
    public class ApplicationDbContext 
        : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Tabla de tareas
        public DbSet<TodoTask> TodoTasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Primero dejamos que Identity configure sus propias tablas
            base.OnModelCreating(modelBuilder);

            // Relación entre usuario y tarea: un usuario puede tener muchas tareas
            modelBuilder.Entity<TodoTask>()
                .HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId);

            // Sembramos un usuario de ejemplo usando el hasher de Identity
            var hasher = new PasswordHasher<ApplicationUser>();

            var demoUser = new ApplicationUser
            {
                Id = 1,
                UserName = "demo@onoff.com",
                NormalizedUserName = "DEMO@ONOFF.COM",
                Email = "demo@onoff.com",
                NormalizedEmail = "DEMO@ONOFF.COM",
                EmailConfirmed = true,
                FullName = "Usuario Demo"
            };

            demoUser.PasswordHash = hasher.HashPassword(demoUser, "123456");

            modelBuilder.Entity<ApplicationUser>().HasData(demoUser);
        }
    }
}
