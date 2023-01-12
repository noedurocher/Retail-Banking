create table Customer(
CustomerID int primary key identity(1,1),
SSNID int unique,
Name varchar(50),
Age int,
AddressLine1 varchar(50),
AddressLine2 varchar(50),
City varchar(50),
State varchar(50),
Status varchar(50))


create table Account(
CustomerID int references Customer(CustomerID),
AccountID int primary key identity(1,1),
AccountType varchar(50),
AccountBalance int,
Status varchar(50))

create table Transactions(
AccountID int,
TransactionID int primary key identity(1,1),
TransactionDate Datetime,
TransactionType varchar(50),
TransactionAmount int)

insert into Transactions values (1,'8/14/2021','deposit',50)
insert into Transactions values (1,'8/14/2021','deposit',50)
insert into Transactions values (1,'8/14/2021','deposit',50)
insert into Transactions values (1,'8/14/2021','deposit',50)
insert into Transactions values (1,'8/14/2021','deposit',50)
insert into Transactions values (1,'8/14/2021','deposit',50)
insert into Transactions values (1,'8/14/2021','deposit',50)
insert into Transactions values (1,'8/14/2021','deposit',50)
insert into Transactions values (1,'8/14/2021','deposit',50)
insert into Transactions values (1,'8/14/2021','deposit',50)
insert into Transactions values (1,'8/14/2021','deposit',50)
insert into Transactions values (1,'8/14/2021','deposit',50)
insert into Transactions values (1,'8/14/2021','deposit',50)
insert into Transactions values (1,'8/14/2021','deposit',50)
insert into Transactions values (1,'8/14/2021','deposit',50)
insert into Transactions values (1,'8/14/2021','deposit',50)
insert into Transactions values (1,'8/14/2021','deposit',50)
insert into Transactions values (1,'8/14/2021','deposit',50)
insert into Transactions values (1,'8/14/2021','deposit',50)
insert into Transactions values (1,'8/14/2021','deposit',50)
insert into Transactions values (1,'8/14/2021','deposit',50)
insert into Transactions values (1,'8/14/2021','deposit',50)
insert into Transactions values (1,'8/14/2021','deposit',50)
insert into Transactions values (1,'8/14/2021','deposit',50)
insert into Transactions values (1,'8/14/2021','deposit',50)
insert into Transactions values (1,'8/14/2021','deposit',50)
insert into Transactions values (1,'8/14/2021','deposit',50)
insert into Transactions values (1,'8/14/2021','deposit',50)
insert into Transactions values (1,'8/14/2021','deposit',50)
insert into Transactions values (1,'8/14/2021','deposit',50)


create table Error(
ErrorID int primary key,
ErrorDescription varchar(100))

insert into Error values (200,'Cannot create more than 2 accounts, Delete an account to proceed.')
insert into Error values (300,'Account Type already exist.')
insert into Error values (2,'No accounts found.')
insert into Error values (3,'No avaliable statements or Account does not exist')


SET IDENTITY_INSERT Account ON
SET IDENTITY_INSERT Transactions ON

select * from Customer
select * from Account

select * from Transactions