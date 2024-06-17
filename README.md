Compatible with https://github.com/DontDoThat21/Angular16 Angular basic demo application for the management of Employees and their Departments with a simple SQL Server DB powering this modern .NET Core API's datas.

Want to use a legacy .NET FW (.NET FW 4.7.2) Web API (still JSON output) with Ang16 instead?
See: https://github.com/DontDoThat21/WebAPI.NET-FW

Table creation SQL (SQL Server): 

  create table Departments(
  DepartmentId int identity(1,1),
  DepartmentName varchar(500)
  );

  create table Employees(
  EmployeeId int identity(1,1),
  EmployeeName varchar(500),
  Department varchar(500),
  DateJoined date,
  PhotoFileName varchar(500)
  );

SELECT TOP (1000) [DepartmentId]
      ,[DepartmentName]
  FROM [EmployeeAppDb].[dbo].[Departments]

SELECT TOP (1000) [EmployeeId]
      ,[EmployeeName]
      ,[Department]
      ,[DateJoined]
      ,[PhotoFileName]
  FROM [EmployeeAppDb].[dbo].[Employees]
