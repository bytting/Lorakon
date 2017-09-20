
/* LORAKON SQL SCRIPT */

/* Database */
CREATE DATABASE nrpa_lorakon 
GO

USE nrpa_lorakon 
GO

/* RingtestReportComments */
CREATE TABLE RingtestReportComments
(
RingtestReportID UNIQUEIDENTIFIER NOT NULL,
dateCreated DATETIME NOT NULL,
vchContactName NVARCHAR(96) NOT NULL,
textComment TEXT NOT NULL,
ID uniqueidentifier default NULL
) 
GO

/* UnitComments */
CREATE TABLE UnitComments
(
UnitID UNIQUEIDENTIFIER NOT NULL,
dateCreated DATETIME NOT NULL,
vchComment VARCHAR(256) NOT NULL,
ID UNIQUEIDENTIFIER DEFAULT NULL
) 
GO

/* Configuration */
CREATE TABLE Configuration
(
ID UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
vchName NVARCHAR(32) NOT NULL,
textStart TEXT,
textNews TEXT,
vchSectionManager VARCHAR(80),
vchAdminEmail VARCHAR(80)
) 
GO

CREATE PROC csp_insert_on_configuration_with_ID_name
	@ID UNIQUEIDENTIFIER,
	@name VARCHAR(32),
	@start TEXT,
	@news TEXT,
	@sectionManager VARCHAR(80),
	@ringtestAdminEmail VARCHAR(80)
AS 
	IF NOT EXISTS (SELECT ID FROM Configuration WHERE vchName = @name) 
	INSERT INTO Configuration VALUES(@ID, @name, @start, @news, @sectionManager, @ringtestAdminEmail)	
GO

CREATE PROC csp_update_all_on_configuration_where_name
	@name VARCHAR(32),
	@start TEXT,
	@news TEXT,
	@sectionManager VARCHAR(80),
	@ringtestAdminEmail VARCHAR(80)
AS 
	IF EXISTS (SELECT ID FROM Configuration WHERE vchName = @name) 
	UPDATE Configuration 
	SET textStart = @start, textNews = @news, vchSectionManager = @sectionManager, vchAdminEmail = @ringtestAdminEmail 
	WHERE vchName = @name
GO

CREATE PROC csp_select_all_on_configuration_where_name
	@name VARCHAR(32)	
AS 
	SELECT * FROM Configuration WHERE vchName = @name
GO

/* Contact */
CREATE TABLE Contact
(
ID UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
AccountID UNIQUEIDENTIFIER NOT NULL,
vchName NVARCHAR(96) NOT NULL,
vchEmail NVARCHAR(96) NOT NULL,
vchPhone NVARCHAR(16),
vchMobile NVARCHAR(16),
vchStatus NVARCHAR(16) NOT NULL
) 
GO

CREATE PROC csp_insert_on_contact_with_ID
	@ID UNIQUEIDENTIFIER,
	@accountID UNIQUEIDENTIFIER,
	@name VARCHAR(96),
	@email VARCHAR(96),
	@phone VARCHAR(16),
	@mobile VARCHAR(16),
	@status VARCHAR(16)		
AS 
	IF NOT EXISTS(SELECT ID FROM Contact WHERE ID = @ID)
	INSERT INTO Contact VALUES(@ID, @accountID, @name, @email, @phone, @mobile, @status)
GO

CREATE PROC csp_update_all_on_contact_where_ID
	@ID UNIQUEIDENTIFIER,
	@accountID UNIQUEIDENTIFIER,
	@name VARCHAR(96),
	@email VARCHAR(96),
	@phone VARCHAR(16),
	@mobile VARCHAR(16),
	@status VARCHAR(16)		
AS 
	IF EXISTS(SELECT ID FROM Contact WHERE ID = @ID)
	UPDATE Contact SET AccountID = @accountID, vchName = @name, vchEmail = @email, vchPhone = @phone, vchMobile = @mobile, vchStatus = @status WHERE ID = @ID
GO

CREATE PROC csp_select_all_on_contact_where_ID
	@ID UNIQUEIDENTIFIER	
