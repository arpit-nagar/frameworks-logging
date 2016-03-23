using System.Data.Entity.ModelConfiguration;
using Tavisca.Frameworks.Logging.Extensions.Data.Models;

namespace Tavisca.Frameworks.Logging.Extensions.Data.Mapping
{
    public class AuditTrailMap : EntityTypeConfiguration<AuditTrail>
    {
        public AuditTrailMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ChainId)
                .HasMaxLength(1024);

            this.Property(t => t.ReferenceNumber)
                .HasMaxLength(1024);

            this.Property(t => t.EventType)
                .HasMaxLength(124);

            this.Property(t => t.IPAddress)
                .HasMaxLength(50);

            this.Property(t => t.Username)
                .HasMaxLength(1024);

            this.Property(t => t.Application)
                .HasMaxLength(1024);

            this.Property(t => t.Module)
                .HasMaxLength(1024);

            this.Property(t => t.Status)
                .HasMaxLength(50);

            this.Property(t => t.Tags)
                .HasMaxLength(2048);

            // Table & Column Mappings
            this.ToTable("AuditTrail");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.TimeStamp).HasColumnName("TimeStamp");
            this.Property(t => t.ChainId).HasColumnName("ChainId");
            this.Property(t => t.ReferenceNumber).HasColumnName("ReferenceNumber");
            this.Property(t => t.EventType).HasColumnName("EventType");
            this.Property(t => t.IPAddress).HasColumnName("IPAddress");
            this.Property(t => t.Username).HasColumnName("Username");
            this.Property(t => t.Application).HasColumnName("Application");
            this.Property(t => t.Module).HasColumnName("Module");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.Tags).HasColumnName("Tags");
        }
    }
}
