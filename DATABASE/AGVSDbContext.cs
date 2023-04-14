using AGVSystemCommonNet6.UserManagers;
using AGVSytemCommonNet6.TASK;
using Microsoft.EntityFrameworkCore;
using System.Xml;

namespace AGVSystemCommonNet6.DATABASE
{
    public class AGVSDbContext : DbContext
    {
        public AGVSDbContext(DbContextOptions<AGVSDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<clsTaskDto> Tasks { get; set; }
    }
}