AS 
	SELECT * FROM Contact WHERE ID = @ID
GO

CREATE PROC csp_link_on_contactcourse_with_courseID
	@contactID UNIQUEIDENTIFIER,
	@courseID UNIQUEIDENTIFIER
AS
	IF NOT EXISTS (SELECT ContactID FROM Contact_Course WHERE ContactID = @contactID AND CourseID = @courseID)
	INSERT INTO Contact_Course VALUES(@contactID, @courseID)
GO

CREATE PROC csp_unlink_on_contactcourse_with_courseID
	@contactID UNIQUEIDENTIFIER,
	@courseID UNIQUEIDENTIFIER	
AS 
	DELETE FROM Contact_Course WHERE ContactID = @contactID AND CourseID = @courseID
GO

CREATE PROC csp_select_ID_on_contactcourse_where_courseID
	@courseID UNIQUEIDENTIFIER	
AS 
	SELECT DISTINCT ContactID FROM Contact_Course WHERE CourseID = @courseID
GO

CREATE PROC csp_unlink_on_contactcourse_where_ID
	@ID UNIQUEIDENTIFIER	
AS 
	DELETE FROM Contact_Course WHERE ContactID = @ID
GO

/* Course */
CREATE TABLE Course
(
ID UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
vchTitle NVARCHAR(512) NOT NULL,
textDescription TEXT DEFAULT '',
textComment TEXT DEFAULT '',
bitCompleted BIT NOT NULL DEFAULT 0
) 
GO

CREATE PROC csp_insert_on_course_with_ID
	@ID UNIQUEIDENTIFIER,
	@title VARCHAR(512),
	@description TEXT,
	@comment TEXT,
	@completed BIT
AS 
	IF NOT EXISTS(SELECT ID FROM Course WHERE ID = @ID)
	INSERT INTO Course VALUES(@ID, @title, @description, @comment, @completed)	
GO

CREATE PROC csp_update_all_on_course_where_ID
	@ID UNIQUEIDENTIFIER,
	@title VARCHAR(512),
	@description TEXT,
	@comment TEXT,
	@completed BIT
AS 
	IF EXISTS(SELECT ID FROM Course WHERE ID = @ID)
	UPDATE Course SET vchTitle = @title, textDescription = @description, textComment = @comment, bitCompleted = @completed WHERE ID = @ID
GO

CREATE PROC csp_delete_on_course_where_ID
	@ID UNIQUEIDENTIFIER	
AS 	
	DELETE FROM Course WHERE ID = @ID	
GO

CREATE PROC csp_delete_links_on_contactcourse_where_courseID
	@courseID UNIQUEIDENTIFIER	
AS 
	DELETE FROM Contact_Course WHERE CourseID = @courseID
GO

CREATE PROC csp_select_all_on_course_where_ID
	@ID UNIQUEIDENTIFIER	
AS 
	SELECT * FROM Course WHERE ID = @ID
GO

CREATE TABLE Contact_Course
(
	ContactID UNIQUEIDENTIFIER NOT NULL,
	CourseID UNIQUEIDENTIFIER NOT NULL
)
GO

/* PendingAccount */
CREATE TABLE PendingAccount
(
ID UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
vchName NVARCHAR(128) NOT NULL,
vchContact NVARCHAR(96),
vchAddress NVARCHAR(256) NOT NULL,
vchPostbox NVARCHAR(32),
vchPostal NVARCHAR(96) NOT NULL,
vchEmail NVARCHAR(96) NOT NULL,
vchPhone NVARCHAR(16),
vchMobile NVARCHAR(16),
vchFax NVARCHAR(16),
vchWebsite NVARCHAR(256)
) 
GO

CREATE PROC csp_insert_on_pendingaccount_with_ID
	@ID UNIQUEIDENTIFIER,	
	@name NVARCHAR(128),
	@contact NVARCHAR(96),
	@address NVARCHAR(256),
	@postbox NVARCHAR(32),
	@postal NVARCHAR(96),
	@email NVARCHAR(96),
	@phone NVARCHAR(16),
	@mobile NVARCHAR(16),
	@fax NVARCHAR(16),
	@website NVARCHAR(256)	
