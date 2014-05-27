CREATE TABLE [dbo].[$safeprojectname$]
(
	[tokenId] NVARCHAR(255) NOT NULL PRIMARY KEY, 
    [tokenCreated] DATETIME NOT NULL, 
    [tokenObjectSerialized] NVARCHAR(MAX) NOT NULL
)

GO


