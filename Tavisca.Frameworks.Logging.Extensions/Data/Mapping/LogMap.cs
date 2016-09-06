using System.Data.Entity.ModelConfiguration;
using Tavisca.Frameworks.Logging.Extensions.Data.Models;

namespace Tavisca.Frameworks.Logging.Extensions.Data.Mapping
{
    public class LogMap : EntityTypeConfiguration<Log>
    {
        public LogMap()
        {
            // Primary Key
            this.HasKey(t => t.LogID);

            // Properties
            this.Property(t => t.Severity)
                .IsRequired()
                .HasMaxLength(32);

            this.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(256);

            this.Property(t => t.MachineName)
                .IsRequired()
                .HasMaxLength(32);

            this.Property(t => t.AppDomainName)
                .IsRequired()
                .HasMaxLength(512);

            this.Property(t => t.ProcessID)
                .IsRequired()
                .HasMaxLength(256);

            this.Property(t => t.ProcessName)
                .IsRequired()
                .HasMaxLength(512);

            this.Property(t => t.ThreadName)
                .HasMaxLength(512);

            this.Property(t => t.Win32ThreadId)
                .HasMaxLength(128);

            this.Property(t => t.Message)
                .HasMaxLength(2048);

            this.Property(t => t.ServiceUrl)
                .HasMaxLength(256);

            this.Property(t => t.MethodName)
             .HasMaxLength(64);

            this.Property(t => t.Status)
                .HasMaxLength(32);

            this.Property(t => t.UsersessionId)
                .HasMaxLength(256);

            this.Property(t => t.UserIdentifier)
                .HasMaxLength(256);

            this.Property(t => t.ApplicationName)
                .HasMaxLength(256);

            this.Property(t => t.ContextIdentifier)
                .HasMaxLength(128);

            this.Property(t => t.IpAddress)
                .HasMaxLength(256);

            // Table & Column Mappings
            this.ToTable("Log");
            this.Property(t => t.LogID).HasColumnName("LogID");
            this.Property(t => t.Priority).HasColumnName("Priority");
            this.Property(t => t.Severity).HasColumnName("Severity");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Timestamp).HasColumnName("Timestamp");
            this.Property(t => t.MachineName).HasColumnName("MachineName");
            this.Property(t => t.AppDomainName).HasColumnName("AppDomainName");
            this.Property(t => t.ProcessID).HasColumnName("ProcessID");
            this.Property(t => t.ProcessName).HasColumnName("ProcessName");
            this.Property(t => t.ThreadName).HasColumnName("ThreadName");
            this.Property(t => t.Win32ThreadId).HasColumnName("Win32ThreadId");
            this.Property(t => t.Message).HasColumnName("Message");
            this.Property(t => t.SessionId).HasColumnName("SessionId");
            this.Property(t => t.ServiceUrl).HasColumnName("ServiceUrl");
            this.Property(t => t.MethodName).HasColumnName("MethodName");
            this.Property(t => t.ProviderId).HasColumnName("ProviderId");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.TimeTaken).HasColumnName("TimeTaken");
            this.Property(t => t.UsersessionId).HasColumnName("UsersessionId");
            this.Property(t => t.UserIdentifier).HasColumnName("UserIdentifier");
            this.Property(t => t.ApplicationName).HasColumnName("ApplicationName");
            this.Property(t => t.ContextIdentifier).HasColumnName("ContextIdentifier");
            this.Property(t => t.IpAddress).HasColumnName("IpAddress");
        }
    }
}