AS 
	IF NOT EXISTS(SELECT ID FROM PendingAccount WHERE ID = @ID) 
	INSERT INTO PendingAccount VALUES (@ID, @name, @contact, @address, @postbox, @postal, @email, @phone, @mobile, @fax, @website)
GO

CREATE PROC csp_select_all_on_pendingaccount_where_ID
	@ID UNIQUEIDENTIFIER	
AS 
	SELECT * FROM PendingAccount WHERE ID = @ID
GO

CREATE PROC csp_delete_on_pendingaccount_by_ID
	@ID UNIQUEIDENTIFIER	
AS 
	DELETE FROM PendingAccount WHERE ID = @ID
GO

/* Account */
CREATE TABLE Account
(
ID UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
RingtestBoxID UNIQUEIDENTIFIER,
vchName NVARCHAR(128) NOT NULL,
vchContact NVARCHAR(96),
vchAddress NVARCHAR(256) NOT NULL,
vchPostbox NVARCHAR(32),
vchPostal NVARCHAR(96) NOT NULL,
vchEmail NVARCHAR(96) NOT NULL,
vchPhone NVARCHAR(16),
vchMobile NVARCHAR(16),
vchFax NVARCHAR(16),
vchWebsite NVARCHAR(256),
bitActive BIT DEFAULT 1,
textComment TEXT DEFAULT '',
intLastRegistrationYear INT NOT NULL DEFAULT 0,
intRingtestCount INT DEFAULT 0,
vchRingtestContact NVARCHAR(96) DEFAULT ''
) 
GO

CREATE PROC csp_insert_on_account_with_ID
	@ID UNIQUEIDENTIFIER,
	@ringtestBoxID UNIQUEIDENTIFIER,
	@name NVARCHAR(128),
	@contact NVARCHAR(96),
	@address NVARCHAR(256),
	@postbox NVARCHAR(32),
	@postal NVARCHAR(96),
	@email NVARCHAR(96),
	@phone NVARCHAR(16),
	@mobile NVARCHAR(16),
	@fax NVARCHAR(16),
	@website NVARCHAR(256),
	@active BIT,
	@comment TEXT,
	@lastRegistrationYear INT,
	@ringtestCount INT,
	@ringtestContact NVARCHAR(96)
AS 
	IF NOT EXISTS(SELECT vchName FROM Account WHERE ID = @ID) 
	INSERT INTO Account VALUES (
		@ID, @ringtestBoxID, @name, @contact, @address, @postbox, @postal, @email, @phone, @mobile, 
		@fax, @website, @active, @comment, @lastRegistrationYear, @ringtestCount, @ringtestContact)
GO

CREATE PROC csp_update_all_on_account_where_ID
	@ID UNIQUEIDENTIFIER,
	@ringtestBoxID UNIQUEIDENTIFIER,
	@name NVARCHAR(128),
	@contact NVARCHAR(96),
	@address NVARCHAR(256),
	@postbox NVARCHAR(32),
	@postal NVARCHAR(96),
	@email NVARCHAR(96),
	@phone NVARCHAR(16),
	@mobile NVARCHAR(16),
	@fax NVARCHAR(16),
	@website NVARCHAR(256),
	@active BIT,
	@comment TEXT,
	@lastRegistrationYear INT,
	@ringtestCount INT,
	@ringtestContact NVARCHAR(96)
AS 
	IF EXISTS(SELECT ID FROM Account WHERE ID = @ID) 
	UPDATE Account SET 
		RingtestBoxID = @ringtestBoxID, vchName = @name, vchContact = @contact, 
		vchAddress = @address, vchPostbox = @postbox, vchPostal = @postal, 
		vchEmail = @email, vchPhone = @phone, vchMobile = @mobile, 
		vchFax = @fax, vchWebsite = @website, bitActive = @active, 
		textComment = @comment, intLastRegistrationYear = @LastRegistrationYear, 
		intRingtestCount = @ringtestCount, vchRingtestContact = @ringtestContact 
		WHERE ID = @ID
