using System.Data.Entity.ModelConfiguration;
using Tavisca.Frameworks.Logging.Extensions.Data.Models;

namespace Tavisca.Frameworks.Logging.Extensions.Data.Mapping
{
    public class LogRequestResponseMap : EntityTypeConfiguration<LogRequestResponse>
    {
        public LogRequestResponseMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("LogRequestResponse");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.LogId).HasColumnName("LogId");
            this.Property(t => t.Request).HasColumnName("Request");
            this.Property(t => t.Response).HasColumnName("Response");

            // Relationships
            this.HasRequired(t => t.Log)
                .WithMany(t => t.LogRequestResponses)
                .HasForeignKey(d => d.LogId);

        }
    }
}
