# UserManagement
A user management system

The service is documented with a swagger output.

To use this service on a ms sql server, create a database with this script:
```sql
IF OBJECT_ID(N'__EFMigrationsHistory') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [_users] (
    [Id] uniqueidentifier NOT NULL,
    [City] nvarchar(max) NULL,
    [Country] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [Login] nvarchar(max) NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    [PasswordHash] varbinary(max) NOT NULL,
    [PasswordSalt] varbinary(max) NOT NULL,
    [Phone] nvarchar(max) NULL,
    [Street] nvarchar(max) NULL,
    [Zip] nvarchar(max) NULL,
    CONSTRAINT [PK__users] PRIMARY KEY ([Id])
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20171109194418_initial', N'2.0.0-rtm-26452');

GO


```