GO

CREATE PROC csp_select_all_on_account_where_ID
	@ID UNIQUEIDENTIFIER	
AS 
	SELECT * FROM Account WHERE ID = @ID
GO

/* AccountAbility */
CREATE TABLE AccountAbility
(
ID UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
vchName NVARCHAR(128) NOT NULL
) 
GO

/* Account_AccountAbility */
CREATE TABLE Account_AccountAbility
(
AccountID UNIQUEIDENTIFIER NOT NULL,
AccountAbilityID UNIQUEIDENTIFIER NOT NULL
) 
GO

/* Ringtest */
CREATE TABLE Ringtest
(
ID UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
intYear INT NOT NULL,
dateStart DATETIME,
vchArchiveRef VARCHAR(80),
textComment TEXT,
bitFinished BIT DEFAULT 0
) 
GO

CREATE PROC csp_insert_on_ringtest_with_ID_year
	@ID UNIQUEIDENTIFIER,
	@year INT,
	@startDate DATETIME,
	@archiveRef VARCHAR(80),
	@comment TEXT,
	@finished BIT
AS 
	IF NOT EXISTS (SELECT ID FROM Ringtest WHERE ID = @ID OR intYear = @year) 
	INSERT INTO Ringtest VALUES(@ID, @year, @startDate, @archiveRef, @comment, @finished)
GO

CREATE PROC csp_update_all_on_ringtest_where_ID_year
	@year INT,
	@startDate DATETIME,
	@archiveRef VARCHAR(80),
	@comment TEXT,
	@finished BIT
AS 
	IF EXISTS (SELECT ID FROM Ringtest WHERE intYear = @year) 
	UPDATE Ringtest SET dateStart = @startDate, vchArchiveRef = @archiveRef, textComment = @comment, bitFinished = @finished WHERE intYear = @year
GO

CREATE PROC csp_select_all_on_ringtest_where_year
	@year INT
AS 
	SELECT * FROM Ringtest WHERE intYear = @year
GO

/* RingtestReport */
CREATE TABLE RingtestReport
(
ID UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
DetectorID UNIQUEIDENTIFIER NOT NULL,
RingtestID UNIQUEIDENTIFIER NOT NULL,
AccountID UNIQUEIDENTIFIER NOT NULL,
ContactID UNIQUEIDENTIFIER NOT NULL,
RingtestBoxID UNIQUEIDENTIFIER NOT NULL,
vchMCAType VARCHAR(80),
realBackground FLOAT,
intIntegralBackground INT,
intCountingBackground INT,
realGeometryFactor FLOAT,
realActivity FLOAT,
realActivityRef FLOAT,
realUncertainty FLOAT,
realAvgIntegralSample FLOAT,
realAvgLivetimeSample FLOAT,
dateRefDate DATETIME,
dateMeasureDate DATETIME,
realError FLOAT,
bitWantEvaluation BIT DEFAULT 0,
bitEvaluated BIT DEFAULT 0,
bitApproved BIT DEFAULT 0,
bitAnswerByEmail BIT DEFAULT 1,
bitAnswerSent BIT DEFAULT 0,
bitIsInspector1000 BIT DEFAULT 0,
textComment TEXT,
intAcceptableLimit INT DEFAULT 10,
realCalculatedUncertainty FLOAT
) 
GO

CREATE PROC csp_insert_on_ringtestreport_with_ID
	@ID UNIQUEIDENTIFIER,	
	@detectorID UNIQUEIDENTIFIER, 
	@ringtestID UNIQUEIDENTIFIER, 
	@accountID UNIQUEIDENTIFIER, 
	@contactID UNIQUEIDENTIFIER, 
	@ringtestBoxID UNIQUEIDENTIFIER, 
	@mcaType VARCHAR(80),
	@background FLOAT, 
	@integralBackground INT, 
	@countingBackground INT, 
	@geometryFactor FLOAT, 
	@activity FLOAT, 
	@activityRef FLOAT, 
	@uncertainty FLOAT, 
	@avgIntegralSample FLOAT, 
	@avgLivetimeSample FLOAT, 
	@refDate DATETIME, 
	@measureDate DATETIME, 
	@error FLOAT, 
	@wantEvaluation BIT, 
	@evaluated BIT, 
	@approved BIT, 
	@answerByEmail BIT, 
	@answerSent BIT, 
	@isInspector1000 BIT, 
	@comment TEXT,
	@acceptableLimit INT,
	@calculatedUncertainty FLOAT
