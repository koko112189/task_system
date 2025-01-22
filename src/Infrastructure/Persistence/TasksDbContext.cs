using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public class TasksDbContext : IdentityDbContext<User>
    {
        public TasksDbContext(DbContextOptions<TasksDbContext> options)
            : base(options) { }

        public DbSet<UserTask> Tasks { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany(u => u.UserTasks)
                .WithOne(t => t.AssignedUser).
                    HasForeignKey(t => t.AssignedUserId);
        }
    }
}
