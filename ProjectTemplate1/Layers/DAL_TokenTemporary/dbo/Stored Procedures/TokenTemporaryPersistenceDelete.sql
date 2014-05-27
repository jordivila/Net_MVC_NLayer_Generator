Create procedure [dbo].TokenTemporaryPersistenceDelete
		@id as [varchar](255)
	as
	begin

		delete from [dbo].[$safeprojectname$] where tokenId = @id

		select @@rowcount

	end