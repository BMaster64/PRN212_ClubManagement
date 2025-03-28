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
-- Create ChatChannel Table
CREATE TABLE ChatChannel (
    ChannelId INT IDENTITY(1,1) PRIMARY KEY,
    ChannelName NVARCHAR(100) NOT NULL,
    ClubId INT NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    
    CONSTRAINT FK_ChatChannel_Club FOREIGN KEY (ClubId) REFERENCES Club(ClubId)
);

-- Create ChatMessage Table
CREATE TABLE ChatMessage (
    MessageId INT IDENTITY(1,1) PRIMARY KEY,
    Content NVARCHAR(MAX) NOT NULL,
    SenderId NVARCHAR(50) NOT NULL,
    ChannelId INT NOT NULL,
    SentAt DATETIME DEFAULT GETDATE(),
    
    CONSTRAINT FK_ChatMessage_User FOREIGN KEY (SenderId) REFERENCES [User](StudentId),
    CONSTRAINT FK_ChatMessage_Channel FOREIGN KEY (ChannelId) REFERENCES ChatChannel(ChannelId)
);

-- Create ClubRegistrationRequest Table
CREATE TABLE ClubRegistrationRequest (
    RequestId INT PRIMARY KEY IDENTITY(1,1),
    ClubName NVARCHAR(MAX) NOT NULL,
    PresidentStudentId NVARCHAR(MAX) NOT NULL,
    PresidentFullName NVARCHAR(MAX) NOT NULL,
    PresidentUsername NVARCHAR(MAX) NOT NULL,
    RequestedAt DATETIME NOT NULL DEFAULT GETDATE(),
    Status INT NOT NULL
);

-- Insert a default channel for each club
INSERT INTO ChatChannel (ChannelName, ClubId)
SELECT CONCAT(ClubName, ' General'), ClubId FROM Club;
-- =============================
-- INSERT SAMPLE DATA
-- =============================

-- Insert Clubs
INSERT INTO Club (ClubName, Description)
VALUES 
('Computer Science Club', 'A club for tech enthusiasts and programmers'),
('Debate Society', 'A forum for intellectual discourse and public speaking'),
('Environmental Action Group', 'Promoting sustainability and environmental awareness');

-- Insert Users
INSERT INTO [User] (StudentId, FullName, Username, Password, RoleId, ClubId)
VALUES 
('CS001', 'Alex Johnson', 'alex.president', '123', 1, 1),
('CS002', 'Emma Rodriguez', 'emma.vp', '123', 2, 1),
('CS003', 'Michael Chen', 'michael.secretary', '123', 3, 1),
('CS004', 'Sarah Kim', 'sarah.member', '123', 4, 1),
('DS001', 'Ryan Thompson', 'ryan.president', '123', 1, 2),
('DS002', 'Olivia Martinez', 'olivia.vp', '123', 2, 2),
('DS003', 'Daniel Park', 'daniel.secretary', '123', 3, 2),
('DS004', 'Isabella Wong', 'isabella.member', '123', 4, 2),
('EA001', 'Sophia Lee', 'sophia.president', '123', 1, 3),
('EA002', 'Ethan Kumar', 'ethan.vp', '123', 2, 3),
('EA003', 'Ava Patel', 'ava.secretary', '123', 3, 3),
('EA004', 'Noah Garcia', 'noah.member', '123', 4, 3);

-- Sample Events for each Club
INSERT INTO Event (EventName, StartTime, EndTime, Location, Description, Status, ClubId)
VALUES 
('Hackathon 2024', '2024-06-15 09:00:00', '2024-06-16 18:00:00', 'University Tech Center', 'Annual programming competition', 'Upcoming', 1),
('Inter-College Debate Competition', '2024-07-20 14:00:00', '2024-07-21 20:00:00', 'Main Auditorium', 'Regional debate tournament', 'Upcoming', 2),
('Beach Cleanup Drive', '2024-08-10 07:00:00', '2024-08-10 12:00:00', 'City Beach', 'Community environmental initiative', 'Upcoming', 3);
