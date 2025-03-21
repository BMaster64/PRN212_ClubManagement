-- =============================
-- CREATE TABLES
-- =============================

-- Create Club Table
CREATE TABLE Club (
    ClubId INT IDENTITY(1,1) PRIMARY KEY,
    ClubName NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX)
);

-- Create User Table
CREATE TABLE [User] (
    StudentId NVARCHAR(50) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Username NVARCHAR(100) NOT NULL UNIQUE,
    Password NVARCHAR(100) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    RoleId INT NOT NULL,
    ClubId INT NOT NULL,

    CONSTRAINT FK_User_Club FOREIGN KEY (ClubId) REFERENCES Club(ClubId)
);
ALTER TABLE [User]  
ADD Status BIT NOT NULL DEFAULT 1;  

-- Create Event Table
CREATE TABLE Event (
    EventId INT IDENTITY(1,1) PRIMARY KEY,
    EventName NVARCHAR(255) NOT NULL,
    StartTime DATETIME NOT NULL,
    EndTime DATETIME NOT NULL,
    Location NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX),
    Status NVARCHAR(50) NOT NULL,
    ClubId INT NOT NULL,

    CONSTRAINT FK_Event_Club FOREIGN KEY (ClubId) REFERENCES Club(ClubId)
);

-- Create Event Registration Table
CREATE TABLE EventRegistration (
    EventRegistrationId INT IDENTITY(1,1) PRIMARY KEY,
    EventId INT NOT NULL,
    StudentId NVARCHAR(50) NOT NULL,
    Status NVARCHAR(50) NOT NULL,

    CONSTRAINT FK_EventRegistration_Event FOREIGN KEY (EventId) REFERENCES Event(EventId),
    CONSTRAINT FK_EventRegistration_User FOREIGN KEY (StudentId) REFERENCES [User](StudentId)
);

-- Create Participation Classification Table
CREATE TABLE ParticipationClassification (
    ParticipationClassificationId INT IDENTITY(1,1) PRIMARY KEY,
    StudentId NVARCHAR(50) NOT NULL,
    Semester NVARCHAR(20) NOT NULL,
    Year INT NOT NULL,
    TotalEventsParticipated INT NOT NULL,
    TotalEvents INT NOT NULL,

    CONSTRAINT FK_ParticipationClassification_User FOREIGN KEY (StudentId) REFERENCES [User](StudentId)
);

-- Create Notification Table
CREATE TABLE Notification (
    NotificationId INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(255) NOT NULL,
    Content NVARCHAR(MAX) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    SenderId NVARCHAR(50) NOT NULL,

    CONSTRAINT FK_Notification_Sender FOREIGN KEY (SenderId) REFERENCES [User](StudentId)
);

-- Create User Notification Table
CREATE TABLE UserNotification (
    UserNotificationId INT IDENTITY(1,1) PRIMARY KEY,
    NotificationId INT NOT NULL,
    StudentId NVARCHAR(50) NOT NULL,
    IsRead BIT DEFAULT 0,
    ClubId INT NOT NULL,

    CONSTRAINT FK_UserNotification_Notification FOREIGN KEY (NotificationId) REFERENCES Notification(NotificationId),
    CONSTRAINT FK_UserNotification_User FOREIGN KEY (StudentId) REFERENCES [User](StudentId)
);

-- Create Report Table
CREATE TABLE Report (
    ReportId INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(255) NOT NULL,
    Content NVARCHAR(MAX) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    Status NVARCHAR(50) DEFAULT 'Pending',
    SenderId NVARCHAR(50) NOT NULL,
    ReceiverId NVARCHAR(50) NOT NULL,

    CONSTRAINT FK_Report_Sender FOREIGN KEY (SenderId) REFERENCES [User](StudentId),
    CONSTRAINT FK_Report_Receiver FOREIGN KEY (ReceiverId) REFERENCES [User](StudentId)
);
ALTER TABLE Report
DROP CONSTRAINT FK_Report_Receiver;
ALTER TABLE Report
DROP COLUMN ReceiverId;
ALTER TABLE Report
ADD ClubId INT;
ALTER TABLE Report
ADD CONSTRAINT FK_Report_Club FOREIGN KEY (ClubId) REFERENCES Club(ClubId);
-- Create User Report Table
CREATE TABLE UserReport (
    UserReportId INT IDENTITY(1,1) PRIMARY KEY,
    ReportId INT NOT NULL,
    StudentId NVARCHAR(50) NOT NULL,
    IsRead BIT DEFAULT 0,
    ClubId INT NOT NULL,

    CONSTRAINT FK_UserReport_Report FOREIGN KEY (ReportId) REFERENCES Report(ReportId),
    CONSTRAINT FK_UserReport_User FOREIGN KEY (StudentId) REFERENCES [User](StudentId)
);
-- =============================
-- INSERT SAMPLE DATA
-- =============================