AS 
	IF NOT EXISTS(SELECT ID FROM RingtestReport WHERE ID = @ID)
	INSERT INTO RingtestReport VALUES(
		@ID, @detectorID, @ringtestID, @accountID, @contactID, @ringtestBoxID, @mcaType, @background, @integralBackground, 
		@countingBackground, @geometryFactor, @activity, @activityRef, @uncertainty, @avgIntegralSample, 
		@avgLivetimeSample, @refDate, @measureDate, @error, @wantEvaluation, @evaluated, @approved, 
		@answerByEmail, @answerSent, @isInspector1000, @comment, @acceptableLimit, @calculatedUncertainty)	
GO

CREATE PROC csp_update_all_on_ringtestreport_where_ID
	@ID UNIQUEIDENTIFIER,	
	@detectorID UNIQUEIDENTIFIER, 
	@ringtestID UNIQUEIDENTIFIER, 
	@accountID UNIQUEIDENTIFIER, 
	@contactID UNIQUEIDENTIFIER, 
	@ringtestBoxID UNIQUEIDENTIFIER, 
	@mcaType VARCHAR(80),
	@background FLOAT, 
	@integralBackground INT, 
	@countingBackground INT, 
	@geometryFactor FLOAT, 
	@activity FLOAT, 
	@activityRef FLOAT, 
	@uncertainty FLOAT, 
	@avgIntegralSample FLOAT, 
	@avgLivetimeSample FLOAT, 
	@refDate DATETIME, 
	@measureDate DATETIME, 
	@error FLOAT, 
	@wantEvaluation BIT, 
	@evaluated BIT, 
	@approved BIT, 
	@answerByEmail BIT, 
	@answerSent BIT, 
	@isInspector1000 BIT, 
	@comment TEXT,
	@acceptableLimit INT,
	@calculatedUncertainty FLOAT
AS 
	IF EXISTS(SELECT ID FROM RingtestReport WHERE ID = @ID)
	UPDATE RingtestReport SET
		DetectorID = @detectorID, RingtestID = @ringtestID, AccountID = @accountID, ContactID = @contactID, RingtestBoxID = @ringtestBoxID, vchMCAType = @mcaType,
		realBackground = @background, intIntegralBackground = @integralBackground, intCountingBackground = @countingBackground, 
		realGeometryFactor = @geometryFactor, realActivity = @activity, realActivityRef = @activityRef, realUncertainty = @uncertainty, 
		realAvgIntegralSample = @avgIntegralSample, realAvgLivetimeSample = @avgLivetimeSample, dateRefDate = @refDate, dateMeasureDate = @measureDate, 
		realError = @error, bitWantEvaluation = @wantEvaluation, bitEvaluated = @evaluated, bitApproved = @approved, 
		bitAnswerByEmail = @answerByEmail, bitAnswerSent = @answerSent, bitIsInspector1000 = @isInspector1000, textComment = @comment, 
		intAcceptableLimit = @acceptableLimit, realCalculatedUncertainty = @calculatedUncertainty WHERE ID = @ID
GO

CREATE PROC csp_select_all_on_ringtestreport_where_ID
	@ID UNIQUEIDENTIFIER
AS 
	SELECT * FROM RingtestReport WHERE ID = @ID
GO

CREATE PROC csp_select_all_on_ringtestreport_where_accountID_detectorID_ringtestID
	@accountID UNIQUEIDENTIFIER,
	@detectorID UNIQUEIDENTIFIER,
	@ringtestID UNIQUEIDENTIFIER
