use  [master]

GO

SET NOCOUNT ON;

IF OBJECT_ID('tempdb..#tmp') > 0 DROP TABLE #tmp
CREATE TABLE #tmp
(
	DoctorID VARCHAR(20)
)

INSERT INTO #tmp VALUES ('15-01')
INSERT INTO #tmp VALUES ('15-02')
INSERT INTO #tmp VALUES ('15-03')
INSERT INTO #tmp VALUES ('15-04')
INSERT INTO #tmp VALUES ('15-05')
INSERT INTO #tmp VALUES ('15-06')
INSERT INTO #tmp VALUES ('15-07')
INSERT INTO #tmp VALUES ('15-08')
INSERT INTO #tmp VALUES ('15-09')
INSERT INTO #tmp VALUES ('15-10')

SELECT 
*
FROM [dbo].[GeocodeAccount] ga WITH (NOLOCK)
INNER JOIN #tmp t ON t.DoctorID = ga.DoctorID

WHERE 
Latitude is NULL


  /*
  -reset values back to zero
  UPDATE  ga
  SET Latitude = NULL, Longitude = null, LocationType=null, ServiceAPIEndPointUID=null, SRID = null
  FROM [GDLMarketingCampaign].[dbo].[GeocodeAccount]  ga WITH (NOLOCK)
  INNER JOIN #tmp t ON t.DoctorID = ga.DoctorID
  */
