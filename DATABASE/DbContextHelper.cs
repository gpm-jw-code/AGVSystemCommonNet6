using Microsoft.EntityFrameworkCore;
using AGVSystemCommonNet6.UserManagers;
using AGVSytemCommonNet6.TASK;

namespace AGVSystemCommonNet6.DATABASE
{
    internal class DbContextHelper : IDisposable
    {
        private readonly string _connectionString;
        internal AGVSDbContext _context;
        private bool disposedValue;

        public DbContextHelper(string connectionString)
        {
            _connectionString = connectionString;
            CreateDbContext();
        }

        private void CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AGVSDbContext>();
            optionsBuilder.UseSqlite(_connectionString);
            _context = new AGVSDbContext(optionsBuilder.Options);
        }

        public Repository<T> CreateRepository<T>(AGVSDbContext dbContext) where T : class
        {
            return new Repository<T>(dbContext);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 處置受控狀態 (受控物件)
                }
                _context.Dispose();
                // TODO: 釋出非受控資源 (非受控物件) 並覆寫完成項
                // TODO: 將大型欄位設為 Null
                disposedValue = true;
            }
        }

        // // TODO: 僅有當 'Dispose(bool disposing)' 具有會釋出非受控資源的程式碼時，才覆寫完成項
        // ~DbContextHelper()
        // {
        //     // 請勿變更此程式碼。請將清除程式碼放入 'Dispose(bool disposing)' 方法
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 請勿變更此程式碼。請將清除程式碼放入 'Dispose(bool disposing)' 方法
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

}