AS 
	SELECT * FROM RingtestReport WHERE AccountID = @accountID AND DetectorID = @detectorID AND RingtestID = @ringtestID
GO

/* RingtestBox */
CREATE TABLE RingtestBox
(
ID UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
vchKNumber NVARCHAR(32) NOT NULL,
vchExternID NVARCHAR(32) NOT NULL,
dateRefDate DATETIME NOT NULL,
realRefValue FLOAT NOT NULL,
realUncertainty FLOAT NOT NULL,
realWeight FLOAT NOT NULL,
vchStatus NVARCHAR(16) NOT NULL,
textComment TEXT
) 
GO

CREATE PROC csp_insert_on_ringtestbox_with_ID_KNumber
	@ID UNIQUEIDENTIFIER,
	@kNumber NVARCHAR(32),
	@externID NVARCHAR(32),
	@refDate DATETIME,
	@refValue FLOAT,
	@uncertainty FLOAT,
	@weight FLOAT,
	@status NVARCHAR(16),
	@comment TEXT
AS 
	IF NOT EXISTS(SELECT ID FROM RingtestBox WHERE ID = @ID OR vchKNumber = @kNumber)
	INSERT INTO RingtestBox VALUES(@ID, @kNumber, @externID, @refDate, @refValue, @uncertainty, @weight, @status, @comment)	
GO

CREATE PROC csp_update_all_on_ringtestbox_where_ID
	@ID UNIQUEIDENTIFIER,
	@kNumber NVARCHAR(32),
	@externID NVARCHAR(32),
	@refDate DATETIME,
	@refValue FLOAT,
	@uncertainty FLOAT,	
	@weight FLOAT,
	@status NVARCHAR(16),
	@comment TEXT
AS 
	UPDATE RingtestBox 
	SET vchKNumber = @kNumber, vchExternID = @externID, dateRefDate = @refDate, realRefValue = @refValue, realUncertainty = @uncertainty, realWeight = @weight, vchStatus = @status, textComment = @comment 
	WHERE ID = @ID
GO

CREATE PROC csp_delete_on_ringtestbox_where_ID
	@ID UNIQUEIDENTIFIER
AS 
	DELETE FROM RingtestBox WHERE ID = @ID
GO

CREATE PROC csp_select_all_on_ringtestbox_where_ID
	@ID UNIQUEIDENTIFIER
AS 
	SELECT * FROM RingtestBox WHERE ID = @ID
GO

/* Device */
CREATE TABLE Device
(
ID UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
AccountID UNIQUEIDENTIFIER NOT NULL,
DeviceCategoryID UNIQUEIDENTIFIER NOT NULL,
DeviceTypeID UNIQUEIDENTIFIER NOT NULL,
vchSerialNumber NVARCHAR(64),
vchStatus NVARCHAR(16) NOT NULL,
vchOwnership NVARCHAR(16) NOT NULL,
textComment TEXT,
dateReceivedNew DATETIME
) 
GO

CREATE PROC csp_insert_on_device_with_ID
	@ID UNIQUEIDENTIFIER,
	@accountID UNIQUEIDENTIFIER,
	@categoryID UNIQUEIDENTIFIER,
	@typeID UNIQUEIDENTIFIER,
	@serialNumber NVARCHAR(64),
	@status NVARCHAR(16),
	@ownership NVARCHAR(16),	
	@comment TEXT,
	@receivedNew DATETIME
AS 
	IF NOT EXISTS(SELECT ID FROM Device WHERE ID = @ID)
	INSERT INTO Device VALUES(@ID, @accountID, @categoryID, @typeID, @serialnumber, @status, @ownership, @comment, @receivedNew)  
GO

CREATE PROC csp_update_all_on_device_where_ID
	@ID UNIQUEIDENTIFIER,
	@accountID UNIQUEIDENTIFIER,
	@categoryID UNIQUEIDENTIFIER,
	@typeID UNIQUEIDENTIFIER,
	@serialNumber NVARCHAR(64),
	@status NVARCHAR(16),
	@ownership NVARCHAR(16),	
	@comment TEXT,
	@receivedNew DATETIME