-- Insert Club Data
INSERT INTO Club (ClubName, Description)
VALUES 
('Chess Club', 'Strategic board games'),
('Coding Club', 'Programming and software development');

-- Insert User Data
INSERT INTO [User] (StudentId, FullName, Username, Password, RoleId, ClubId)
VALUES 
('S001', 'John Doe', 'johndoe', 'password123', 1, 1),
('S002', 'Jane Smith', 'janesmith', 'password123', 2, 1),
('S003', 'Emily Johnson', 'emilyj', 'password123', 3, 2);

-- Insert Event Data
INSERT INTO Event (EventName, StartTime, EndTime, Location, Description, Status, ClubId)
VALUES 
('Chess Tournament', '2025-05-10 09:00:00', '2025-05-10 17:00:00', 'Main Hall', 'Annual chess competition', 'Scheduled', 1);

-- Insert Event Registration
INSERT INTO EventRegistration (EventId, StudentId, Status)
VALUES 
(1, 'S001', 'Registered'),
(1, 'S002', 'Attended');

-- Insert Notification
INSERT INTO Notification (Title, Content, SenderId)
VALUES 
('Event Reminder', 'The chess tournament is this Saturday!', 'S001');

-- Insert User Notification
INSERT INTO UserNotification (NotificationId, StudentId, IsRead, ClubId)
VALUES 
(1, 'S002', 0, 1);

-- Insert Report
INSERT INTO Report (Title, Content, SenderId, ReceiverId)
VALUES 
('Monthly Chess Report', 'Chess club participation increased by 10% this month.', 'S001', 'S002');

-- =============================
-- QUERIES
-- =============================

-- 1. Get All Events for a Club
SELECT * 
FROM Event 
WHERE ClubId = 1;

-- 2. Get All Event Registrations
SELECT 
    E.EventName, 
    R.Status, 
    U.FullName
FROM 
    EventRegistration R
JOIN 
    Event E ON R.EventId = E.EventId
JOIN 
    [User] U ON R.StudentId = U.StudentId;

-- 3. Get Pending Reports
SELECT 
    R.Title, 
    R.Content, 
    S.FullName AS Sender, 
    RCV.FullName AS Receiver
FROM 
    Report R
JOIN 
    [User] S ON R.SenderId = S.StudentId
JOIN 
    [User] RCV ON R.ReceiverId = RCV.StudentId
WHERE 
    R.Status = 'Pending';

-- 4. Get Notifications for a Club
SELECT 
    N.Title, 
    N.Content, 
    U.FullName AS Sender
FROM 
    Notification N
JOIN 
    [User] U ON N.SenderId = U.StudentId;

-- 5. Get Participation Statistics
SELECT 
    PC.StudentId, 
    U.FullName, 
    PC.Semester, 
    PC.Year, 
    PC.TotalEventsParticipated, 
    PC.TotalEvents,
    (PC.TotalEventsParticipated * 1.0 / NULLIF(PC.TotalEvents, 0)) * 100 AS ParticipationRate
FROM 
    ParticipationClassification PC
JOIN 
    [User] U ON PC.StudentId = U.StudentId;

-- 6. Approve Report
UPDATE Report
SET Status = 'Approved'
WHERE ReportId = 1;

-- 7. Delete Report
DELETE FROM Report
WHERE ReportId = 1;

-- 8. Get Users in a Club
SELECT 
    U.StudentId,
    U.FullName,
    U.Username
FROM 
    [User] U
WHERE 
    U.ClubId = 1;

-- 9. Get Club Participation Rate
SELECT 
    U.FullName,
    (PC.TotalEventsParticipated * 1.0 / NULLIF(PC.TotalEvents, 0)) * 100 AS ParticipationRate
FROM 
    ParticipationClassification PC
JOIN 
    [User] U ON PC.StudentId = U.StudentId
WHERE 
    U.ClubId = 1;

-- 10. Get All Reports Sent by a User
SELECT 
    R.Title,
    R.Content,
    R.CreatedAt,
    R.Status
FROM 
    Report R
WHERE 
    R.SenderId = 'S001';

-- =============================
-- END OF FILE
-- =============================
