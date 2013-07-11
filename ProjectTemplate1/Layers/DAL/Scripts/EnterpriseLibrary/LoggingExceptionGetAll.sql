SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create procedure [dbo].[LoggingExceptionGetAll]
	@filter xml
as
begin

	--In case this Stored runs under SQL Server 2012 feel free to use "offset" instead of "row_number over"

	set nocount on;

	declare @logTraceSourceSelected varchar(255)
	declare @sqlcommand nvarchar(max)
	declare @sqlCommandParametersDefinition nvarchar(max);
	declare @sortAscendingDescription varchar(255)
	declare @totalcount int = 0
	declare @totalpages int = 0
	declare @pageIndex int = 0
	declare @pageSize int
	declare @sortBy varchar(255) = null
	declare @sortAscending bit = null

	select	
			@logTraceSourceSelected = r.rootnode.value('LogTraceSourceSelected[1][not(@xsi:nil = "true")]', 'nvarchar(255)'),
			@pageIndex = r.rootnode.value('Page[1][not(@xsi:nil = "true")]', 'int'),
			@pageSize = r.rootnode.value('PageSize[1][not(@xsi:nil = "true")]', 'int'),
			@sortBy = r.rootnode.value('SortBy[1][not(@xsi:nil = "true")]', 'nvarchar(255)'),
			@sortAscending = r.rootnode.value('SortAscending[1][not(@xsi:nil = "true")]', 'bit')
	from	@filter.nodes('/DataFilterLogger') as r(rootnode)
	
	select 
			@sortBy = ISNULL(@sortBy, 'LogID'), 
			@sortAscendingDescription = 
			case 
				when (ISNULL(@sortAscending, 1)) = 0 then 'desc' 
				else 'asc' 
			end

	set @sqlcommand = N'
						select	count(*) TotalCount,
								case (count(*)) % @pageSize 
									when 0 then count(*) / @pageSize
									else	(count(*) / @pageSize) + 1
								end TotalPages
						from
							[dbo].[log]
						where
							Title like ''%'' + @logTraceSourceSelected + ''%''

						select * from (select 
											[logid]
											,[eventid]
											,[priority]
											,[severity]
											,[title]
											,[timestamp]
											,[machinename]
											,[appdomainname]
											,[processid]
											,[processname]
											,[threadname]
											,[win32threadid]
											,[message]
											,[formattedmessage] 
											,row_number() over ( order by ' + @sortBy + ' ' + @sortAscendingDescription + ') as rowNumber
										from
											[dbo].[log]
										where
											Title like ''%'' + @logTraceSourceSelected + ''%''
										)as tmp
						where rowNumber between (@pageindex * @pagesize) and ((@pageindex * @pagesize)+@pagesize)
						order by ' + @sortby + ' ' + @sortAscendingDescription;


	set @sqlCommandParametersDefinition = N'@logTraceSourceSelected varchar(256), @pageIndex int , @pageSize int';

	execute sp_executesql @sqlcommand, 
							@sqlCommandParametersDefinition, 
							@logTraceSourceSelected = @logTraceSourceSelected,
							@pageIndex = @pageIndex,
							@pageSize = @pageSize

end

GO

--declare @p5 xml
--set @p5=convert(xml,
--N'<DataFilterLogger xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
--	<LogTraceSourceSelected>BeginREquest</LogTraceSourceSelected>
--	<IsClientVisible>false</IsClientVisible>
--	<Page>0</Page>
--	<PageSize>10</PageSize>
--	<SortAscending>false</SortAscending>
--</DataFilterLogger>')
--exec LoggingExceptionGetAll @filter=@p5
