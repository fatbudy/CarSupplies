IF NOT EXISTS (SELECT 1 FROM SYSOBJECTS WHERE ID=OBJECT_ID('actionstepsqlitem'))
BEGIN
CREATE TABLE [dbo].actionstepsqlitem(
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ActionName] [nvarchar](50) NOT NULL,
	[StepSQL] [nvarchar](2000) NULL,
	[paramnumber] [tinyint] NULL,
 CONSTRAINT [PK_ActionStepSQL] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[ActionStepSQL] ADD  CONSTRAINT [DF_ActionStepSQL_param]  DEFAULT ((0)) FOR [paramnumber]
END

GO

create trigger comp_param_onActionStepSQL
on [dbo].[ActionStepSQLItem]
for insert,update
as
set nocount off

	declare @sql nvarchar(2000)
	declare @count tinyint
	declare @count_tmp int
	declare @id int
	select @id =id, @sql=StepSQL,@count=paramnumber from inserted
	select @count_tmp=dbo.get_param(@sql)
	if(@count_tmp<>@count)
	begin
		update dbo.actionstepsqlitem
			set paramnumber=@count_tmp
			where id=@id
	end
set nocount on

go

--           (<AKKey, nvarchar(50),>           ,<ActionName, nvarchar(50),>           ,<StepSQL, nvarchar(2000),>           ,<paramnumber, tinyint,>)
INSERT INTO actionstepsqlitem           ([ActionName]           ,[StepSQL])     VALUES
           ('' ,'')
GO
