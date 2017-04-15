IF OBJECT_ID(N'__EFMigrationsHistory') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [Author] (
    [Id] bigint NOT NULL IDENTITY,
    [ApplicationUserId] nvarchar(max),
    [Biography] nvarchar(max),
    [FirstName] nvarchar(max) NOT NULL,
    [LastName] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Author] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Category] (
    [Id] bigint NOT NULL IDENTITY,
    [Description] nvarchar(max),
    [Name] nvarchar(max),
    CONSTRAINT [PK_Category] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Feature] (
    [Id] bigint NOT NULL IDENTITY,
    [Description] nvarchar(max),
    [Title] nvarchar(max),
    CONSTRAINT [PK_Feature] PRIMARY KEY ([Id])
);

GO

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

GO

CREATE TABLE [BlogPostCategory] (
    [BlogPostId] bigint NOT NULL,
    [CategoryId] bigint NOT NULL,
    CONSTRAINT [PK_BlogPostCategory] PRIMARY KEY ([BlogPostId], [CategoryId]),
    CONSTRAINT [FK_BlogPostCategory_BlogPost_BlogPostId] FOREIGN KEY ([BlogPostId]) REFERENCES [BlogPost] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_BlogPostCategory_Category_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Category] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [BlogPostFeature] (
    [FeatureId] bigint NOT NULL,
    [BlogPostId] bigint NOT NULL,
    CONSTRAINT [PK_BlogPostFeature] PRIMARY KEY ([FeatureId], [BlogPostId]),
    CONSTRAINT [FK_BlogPostFeature_BlogPost_BlogPostId] FOREIGN KEY ([BlogPostId]) REFERENCES [BlogPost] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_BlogPostFeature_Feature_FeatureId] FOREIGN KEY ([FeatureId]) REFERENCES [Feature] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_BlogPost_AuthorId] ON [BlogPost] ([AuthorId]);

GO

CREATE INDEX [IX_BlogPostCategory_CategoryId] ON [BlogPostCategory] ([CategoryId]);

GO

CREATE INDEX [IX_BlogPostFeature_BlogPostId] ON [BlogPostFeature] ([BlogPostId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20170415194728_initial', N'1.1.1');

GO

