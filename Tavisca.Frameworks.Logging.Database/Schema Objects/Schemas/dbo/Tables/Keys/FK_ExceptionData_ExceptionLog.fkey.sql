ALTER TABLE [dbo].[ExceptionData]
    ADD CONSTRAINT [FK_ExceptionData_ExceptionLog] FOREIGN KEY ([LogId]) REFERENCES [dbo].[ExceptionLog] ([LogID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