AS 
	IF EXISTS(SELECT ID FROM Device WHERE ID = @ID)
	UPDATE Device SET AccountID = @accountID, DeviceCategoryID = @categoryID, DeviceTypeID = @typeID, vchSerialNumber = @serialnumber, vchStatus = @status, 
		vchOwnership = @ownership, textComment = @comment, dateReceivedNew = @receivedNew WHERE ID = @ID	
GO

CREATE PROC csp_delete_on_device_where_ID
	@ID UNIQUEIDENTIFIER
AS 
	DELETE FROM Device WHERE ID = @ID
GO

CREATE PROC csp_select_all_on_device_where_ID
	@ID UNIQUEIDENTIFIER
AS 	
	SELECT * FROM Device WHERE ID = @ID
GO

CREATE PROC csp_select_all_on_device_where_categoryID_orderkey
	@categoryID UNIQUEIDENTIFIER,
	@orderKey VARCHAR(32)
AS 
	IF @orderKey IN ('vchSerialnumber', 'vchStatus', 'vchOwnership') 
		SELECT * FROM Device WHERE DeviceCategoryID = @categoryID
        ORDER BY CASE @orderKey
            WHEN 'vchSerialnumber' THEN vchSerialnumber
            WHEN 'vchStatus' THEN vchStatus 
			WHEN 'vchOwnership' THEN vchOwnership
        END 
	ELSE 
		SELECT * FROM Device WHERE DeviceCategoryID = @categoryID	
GO

CREATE PROC csp_select_all_on_device_where_typeID_orderkey
	@typeID UNIQUEIDENTIFIER,
	@orderKey VARCHAR(32)
AS 
	IF @orderKey IN ('vchSerialnumber', 'vchStatus', 'vchOwnership') 
		SELECT * FROM Device WHERE DeviceTypeID = @typeID
        ORDER BY CASE @orderKey
            WHEN 'vchSerialnumber' THEN vchSerialnumber
            WHEN 'vchStatus' THEN vchStatus 
			WHEN 'vchOwnership' THEN vchOwnership
        END 
	ELSE 
		SELECT * FROM Device WHERE DeviceTypeID = @typeID		
GO

CREATE PROC csp_select_identifiers_on_device_where_accountID_categoryID_status
	@accountID UNIQUEIDENTIFIER,
	@categoryID UNIQUEIDENTIFIER,
	@status VARCHAR(16)
AS 	
	SELECT ID, vchSerialNumber FROM Device WHERE AccountID = @accountID AND DeviceCategoryID = @categoryID AND vchStatus = @status
GO

CREATE PROC csp_select_all_on_device_where_accountID_categoryID_status
	@accountID UNIQUEIDENTIFIER,
	@categoryID UNIQUEIDENTIFIER,
	@status VARCHAR(16)
AS 	
	SELECT * FROM Device WHERE AccountID = @accountID AND DeviceCategoryID = @categoryID AND vchStatus = @status
GO

/* DeviceCategory */
CREATE TABLE DeviceCategory
(
ID UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
vchName NVARCHAR(64) NOT NULL,
bitSticky BIT NOT NULL
) 
GO

CREATE PROC csp_insert_on_devicecategory_with_ID_name
	@ID UNIQUEIDENTIFIER,
	@name VARCHAR(64),
	@sticky BIT
AS 
	IF NOT EXISTS (SELECT ID FROM DeviceCategory WHERE vchName = @name) 
	INSERT INTO DeviceCategory VALUES(@ID, @name, @sticky)
GO

CREATE PROC csp_update_all_on_devicecategory_where_ID
	@ID UNIQUEIDENTIFIER,
	@name VARCHAR(64),
	@sticky BIT
AS 
	IF EXISTS (SELECT ID FROM DeviceCategory WHERE ID = @ID) 
	UPDATE DeviceCategory SET vchName = @name, bitSticky = @sticky WHERE ID = @ID
GO

