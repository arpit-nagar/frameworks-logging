
CREATE Procedure [dbo].[spDeleteLog]

	(
		@LogID int
	)


AS

DELETE FROM [Log] WHERE LogID = @LogId