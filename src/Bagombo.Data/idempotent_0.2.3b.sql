IF OBJECT_ID(N'__EFMigrationsHistory') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170513221518_Initial')
BEGIN
    CREATE TABLE [Author] (
        [Id] bigint NOT NULL IDENTITY,
        [ApplicationUserId] nvarchar(max),
        [Biography] nvarchar(max),
        [FirstName] nvarchar(max) NOT NULL,
        [LastName] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Author] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170513221518_Initial')
BEGIN
    CREATE TABLE [Category] (
        [Id] bigint NOT NULL IDENTITY,
        [Description] nvarchar(max),
        [Name] nvarchar(max),
        CONSTRAINT [PK_Category] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170513221518_Initial')
BEGIN
    CREATE TABLE [Topic] (
        [Id] bigint NOT NULL IDENTITY,
        [Description] nvarchar(max),
        [Title] nvarchar(max),
        CONSTRAINT [PK_Topic] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170513221518_Initial')
BEGIN
    CREATE TABLE [BlogPost] (
        [Id] bigint NOT NULL IDENTITY,
        [AuthorId] bigint,
        [Content] nvarchar(max),
        [CreatedAt] datetime2 NOT NULL,
        [Description] nvarchar(max),
        [ModifiedAt] datetime2 NOT NULL,
        [Public] bit NOT NULL,
        [PublishOn] datetime2 NOT NULL,
        [Title] nvarchar(max),
        CONSTRAINT [PK_BlogPost] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_BlogPost_Author_AuthorId] FOREIGN KEY ([AuthorId]) REFERENCES [Author] ([Id]) ON DELETE SET NULL
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170513221518_Initial')
BEGIN
    CREATE TABLE [BlogPostCategory] (
        [BlogPostId] bigint NOT NULL,
        [CategoryId] bigint NOT NULL,
        CONSTRAINT [PK_BlogPostCategory] PRIMARY KEY ([BlogPostId], [CategoryId]),
        CONSTRAINT [FK_BlogPostCategory_BlogPost_BlogPostId] FOREIGN KEY ([BlogPostId]) REFERENCES [BlogPost] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_BlogPostCategory_Category_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Category] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170513221518_Initial')
BEGIN
    CREATE TABLE [BlogPostTopic] (
        [TopicId] bigint NOT NULL,
        [BlogPostId] bigint NOT NULL,
        CONSTRAINT [PK_BlogPostTopic] PRIMARY KEY ([TopicId], [BlogPostId]),
        CONSTRAINT [FK_BlogPostTopic_BlogPost_BlogPostId] FOREIGN KEY ([BlogPostId]) REFERENCES [BlogPost] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_BlogPostTopic_Topic_TopicId] FOREIGN KEY ([TopicId]) REFERENCES [Topic] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170513221518_Initial')
BEGIN
    CREATE INDEX [IX_BlogPost_AuthorId] ON [BlogPost] ([AuthorId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170513221518_Initial')
BEGIN
    CREATE INDEX [IX_BlogPostCategory_CategoryId] ON [BlogPostCategory] ([CategoryId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170513221518_Initial')
BEGIN
    CREATE INDEX [IX_BlogPostTopic_BlogPostId] ON [BlogPostTopic] ([BlogPostId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170513221518_Initial')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20170513221518_Initial', N'1.1.2');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170528203126_1a')
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'Topic') AND [c].[name] = N'Title');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Topic] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [Topic] ALTER COLUMN [Title] nvarchar(450);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170528203126_1a')
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'Category') AND [c].[name] = N'Name');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Category] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [Category] ALTER COLUMN [Name] nvarchar(450);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170528203126_1a')
BEGIN
    CREATE UNIQUE INDEX [IX_Topic_Title] ON [Topic] ([Title]) WHERE [Title] IS NOT NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170528203126_1a')
BEGIN
    CREATE UNIQUE INDEX [IX_Category_Name] ON [Category] ([Name]) WHERE [Name] IS NOT NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170528203126_1a')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20170528203126_1a', N'1.1.2');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170528214424_0.2.3b')
BEGIN
    ALTER TABLE [Topic] ADD [ShowOnHomePage] bit NOT NULL DEFAULT 0;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170528214424_0.2.3b')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20170528214424_0.2.3b', N'1.1.2');
END;

GO

