SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create procedure [dbo].[LoggingExceptionGetById]

	@exceptionGuid as nvarchar(256)
AS
begin

	set nocount on;

	select 
		[LogID]
		,[EventID]
		,[Priority]
		,[Severity]
		,[Title]
		,[Timestamp]
		,[MachineName]
		,[AppDomainName]
		,[ProcessID]
		,[ProcessName]
		,[ThreadName]
		,[Win32ThreadId]
		,[Message]
		,cast([FormattedMessage] as xml)
   from 
		[dbo].[Log]
   where 
		 Title LIKE '%' + @exceptionGuid + '%'
end

go

-- exec LoggingExceptionGetById 'f1439af0-1b29-4f7b-86a0-24077d248b3f'
