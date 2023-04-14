using AGVSystemCommonNet6.UserManagers;
using AGVSytemCommonNet6.TASK;
using Microsoft.EntityFrameworkCore;

namespace AGVSystemCommonNet6.DATABASE
{
    public class AGVSDbContext : DbContext
    {
        public AGVSDbContext(DbContextOptions<AGVSDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<clsTaskState> Tasks { get; set; }
    }
}
