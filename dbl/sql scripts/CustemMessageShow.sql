IF NOT EXISTS (SELECT 1 FROM SYSOBJECTS WHERE id=OBJECT_ID('CustomMessageList'))
BEGIN
CREATE TABLE [dbo].[CustomMessageList](
	[id] [int] NOT NULL,
	[caption] [nvarchar](50) NULL,
	[text] [nvarchar](255) NOT NULL,
	[icon] [int] NULL,
	[button] [tinyint] NULL,
 CONSTRAINT [PK_CustomMessageList] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

--插入数据
--icon
        --0,   消息框未包含符号。
        --16,     该消息框包含一个符号，该符号是由一个红色背景的圆圈及其中的白色 X 组成的。
        --32,     该消息框包含一个符号，该符号是由一个圆圈和其中的一个问号组成的。不再建议使用问号消息图标，原因是该图标无法清楚地表示特定类型的消息，并且问号形式的消息表述可应用于任何消息类型。此外，用户还可能将问号消息符号与帮助信息混淆。因此，请不要在消息框中使用此问号消息符号。系统继续支持此符号只是为了向后兼容。
        --48,     该消息框包含一个符号，该符号是由一个黄色背景的三角形及其中的一个感叹号组成的。
        --64,     该消息框包含一个符号，该符号是由一个圆圈及其中的小写字母 i 组成的
--buttion
        --OK = 0,消息框包含“确定”按钮。
        --OKCancel = 1,消息框包含“确定”和“取消”按钮。
        --AbortRetryIgnore = 2, 消息框包含“中止”、“重试”和“忽略”按钮。
        --YesNoCancel = 3,消息框包含“是”、“否”和“取消”按钮。
        --YesNo = 4,消息框包含“是”和“否”按钮。
        --RetryCancel = 5,消息框包含“重试”和“取消”按钮。

--           <caption, nvarchar(50),>           ,<text, nvarchar(255),>           ,<icon, int,>           ,<buttion, tinyint,>)
--																										0,16,32,48,64			0,1,2,3,4,5,
INSERT INTO [CustomMessageList]
           ([caption]           ,[text]           ,[icon]           ,[button])     VALUES           (
           ''           ,''	           ,''           ,'')
GO

