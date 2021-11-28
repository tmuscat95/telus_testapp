using System;
using Microsoft.EntityFrameworkCore;
using TestApplication.Data.DbModels;

namespace TestApplication.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) :base(options)
        {
        }

        public DbSet<Account> accounts { get; set; }
        public DbSet<QueueGroup> queueGroups { get; set; }
        public DbSet<MonitorData> monitorData { get; set; }
        
    }
}
