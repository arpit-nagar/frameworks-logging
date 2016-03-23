using System.Data.Entity.ModelConfiguration;
using Tavisca.Frameworks.Logging.Extensions.Data.Models;

namespace Tavisca.Frameworks.Logging.Extensions.Data.Mapping
{
    public class SlowBookingMap : EntityTypeConfiguration<SlowBooking>
    {
        public SlowBookingMap()
        {
            // Primary Key
            this.HasKey(t => t.LogId);

            // Properties
            this.Property(t => t.SupplierName)
                .HasMaxLength(32);

            this.Property(t => t.SupplierConfirmationNo)
                .HasMaxLength(16);

            this.Property(t => t.VendorConfirmationNo)
                .HasMaxLength(16);

            this.Property(t => t.Status)
                .HasMaxLength(8);

            // Table & Column Mappings
            this.ToTable("SlowBooking");
            this.Property(t => t.LogId).HasColumnName("LogId");
            this.Property(t => t.SupplierName).HasColumnName("SupplierName");
            this.Property(t => t.TripProductId).HasColumnName("TripProductId");
            this.Property(t => t.TripFolderId).HasColumnName("TripFolderId");
            this.Property(t => t.SupplierConfirmationNo).HasColumnName("SupplierConfirmationNo");
            this.Property(t => t.VendorConfirmationNo).HasColumnName("VendorConfirmationNo");
            this.Property(t => t.SessionId).HasColumnName("SessionId");
            this.Property(t => t.SupplierId).HasColumnName("SupplierId");
            this.Property(t => t.TimeTaken).HasColumnName("TimeTaken");
            this.Property(t => t.AddDate).HasColumnName("AddDate");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.Message).HasColumnName("Message");
            this.Property(t => t.AdditionalInfo).HasColumnName("AdditionalInfo");
        }
    }
}
