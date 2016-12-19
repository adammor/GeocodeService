use  [master]

GO

SET NOCOUNT ON;

IF OBJECT_ID('tempdb..#tmp') IS NOT NULL	
DROP TABLE #tmp

CREATE TABLE #tmp(
		[DoctorID] [varchar](20), [Address] [varchar](50), [City] [varchar](50),[Region] [char](2),[PostalCode] [char](10),[Active] [tinyint],
		[Latitude] [float],[Longitude] [float]
	)

INSERT INTO #tmp VALUES('15-01','1 Hoag Dr','Newport Beach','CA','92663','1',NULL,NULL)
INSERT INTO #tmp VALUES('15-02','16200 Sand Canyon Ave','Irvine','CA','92618','1',NULL,NULL)
INSERT INTO #tmp VALUES('15-03','510 Superior Ave','Newport Beach','CA','92663','1',NULL,NULL)
INSERT INTO #tmp VALUES('15-04','2560 Bryan Ave','Tustin','CA','92780','1',NULL,NULL)
INSERT INTO #tmp VALUES('15-05','19582 Beach Blvd,','Huntington Beach','CA','92648','1',NULL,NULL)
INSERT INTO #tmp VALUES('15-06',' 4900 Barranca Pkwy','Irvine','CA','92604','1',NULL,NULL)
INSERT INTO #tmp VALUES('15-07','1190 Baker St','Costa Mesa','CA','92626','1',NULL,NULL)
INSERT INTO #tmp VALUES('15-08','16405 Sand Canyon Ave','Irvine','CA','92618','1',NULL,NULL)
INSERT INTO #tmp VALUES('15-09','6352 Irvine Blvd','Irvine','CA','92620','1',NULL,NULL)
INSERT INTO #tmp VALUES('15-10','1601 Dove St','Newport Beach','CA','92660','1',NULL,NULL)
INSERT INTO #tmp VALUES('15-11','1000 Bristol St N','Newport Beach','CA','92660','1',NULL,NULL)
INSERT INTO #tmp VALUES('15-12','4631 Teller Ave','Newport Beach','CA','92660','1',NULL,NULL)
INSERT INTO #tmp VALUES('15-13','1202 Bristol St','Costa Mesa','CA','92626','1',NULL,NULL)
INSERT INTO #tmp VALUES('15-14','1 Medical Plaza Dr','Irvine','CA','92697','1',NULL,NULL)
INSERT INTO #tmp VALUES('15-15','1501 E 16th St','Newport Beach','CA','92663','1',NULL,NULL)
INSERT INTO #tmp VALUES('15-16','330 Placentia Ave','Newport Beach','CA','92663','1',NULL,NULL)
INSERT INTO #tmp VALUES('15-17','351 Hospital Rd ','Newport Beach','CA','92663','1',NULL,NULL)
INSERT INTO #tmp VALUES('15-18','3900 Pacific Coast Hwy','Newport Beach','CA','92663','1',NULL,NULL)
INSERT INTO #tmp VALUES('15-19','250 E Yale Loop','Irvine','CA','92604','1',NULL,NULL)
INSERT INTO #tmp VALUES('15-20','520 Superior Ave','Newport Beach','CA','92663','1',NULL,NULL)

--add any new doctor addresses
	INSERT INTO [dbo].[GeocodeAccount]
			   (
				  [DoctorID],[Address],[City],[Region],[Country],[PostalCode],[Active]
				, [SRID], [LocationType], [Latitude], [Longitude], [ServiceAPIEndPointUID], [GeocodeUpdateDate]
				, [RecordInsertedLocalDTS]
				)
	SELECT 
		t.[DoctorID], t.[Address], t.[City], t.[Region], [Country]='USA', t.[PostalCode], t.[Active]
		, [SRID]='4326', [LocationType]='(unknown)', t.[Latitude], t.[Longitude], [ServiceAPIEndPointUID]=null, [GeocodeUpdateDate] = '2016-01-01 00:00:00.000'
		, [RecordInsertedLocalDTS] = '2016-01-01 00:00:00.000'
	FROM 
		#tmp AS t
	LEFT OUTER JOIN 
		dbo.[GeocodeAccount] AS ga ON t.DoctorID = ga.DoctorID 
	WHERE 
		ga.DoctorID IS NULL


		