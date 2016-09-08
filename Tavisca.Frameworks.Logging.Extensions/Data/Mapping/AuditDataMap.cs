using System.Data.Entity.ModelConfiguration;
using Tavisca.Frameworks.Logging.Extensions.Data.Models;

namespace Tavisca.Frameworks.Logging.Extensions.Data.Mapping
{
    public class AuditDataMap : EntityTypeConfiguration<AuditData>
    {
        public AuditDataMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Key)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("AuditData");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.LogId).HasColumnName("LogId");
            this.Property(t => t.Key).HasColumnName("Key");
            this.Property(t => t.Value).HasColumnName("Value");
        }
    }
}
