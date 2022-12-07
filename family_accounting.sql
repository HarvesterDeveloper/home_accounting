USE [master]
GO
/*CREATE DATABASE [home_accounting] ON PRIMARY ( NAME = N'home_accounting', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\home_accounting.mdf') COLLATE Cyrillic_General_CI_AS*/
CREATE DATABASE [home_accounting] ON PRIMARY ( NAME = N'home_accounting', FILENAME = N'D:\MSSQL\home_accounting.mdf') COLLATE Cyrillic_General_CI_AS
GO

USE [home_accounting]
GO

CREATE TABLE members
(
	id INT IDENTITY(100,1) not null,
	name NVARCHAR(25),
	password NVARCHAR(50),
	CONSTRAINT pk_member PRIMARY KEY (id),
	CONSTRAINT unique_family_member UNIQUE (name)
)
GO

CREATE TABLE goals
(
	id INT IDENTITY(0,1) NOT NULL,
	owner_id INT,
	name NVARCHAR(25),
	description NVARCHAR(100),
	goal_money MONEY,
	CONSTRAINT pk_goals PRIMARY KEY (id),
	CONSTRAINT fk_goals_to_members FOREIGN KEY (owner_id) REFERENCES members (id) ON DELETE CASCADE ON UPDATE CASCADE
)
GO

CREATE TABLE actions
(
	id INT IDENTITY(0,1) NOT NULL,
	owner_id INT,
	goal_id INT,
	source TEXT,
	is_income BIT,
	amount MONEY,
	date DATETIME2,
	commentary TEXT,
	CONSTRAINT pk_actions PRIMARY KEY (id),
	CONSTRAINT fk_actions_to_members FOREIGN KEY (owner_id) REFERENCES members (id) ON DELETE CASCADE ON UPDATE CASCADE
)
GO

SET IDENTITY_INSERT members ON
insert into members(id, name, password)
values(1, 'Victor', 'asd'),
      (2, 'Victoria', 'zxc')
SET IDENTITY_INSERT members OFF
GO

SET IDENTITY_INSERT goals ON
insert into goals(id, owner_id, name, description, goal_money)
values(1, 1, 'На всякий случай', 'just in case', 100000.00)
SET IDENTITY_INSERT goals OFF
GO

SET IDENTITY_INSERT actions ON
insert into actions(id, owner_id, goal_id, source, is_income, amount, date, commentary)
values(1, 1, 1, 'Фриланс', 1, 1337.228, '2022-09-01 12:00:00', 'заработал с заказов'),
      (2, 1, -1, 'За жилье', 0, 2500, '2022-09-01 12:00:00', 'за съемную квартиру'),
	  (3, 1, -1, 'Коммунальные услуги', 0, 5000, '2022-09-01 12:00:00', ''),
	  (4, 1, -1, 'Еда', 0, 5000, '2022-09-01 12:00:00', ''),
	  (5, 1, -1, 'Интернет', 0, 400, '2022-09-01 12:00:00', ''),
	  (6, 1, -1, 'На мобильный', 0, 800, '2022-09-01 12:00:00', 'на обе симкарты'),
	  (7, 1, -1, 'Одежда', 0, 4500, '2022-09-01 12:00:00', ''),
	  (8, 1, -1, 'Бытовые расходы', 0, 2000, '2022-09-01 12:00:00', ''),
	  (9, 1, -1, 'Развлечения', 0, 1000, '2022-09-01 12:00:00', '')
SET IDENTITY_INSERT actions OFF
GO