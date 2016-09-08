using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Tavisca.Frameworks.Logging.Extensions.Data.Models;

namespace Tavisca.Frameworks.Logging.Extensions.Data.Mapping
{
    public class MaskingExpressionMap : EntityTypeConfiguration<MaskingExpression>
    {
        public MaskingExpressionMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ProviderName, t.AddDate });

            // Properties
            this.Property(t => t.ProviderName)
                .IsRequired()
                .HasMaxLength(16);

            this.Property(t => t.CallType)
                .HasMaxLength(512);

            this.Property(t => t.Regex)
                .HasMaxLength(512);

            this.Property(t => t.ReplacementExpression)
                .HasMaxLength(512);

            // Table & Column Mappings
            this.ToTable("MaskingExpressions");
            this.Property(t => t.ProviderName).HasColumnName("ProviderName");
            this.Property(t => t.CallType).HasColumnName("CallType");
            this.Property(t => t.Regex).HasColumnName("Regex");
            this.Property(t => t.ReplacementExpression).HasColumnName("ReplacementExpression");
            this.Property(t => t.AddDate).HasColumnName("AddDate");
        }
    }
}