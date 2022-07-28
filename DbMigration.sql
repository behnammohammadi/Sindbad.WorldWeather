USE master
GO

IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'Climate')
BEGIN
	CREATE DATABASE Climate
END
GO

USE [Climate]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE TABLE [dbo].[Weather](
	[City] [nvarchar](128) NOT NULL,
	[CalculatedOn] bigint NOT NULL,
	[Summary] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](1024) NOT NULL,
	[Temp] decimal(20,3) NULL,
	[TempMin] decimal(20,3) NULL,
	[TempMax] decimal(20,3) NULL,
	[FeelsLike] decimal(20,3) NULL,
	[Visibility] decimal(20,3) NULL,
	[WindSpeed] decimal(20,3) NULL,
	[WindDegree] decimal(20,3) NULL,
	[Cloudiness] decimal(20,3) NULL);
GO

CREATE NONCLUSTERED INDEX NIX_Weather_City ON [dbo].[Weather] ([City] asc)
CREATE NONCLUSTERED INDEX NIX_Weather_CalculatedOn ON [dbo].[Weather] ([CalculatedOn] desc)

GO

