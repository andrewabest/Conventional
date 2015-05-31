CREATE TABLE [dbo].[TableWithClusteredIndex] (
	[Id] [int] IDENTITY(1,1) NOT NULL,
	CONSTRAINT [PK_TableWithClusteredIndex] PRIMARY KEY CLUSTERED(
		[Id] ASC
	)
)