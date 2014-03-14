	SET ANSI_NULLS ON
	GO

	SET QUOTED_IDENTIFIER ON
	GO

	SET ANSI_PADDING ON
	GO

	IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[TokenTemporaryPersistence]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
	BEGIN

		CREATE TABLE [dbo].[TokenTemporaryPersistence]
		(
			[tokenId] [varchar](255) NOT NULL,
			[tokenCreated] [datetime] NOT NULL,
			[tokenObjectSerialized] [varchar](max) NOT NULL,
			CONSTRAINT [TokenTemporaryPersistence_PK] PRIMARY KEY CLUSTERED 
			(
				[tokenId] ASC
			)
			WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
		) 
		ON [PRIMARY]
	END
	GO

	IF (EXISTS (SELECT name
				  FROM sysobjects
				 WHERE (name = N'TokenTemporaryPersistenceGetById')
				   AND (type = 'P')))

		DROP PROCEDURE dbo.TokenTemporaryPersistenceGetById
	GO

	Create procedure [dbo].[TokenTemporaryPersistenceGetById]
		@id as [varchar](255)
	as
	begin
		select	
				[tokenId]
				,[tokenCreated]
				,[tokenObjectSerialized]
		from  
				[dbo].[TokenTemporaryPersistence]
		where
				[tokenId] = @id
	end
	
	GO

	IF (EXISTS (SELECT name
				  FROM sysobjects
				 WHERE (name = N'TokenTemporaryPersistenceInsert')
				   AND (type = 'P')))

		DROP PROCEDURE dbo.TokenTemporaryPersistenceInsert
	GO

	Create procedure [dbo].[TokenTemporaryPersistenceInsert]
		@id as [varchar](255),
		@tokenCreated [datetime],
		@tokenObjectSerialized [varchar](max)
	as
	begin

		insert into [dbo].[TokenTemporaryPersistence] ([tokenId], [tokenCreated] ,[tokenObjectSerialized])
		select	@id, @tokenCreated, @tokenObjectSerialized

	end
	
	GO
	
	IF (EXISTS (SELECT name
				  FROM sysobjects
				 WHERE (name = N'TokenTemporaryPersistenceDelete')
				   AND (type = 'P')))

		DROP PROCEDURE dbo.TokenTemporaryPersistenceDelete
	GO


	Create procedure [dbo].TokenTemporaryPersistenceDelete
		@id as [varchar](255)
	as
	begin

		delete from [dbo].[TokenTemporaryPersistence] where tokenId = @id

		select @@rowcount

	end

GO
