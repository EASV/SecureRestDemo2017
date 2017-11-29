DROP TABLE Users
GO

CREATE TABLE Users
(
   Id        INT    NOT NULL   PRIMARY KEY, -- primary key column
   Username      [NVARCHAR](50)  NOT NULL,
   Password   [NVARCHAR](50)  NOT NULL,
   Role   [NVARCHAR](50)
);
GO

INSERT INTO Users
   ([Id],[Username],[Password], [Role])
VALUES
   ( 1, N'lbilde', N'shh', N'Administrator'),
   ( 2, N'dinko', N'aha', N'')   
GO   

SELECT * FROM Users
Go


