using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Tavisca.Frameworks.Logging.Extensions.Data.Models;

namespace Tavisca.Frameworks.Logging.Extensions.Data.Mapping
{
    public class ExceptionDataMap : EntityTypeConfiguration<ExceptionData>
    {
        public ExceptionDataMap()
        {
            // Primary Key
            this.HasKey(t => new { t.LogId, t.Key });

            // Properties
            this.Property(t => t.LogId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Key)
                .IsRequired()
                .HasMaxLength(256);

            this.Property(t => t.Value);

            // Table & Column Mappings
            this.ToTable("ExceptionData");
            this.Property(t => t.LogId).HasColumnName("LogId");
            this.Property(t => t.Key).HasColumnName("Key");
            this.Property(t => t.Value).HasColumnName("Value");

            // Relationships
            this.HasRequired(t => t.ExceptionLog)
                .WithMany(t => t.ExceptionDatas)
                .HasForeignKey(d => d.LogId);

        }
    }
}
