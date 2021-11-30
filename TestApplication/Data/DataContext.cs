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

        virtual public DbSet<Account> accounts { get; set; }
        virtual public DbSet<QueueGroup> queueGroups { get; set; }
        virtual public DbSet<MonitorData> monitorData { get; set; }
        
    }
}
