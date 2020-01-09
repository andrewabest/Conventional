CREATE TABLE [dbo].[Record](
	[RecordThingo] [int] NOT NULL,
    CONSTRAINT CHK_RecordThingo_Record CHECK (RecordThingo > 0))