
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

