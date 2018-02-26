create table Users
(
ID int primary key IDENTITY(1,1) not null,
PublicKey uniqueidentifier,

IsCompany bit not null,

Name nvarchar(20) not null,
SurName nvarchar(20) not null,

Email nvarchar(50) not null unique,
PasswordHash nvarchar(50) not null,
Phone1 nvarchar(100),
Phone2 nvarchar(100),
Phone3 nvarchar(100),
Skype nvarchar(100),

Place nvarchar(50),

Ballance nvarchar(20),
Valuta nvarchar(20),

AccountCreated datetime not null,
LastLogIn datetime,
UserLogoPath nvarchar(300),
Gender bit,
Birthday datetime,

Rating int not null,
Honors nvarchar(MAX),

SubscribeToGroups nvarchar(MAX),
IsEnabled bit not null,
ReceiveNews bit not null,
Lang nvarchar(50) not null,
)


create table Mails
(
ID int primary key IDENTITY(1,1) not null,
Recipient nvarchar(50) not null,
EmailSubject nvarchar(50), 
Datetime datetime not null,
)


create table Tasks
(
ID int primary key IDENTITY(1,1) not null,
PublicKey uniqueidentifier not null,

Customer int not null,
Worker int,

Name nvarchar(100) not null,
Description nvarchar(500) not null,

TopicalTo datetime not null,
Created datetime not null,
Closed datetime,

Groop nvarchar(100) not null,
SubGroop nvarchar(100) not null,
Price int not null,

ProccessStatus nvarchar(50) not null,
)


create table Events
(
ID int primary key IDENTITY(1,1) not null,
Name nvarchar(50) not null,
Decrtiption nvarchar(50) not null,
UserPk uniqueidentifier not null,
Datetime datetime not null,
)


create table Chats
(
ID int primary key IDENTITY(1,1) not null,
PublicKey uniqueidentifier not null,
BelongsToUserA int foreign key references Users (ID),
BelongsToUserB int foreign key references Users (ID),
LastAction datetime not null,
ExistNewMessage bit not null,
)


create table ChatMessages
(
ID int primary key IDENTITY(1,1) not null,
PublicKey uniqueidentifier not null,
BelongToChat int foreign key references Chats (ID),
IsNew bit not null,
Created datetime not null,
Message nvarchar(max) not null,
Attachments nvarchar(max),
)


create table CommonNews
(
ID int primary key IDENTITY(1,1) not null,
PublicKey uniqueidentifier not null,
Html nvarchar(max),
Created datetime not null,
)


create table CommonAdvertising
(
ID int primary key IDENTITY(1,1) not null,
PublicKey uniqueidentifier not null,
CompanyName nvarchar(100) not null,
CompanyRate int not null,
Expired datetime not null,
ImagePath nvarchar(300) not null,
FirstData nvarchar(50),
SecondData nvarchar(50),
Icon nvarchar(50) not null,
)

create table TaskAttachments
(
ID int primary key IDENTITY(1,1) not null,
PublicKey uniqueidentifier not null,
BelongsToTask int foreign key references Tasks (ID),
Url nvarchar(300) not null,
Extention nvarchar(5),
FileTitle nvarchar(100) not null,
)


create table TaskBids
(
ID int primary key IDENTITY(1,1) not null,
PublicKey uniqueidentifier not null,
BelongsToTask int foreign key references Tasks (ID),
BelongsToWorker int foreign key references Users (ID),
Message nvarchar(max) not null,
Price int not null,
Created datetime not null,
Denied bit not null
)


create table TaskPreWorkers
(
ID int primary key IDENTITY(1,1) not null,
PublicKey uniqueidentifier not null,
BelongsToTask int foreign key references Tasks (ID),
BelongsToWorker int foreign key references Users (ID),
Message nvarchar(max),
Approved datetime not null,
)



create table TaskOffers
(
ID int primary key IDENTITY(1,1) not null,
PublicKey uniqueidentifier not null,
BelongsToTask int foreign key references Tasks (ID),
BelongsToWorker int foreign key references Users (ID),
Message nvarchar(max) not null,
Created datetime not null,
)



create table Notifications
(
ID int primary key IDENTITY(1,1) not null,
PublicKey uniqueidentifier not null,
BelongsToTask int foreign key references Tasks (ID),
BelongsToUser int foreign key references Users (ID),
BelongsToFromUser int foreign key references Users (ID),
Reviewed bit not null,
NotificationType varchar(20) not null,
Created datetime not null
)





create table Portfolios
(
ID int primary key IDENTITY(1,1) not null,
PublicKey uniqueidentifier,
BelongsToUser int foreign key references Users (ID),
Title nvarchar(200),
Description nvarchar(MAX),
)


create table PortfolioJobs
(
ID int primary key IDENTITY(1,1) not null,
PublicKey uniqueidentifier,
BelongsToPortfolio int foreign key references Portfolios (ID),
Title nvarchar(200),
Description nvarchar(MAX),
)


create table PortfolioJobImages
(
ID int primary key IDENTITY(1,1) not null,
PublicKey uniqueidentifier,
BelongsToPortfolioJob int foreign key references PortfolioJobs (ID),
Small nvarchar(MAX),
Large nvarchar(MAX),
)


create table TempUserData
(
ID int primary key IDENTITY(1,1) not null,
PublicKey uniqueidentifier,
Name nvarchar(20),
SurName nvarchar(20),
UserLogoPath nvarchar(300),
BelongsToUser int foreign key references Users (ID),
)


create table RialtoState
(
ID int primary key IDENTITY(1,1) not null,
TotalAveCount float not null,
AveRate float not null,
ChartValue float not null,
)


create table RialtoAction
(
ID int primary key IDENTITY(1,1) not null,
BidTime datetime not null,
Bid float not null,
CurrentChartValue float not null,
BelongsToUser int foreign key references Users (ID),
)


create table RialtoUsersStates
(
ID int primary key IDENTITY(1,1) not null,
Ballance float not null,
BelongsToUser int foreign key references Users (ID),
)


create table RialtoUserAssets
(
ID int primary key IDENTITY(1,1) not null,
Value float not null,
Rate float not null,
Benefit float not null,
Sign bit not null,
BelongsToUser int foreign key references Users (ID),
)


create table RialtoChat
(
ID int primary key IDENTITY(1,1) not null,
SenderName nvarchar(30) not null,
Created datetime not null,
Message nvarchar(max) not null,
)



create table SmsSending
(
ID int primary key IDENTITY(1,1) not null,
PublicKey uniqueidentifier,
Ip nvarchar(30) not null,
Counter int not null,
ResetTime  datetime,
)