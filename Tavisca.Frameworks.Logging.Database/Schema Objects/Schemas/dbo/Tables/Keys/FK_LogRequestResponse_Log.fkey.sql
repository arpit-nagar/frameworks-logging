ALTER TABLE [dbo].[LogRequestResponse]
    ADD CONSTRAINT [FK_LogRequestResponse_Log] FOREIGN KEY ([LogId]) REFERENCES [dbo].[Log] ([LogID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

