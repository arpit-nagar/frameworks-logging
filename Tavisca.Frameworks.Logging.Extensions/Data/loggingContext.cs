using System.Data.Entity;
using Tavisca.Frameworks.Logging.Extensions.Data.Mapping;
using Tavisca.Frameworks.Logging.Extensions.Data.Models;

namespace Tavisca.Frameworks.Logging.Extensions.Data
{
    public class LoggingContext : DbContext
    {
        static LoggingContext()
        {
            Database.SetInitializer<LoggingContext>(null);
        }

        public LoggingContext()
			: base("Name=log")
		{
		}

        public DbSet<AuditData> AuditDatas { get; set; }
        public DbSet<AuditTrail> AuditTrails { get; set; }
        public DbSet<ExceptionData> ExceptionDatas { get; set; }
        public DbSet<ExceptionLog> ExceptionLogs { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<LogData> LogDatas { get; set; }
        public DbSet<LogRequestResponse> LogRequestResponses { get; set; }
        public DbSet<MaskingExpression> MaskingExpressions { get; set; }
        public DbSet<SlowBooking> SlowBookings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AuditDataMap());
            modelBuilder.Configurations.Add(new AuditTrailMap());
            modelBuilder.Configurations.Add(new ExceptionDataMap());
            modelBuilder.Configurations.Add(new ExceptionLogMap());
            modelBuilder.Configurations.Add(new LogMap());
            modelBuilder.Configurations.Add(new LogDataMap());
            modelBuilder.Configurations.Add(new LogRequestResponseMap());
            modelBuilder.Configurations.Add(new MaskingExpressionMap());
            modelBuilder.Configurations.Add(new SlowBookingMap());
        }
    }
}
