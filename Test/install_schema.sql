use  [master]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IdENTIFIER ON
GO
SET ANSI_PADDING ON
GO
SET NOCOUNT ON
GO
DECLARE @SQL NVARCHAR(MAX), @TABLENAME VARCHAR(50)
SET @TABLENAME = 'GeocodeAccount'

IF OBJECT_Id(@TABLENAME, 'U') IS NULL

	BEGIN

		SET @SQL =

N'
CREATE TABLE [dbo].[GeocodeAccount](
    [GeocodeAccountId] [int] identity(1,1) NOT NULL,
	[DoctorId] [varchar](20) NOT NULL,
    [Address] [varchar](50) NOT NULL,
    [City] [varchar](50) NOT NULL,
    [Region] [char](2) NOT NULL,
    [Country] [varchar](50) NOT NULL,
    [PostalCode] [char](15) NOT NULL,
	[Active] [tinyint] NOT NULL,
	[SRId] [int] NULL,
	[LocationType] [varchar](30) NULL,
    [Latitude] [float] NULL,
    [Longitude] [float] NULL,
	[ServiceAPIEndPointUID] CHAR(36),
	[GeocodeUpdateDate] [datetime] NULL,
	[RecordInsertedLocalDTS] [datetime] NOT NULL CONSTRAINT CreateTS_AG_DF1 DEFAULT Convert(Char(10), CURRENT_TIMESTAMP, 120) ,
	[RecordUpdatedLocalDTS] [datetime] NULL,

	 CONSTRAINT [PK_GeocodeAccount] PRIMARY KEY CLUSTERED
(
    [GeocodeAccountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
CREATE INDEX [idx_GeocodeAccount_DoctorId] ON dbo.GeocodeAccount(DoctorId)
CREATE INDEX [idx_GeocodeAccount_GeocodeUpdateDate] ON dbo.[GeocodeAccount]([GeocodeUpdateDate])
ALTER TABLE [dbo].[GeocodeAccount] ADD  CONSTRAINT [DF_GeocodeAccount_Latitude]  DEFAULT ((0)) FOR [Latitude]
ALTER TABLE [dbo].[GeocodeAccount] ADD  CONSTRAINT [DF_GeocodeAccount_Longitude]  DEFAULT ((0)) FOR [Longitude]
'

EXEC sp_executesql @sql


END
