using System;

namespace Tavisca.Frameworks.Logging.Extensions.Data.Models
{
    public class SlowBooking
    {
        public int LogId { get; set; }
        public string SupplierName { get; set; }
        public Nullable<System.Guid> TripProductId { get; set; }
        public Nullable<System.Guid> TripFolderId { get; set; }
        public string SupplierConfirmationNo { get; set; }
        public string VendorConfirmationNo { get; set; }
        public Nullable<System.Guid> SessionId { get; set; }
        public Nullable<int> SupplierId { get; set; }
        public Nullable<double> TimeTaken { get; set; }
        public System.DateTime AddDate { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public string AdditionalInfo { get; set; }
    }
}
