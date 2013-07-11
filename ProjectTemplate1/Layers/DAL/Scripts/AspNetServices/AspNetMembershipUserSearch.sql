SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create procedure [dbo].[aspnet_Membership_FindUsersByFilter] 
(
	@applicationName varchar(256),
	@MinutesSinceLastInActive int,
	@CurrentTimeUtc datetime,
	@filter xml
)
as

	set nocount on;
	set quoted_identifier on;

	declare @dateInActive datetime
	declare @rowstart int 
	declare @rowend int 
	declare @totalcount int
	declare @totalpages int
	declare @pageIndex int
	declare @pageSize int
	declare @sortType varchar(255)
	declare @sortAscending varchar(255)
	declare @tmpfilter table
	( 
		 UserName nvarchar(256)
		,CreationDateFrom datetime null
		,CreationDateTo datetime null
		,Email varchar(256) null
		,Locked bit null
		,Approved bit null
		,IsOnline bit null
		,PageIndex int
		,PageSize int
		,SortType varchar(255)
		,SortAscending varchar(255)
	)
	
	declare @tmpFilterRoles table
	(
		roleId uniqueidentifier,
		IsInRoleName nvarchar(256)
	)
	
	declare @tmpResults table
	(
		UserName nvarchar(256) null, 
		Email nvarchar(256) null, 
		PasswordQuestion nvarchar(256) null, 
		Comment ntext null, 
		IsApproved bit null,
		CreateDate datetime null,
		LastLoginDate datetime null,
		LastActivityDate datetime null,
		LastPasswordChangedDate datetime null,
		ProviderUserKey uniqueidentifier null, 
		IsLockedOut bit null,
		LastLockoutDate datetime null,
		[rownumber] int	
	)
	

	insert into @tmpfilter
	select
			 r.rootnode.value('UserName[1][not(@xsi:nil = "true")]', 'nvarchar(256)')	as [UserName]
			,r.rootnode.value('CreationDateFrom[1][not(@xsi:nil = "true")]', 'datetime')	as [CreationDateFrom]
			,r.rootnode.value('CreationDateTo[1][not(@xsi:nil = "true")]', 'datetime')	as [CreationDateTo]
			,r.rootnode.value('Email[1][not(@xsi:nil = "true")]', 'varchar(256)')	as [Email]
			,r.rootnode.value('Locked[1][not(@xsi:nil = "true")]', 'bit')	as [Locked]
			,r.rootnode.value('Approved[1][not(@xsi:nil = "true")]', 'bit')	as [Approved]
			,r.rootnode.value('IsOnline[1][not(@xsi:nil = "true")]', 'bit')	as [IsOnline]
			,r.rootnode.value('Page[1][not(@xsi:nil = "true")]', 'int')	as [PageIndex]
			,r.rootnode.value('PageSize[1][not(@xsi:nil = "true")]', 'int')	as [PageSize]
			,r.rootnode.value('SortBy[1][not(@xsi:nil = "true")]', 'varchar(255)')	as [SortBy]
			,r.rootnode.value('SortAscending[1][not(@xsi:nil = "true")]', 'bit')	as [SortAscending]
	from			
			@filter.nodes('/DataFilterUserList') as r(rootnode)
	
	
	select 
			@dateInActive = DATEADD(minute,  -(@MinutesSinceLastInActive), @CurrentTimeUtc)
			, @pageSize = filter.PageSize
			, @pageIndex = filter.PageIndex
			, @rowstart = @pageSize * @pageIndex + 1
			, @rowend = @rowstart + @pageSize - 1
			, @sortType = 
						 case 
							when ISNULL(filter.SortType, '')  = '' then 'UserName'
							else filter.SortType
						 end
			, @sortAscending = 
						 case 
							when filter.SortAscending = 1 then ' asc'
							else ' desc'
						 end
	from	
			@tmpfilter filter
			
	insert into @tmpFilterRoles
	select
			roles.RoleId,
			r.rolesFilter.query('.').value('.', 'nvarchar(256)')	as [IsInRoleName]
	from 
			@filter.nodes('//DataFilterUserList/IsInRoleName/string') as r(rolesFilter),
			[dbo].[aspnet_Roles] roles
	where
			roles.LoweredRoleName = lower(r.rolesFilter.query('.').value('.', 'nvarchar(255)'))
	
	
	
	insert into @tmpResults
	select 
			u.UserName, 
			m.Email, 
			m.PasswordQuestion, 
			m.Comment, 
			m.IsApproved,
			m.CreateDate,
			m.LastLoginDate,
			u.LastActivityDate,
			m.LastPasswordChangedDate,
			u.UserId, 
			m.IsLockedOut,
			m.LastLockoutDate,
			row_number() over (
								order by 
									case when @sortType + @sortAscending = 'UserName desc' then u.UserName end desc ,
									case when @sortType + @sortAscending ='UserName asc' then u.UserName end asc ,

									case when @sortType + @sortAscending ='Email desc' then m.Email end desc ,
									case when @sortType + @sortAscending ='Email asc' then m.Email end asc ,

									case when @sortType + @sortAscending ='CreateDate desc' then m.CreateDate end desc ,
									case when @sortType + @sortAscending ='CreateDate asc' then m.CreateDate end asc ,

									case when @sortType + @sortAscending ='IsApproved desc' then m.IsApproved end desc ,
									case when @sortType + @sortAscending ='IsApproved asc' then m.IsApproved end asc ,

									case when @sortType + @sortAscending ='IsLockedOut desc' then m.IsLockedOut end desc ,
									case when @sortType + @sortAscending ='IsLockedOut asc' then m.IsLockedOut end asc 
								) as rownumber  
	from 
			[dbo].[aspnet_Applications] a(NOLOCK)
			,[dbo].[aspnet_Users] u (NOLOCK)
			,[dbo].[aspnet_Membership]	m (NOLOCK)
			,@tmpfilter as f
	where 
			u.ApplicationId = a.ApplicationId
			and a.LoweredApplicationName = LOWER(@applicationName)
			and u.UserId = m.UserId
			-- As far as this application uses Email & UserName as if they were "the same" field
			-- This lines are commented to improve performance
					--and (f.UserName is null or LOWER(u.UserName) like '%' + isnull(LOWER(f.UserName), LOWER(u.UserName)) + '%')
					--and  (f.Email is null or LOWER(m.LoweredEmail) = LOWER(f.Email))
			-- Comment this line in order to use Email & UserName as different fields
			and  (LOWER(m.LoweredEmail) like '%' + LOWER(f.UserName) + '%')
			and (f.Locked is null or m.IsLockedOut	 = f.Locked)	
			and (f.Approved is null or m.IsApproved	 = f.Approved)	
			and (f.CreationDateFrom is null or m.CreateDate between f.CreationDateFrom and isNull(f.CreationDateTo, getdate()))
			and (f.CreationDateTo is null or m.CreateDate between isNull(f.CreationDateFrom, cast(0 as datetime)) and f.CreationDateTo)
			and (f.IsOnline is null 
					or (f.IsOnline = 1 and u.LastActivityDate > @dateInActive) 
					or (f.IsOnline = 0 and u.LastActivityDate < @dateInActive)
				)
			and (select 
								count(ur.RoleId)
					from 
								@tmpFilterRoles ,
								[dbo].[aspnet_UsersInRoles] ur	
					where 
								ur.UserId = m.UserId
								and ur.RoleId = [@tmpFilterRoles].roleId
								) > 0

	select	@totalcount = max(rownumber) ,
			@totalpages= 
				case @totalcount % @pageSize 
					when 0 then @totalcount / @pageSize
					else	(@totalcount / @pageSize) + 1
				end 
	from @tmpResults
	
	select isnull(@totalcount,0) as totalcount, isnull(@totalpages,0) as totalpages
	
	select	* 
	from	@tmpResults
	where	rownumber between @rowstart and @rowend

GO

--declare @p8 xml
--set @p8=convert(xml,N'
--<DataFilterUserList xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
--  <UserName>gmail.com</UserName>
--  <CreationDateFrom xsi:nil="true"/>
--  <CreationDateTo xsi:nil="true"/>
--  <Approved xsi:nil="true"/>
--  <Locked xsi:nil="true"/>
--  <IsOnline xsi:nil="true"/>
--  <IsInRoleName>
--    <string>Administrator</string>
--    <string>Guest</string>
--  </IsInRoleName>
--  <Page>0</Page>
--  <PageSize>10</PageSize>
--  <SortBy/>
--  <SortAscending>true</SortAscending>
--  <IsClientVisible>false</IsClientVisible>
--</DataFilterUserList>
--')
--exec aspnet_Membership_FindUsersByFilter @applicationName=N'/',@MinutesSinceLastInActive=20,@CurrentTimeUtc='2013-04-10 09:22:22.343',@filter=@p8
