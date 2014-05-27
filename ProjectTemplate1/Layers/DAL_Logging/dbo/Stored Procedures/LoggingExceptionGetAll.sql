
Create procedure [dbo].[LoggingExceptionGetAll]
	@filter xml
as
begin

	--In case this Stored runs under SQL Server 2012 feel free to use "offset" instead of "row_number over"

	set nocount on;

	declare @logTraceSourceSelected varchar(255)
	declare @logDateCreationFrom datetime, @logDateCreationTo datetime
	declare @sqlcommand nvarchar(max)
	declare @sqlcommandWhereClause nvarchar(max)
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
			@logDateCreationFrom = r.rootnode.value('CreationDate[1][not(@xsi:nil = "true")]', 'datetime'),
			@pageIndex = r.rootnode.value('Page[1][not(@xsi:nil = "true")]', 'int'),
			@pageSize = r.rootnode.value('PageSize[1][not(@xsi:nil = "true")]', 'int'),
			@sortBy = r.rootnode.value('SortBy[1][not(@xsi:nil = "true")]', 'nvarchar(255)'),
			@sortAscending = r.rootnode.value('SortAscending[1][not(@xsi:nil = "true")]', 'bit')
	from	@filter.nodes('/DataFilterLogger') as r(rootnode)
	
	set @logDateCreationTo =  DATEADD(DAY, 1, @logDateCreationFrom)
	
	select 
			@sortBy = ISNULL(@sortBy, 'LogID'), 
			@sortAscendingDescription = 
			case 
				when (ISNULL(@sortAscending, 1)) = 0 then 'desc' 
				else 'asc' 
			end


	set @sqlcommandWhereClause = N' from
										[log]
										inner JOIN CategoryLog on CategoryLog.CategoryLogID = [Log].LogID
										inner join Category on Category.CategoryID = CategoryLog.CategoryID
									where
										CategoryName = @logTraceSourceSelected
										and [Timestamp] between @logDateCreationFrom and @logDateCreationTo'

	set @sqlcommand = N'
						select	count(*) TotalCount,
								case (count(*)) % @pageSize 
									when 0 then count(*) / @pageSize
									else	(count(*) / @pageSize) + 1
								end TotalPages
						' + @sqlcommandWhereClause + '

						select * from (select 
											[Log].[logid]
											,[Log].[eventid]
											,[Log].[priority]
											,[Log].[severity]
											,[Log].[title]
											,[Log].[timestamp]
											,[Log].[machinename]
											,[Log].[appdomainname]
											,[Log].[processid]
											,[Log].[processname]
											,[Log].[threadname]
											,[Log].[win32threadid]
											,[Log].[message]
											,[Log].[formattedmessage] 
											,row_number() over ( order by [Log].' + @sortBy + ' ' + @sortAscendingDescription + ') as rowNumber
						' + @sqlcommandWhereClause + ' 
										)as tmp
						where rowNumber between (@pageindex * @pagesize) and ((@pageindex * @pagesize)+@pagesize)
						' 
						-- order by ' + @sortby + ' ' + @sortAscendingDescription;


	set @sqlCommandParametersDefinition = N'@logTraceSourceSelected varchar(256), @logDateCreationFrom datetime, @logDateCreationTo datetime, @pageIndex int , @pageSize int';

	execute sp_executesql @sqlcommand, 
							@sqlCommandParametersDefinition, 
							@logTraceSourceSelected = @logTraceSourceSelected,
							@logDateCreationFrom = @logDateCreationFrom,
							@logDateCreationTo = @logDateCreationTo,
							@pageIndex = @pageIndex,
							@pageSize = @pageSize

end