CREATE PROC csp_delete_on_devicecategory_where_ID
	@ID UNIQUEIDENTIFIER
AS 
	DELETE FROM DeviceCategory WHERE ID = @ID
GO

CREATE PROC csp_select_all_on_devicecategory_where_ID
	@ID UNIQUEIDENTIFIER
AS 
	IF EXISTS (SELECT ID FROM DeviceCategory WHERE ID = @ID) 
	SELECT * FROM DeviceCategory WHERE ID = @ID
GO

CREATE PROC csp_select_all_on_devicecategory_where_name
	@name VARCHAR(64)
AS 
	IF EXISTS (SELECT ID FROM DeviceCategory WHERE vchName = @name) 
	SELECT * FROM DeviceCategory WHERE vchName = @name
GO

/* DeviceType */
CREATE TABLE DeviceType
(
ID UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
DeviceCategoryID UNIQUEIDENTIFIER NOT NULL,
vchName NVARCHAR(64) NOT NULL,
bitSticky BIT NOT NULL
) 
GO

CREATE PROC csp_insert_on_devicetype_with_ID_categoryID_name
	@ID UNIQUEIDENTIFIER,
	@categoryID UNIQUEIDENTIFIER,
	@name VARCHAR(64),
	@sticky BIT
AS 
	IF NOT EXISTS (SELECT ID FROM DeviceType WHERE vchName = @name) 
	INSERT INTO DeviceType VALUES(@ID, @categoryID, @name, @sticky)
GO

CREATE PROC csp_update_all_on_devicetype_where_ID
	@ID UNIQUEIDENTIFIER,
	@categoryID UNIQUEIDENTIFIER,
	@name VARCHAR(64),
	@sticky BIT
AS 
	IF EXISTS (SELECT ID FROM DeviceType WHERE ID = @ID) 
	UPDATE DeviceType SET DeviceCategoryID = @categoryID, vchName = @name, bitSticky = @sticky WHERE ID = @ID
GO

CREATE PROC csp_delete_on_devicetype_where_ID
	@ID UNIQUEIDENTIFIER
AS 
	DELETE FROM DeviceType WHERE ID = @ID
GO

CREATE PROC csp_select_all_on_devicetype_where_ID
	@ID UNIQUEIDENTIFIER
AS 
	SELECT * FROM DeviceType WHERE ID = @ID
GO

/* SequenceSerial */
CREATE TABLE SequenceSerial
(
	val INT NOT NULL IDENTITY(1, 1)
) 
GO

CREATE PROC csp_GetSerialnumber 
	@val AS INT OUTPUT
AS 
BEGIN TRAN 
  SAVE TRAN s1 
  INSERT INTO SequenceSerial DEFAULT VALUES
  ROLLBACK TRAN s1 
  SET @val = scope_identity() 
COMMIT TRAN
GO

/* Add required categories and types */
DECLARE @DetectorID UNIQUEIDENTIFIER
DECLARE @MCAID UNIQUEIDENTIFIER
SET @DetectorID = newId()
SET @MCAID = newId()

IF NOT EXISTS (SELECT ID FROM DeviceCategory WHERE vchName = 'Detektor') 
INSERT INTO DeviceCategory VALUES(@DetectorID, 'Detektor', 1)

IF NOT EXISTS (SELECT ID FROM DeviceCategory WHERE vchName = 'MCA') 
INSERT INTO DeviceCategory VALUES(@MCAID, 'MCA', 1)

IF NOT EXISTS (SELECT ID FROM DeviceType WHERE vchName = 'Inspector 1000') 
INSERT INTO DeviceType VALUES(newId(), @MCAID, 'Inspector 1000', 1)

IF NOT EXISTS (SELECT ID FROM DeviceType WHERE vchName = 'Serie 10') 
INSERT INTO DeviceType VALUES(newId(), @MCAID, 'Serie 10', 1)

IF NOT EXISTS (SELECT ID FROM DeviceType WHERE vchName = 'Serie 10+') 
INSERT INTO DeviceType VALUES(newId(), @MCAID, 'Serie 10+', 1)