CREATE TABLE [dbo].[CategoryLog] (
    [CategoryLogID] INT IDENTITY (1, 1) NOT NULL,
    [CategoryID]    INT NOT NULL,
    [LogID]         INT NOT NULL,
    CONSTRAINT [PK_CategoryLog] PRIMARY KEY CLUSTERED ([CategoryLogID] ASC),
    CONSTRAINT [FK_CategoryLog_Category] FOREIGN KEY ([CategoryID]) REFERENCES [dbo].[Category] ([CategoryID]),
    CONSTRAINT [FK_CategoryLog_Log] FOREIGN KEY ([LogID]) REFERENCES [dbo].[Log] ([LogID])
);


GO
CREATE NONCLUSTERED INDEX [ixCategoryLog]
    ON [dbo].[CategoryLog]([LogID] ASC, [CategoryID] ASC);

