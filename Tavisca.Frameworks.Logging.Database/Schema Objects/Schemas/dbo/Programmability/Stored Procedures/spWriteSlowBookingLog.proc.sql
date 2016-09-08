CREATE PROCEDURE [dbo].[spWriteSlowBookingLog](
	@SupplierName [nvarchar](32),
	@TripProductId [uniqueidentifier] ,
	@TripFolderId [uniqueidentifier] ,
	@SupplierConfirmationNo [nvarchar](16) ,
	@VendorConfirmationNo [nvarchar](16) ,
	@SessionId [uniqueidentifier] ,
	@SupplierId [int] ,
	@TimeTaken [float],
	@AddDate [datetime] ,
	@Status [nvarchar](8) ,
	@Message [nvarchar](max) ,
	@AdditionalInfo [nvarchar](max)
)
AS 
	INSERT INTO [SlowBooking] (
		
	[SupplierName] ,
	[TripProductId],
	[TripFolderId] ,
	[SupplierConfirmationNo] ,
	[VendorConfirmationNo] ,
	[SessionId],
	[SupplierId],
	[TimeTaken],
	[AddDate],
	[Status] ,
	[Message],
	[AdditionalInfo]
	)
	VALUES (
		@SupplierName , 
		@TripProductId, 
		@TripFolderId, 
		@SupplierConfirmationNo, 
		@VendorConfirmationNo,
		@SessionId, 
		@SupplierId,
		@TimeTaken,
		@AddDate,
		@Status,
		@Message,
		@AdditionalInfo )