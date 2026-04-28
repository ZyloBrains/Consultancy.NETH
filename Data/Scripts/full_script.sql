IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Blogs] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(300) NOT NULL,
    [TitleNp] nvarchar(300) NOT NULL,
    [Slug] nvarchar(300) NOT NULL,
    [ShortDescription] nvarchar(500) NULL,
    [ShortDescriptionNp] nvarchar(500) NULL,
    [Content] nvarchar(max) NULL,
    [ContentNp] nvarchar(max) NULL,
    [Image] nvarchar(500) NULL,
    [Author] nvarchar(200) NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Blogs] PRIMARY KEY ([Id])
);

CREATE TABLE [Categories] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    [NameNp] nvarchar(100) NOT NULL,
    [Slug] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_Categories] PRIMARY KEY ([Id])
);

CREATE TABLE [ContactInquiries] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(200) NOT NULL,
    [Email] nvarchar(200) NOT NULL,
    [Phone] nvarchar(20) NULL,
    [Subject] nvarchar(200) NULL,
    [Message] nvarchar(max) NULL,
    [Status] nvarchar(50) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_ContactInquiries] PRIMARY KEY ([Id])
);

CREATE TABLE [Countries] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    [NameNp] nvarchar(100) NOT NULL,
    [Slug] nvarchar(100) NOT NULL,
    [FlagImage] nvarchar(500) NULL,
    [Description] nvarchar(max) NULL,
    [DescriptionNp] nvarchar(max) NULL,
    [Universities] nvarchar(500) NULL,
    [CostOfLiving] nvarchar(500) NULL,
    [VisaInfo] nvarchar(1000) NULL,
    [WorkPermit] nvarchar(1000) NULL,
    [IsActive] bit NOT NULL,
    [DisplayOrder] int NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Countries] PRIMARY KEY ([Id])
);

CREATE TABLE [Events] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(300) NOT NULL,
    [TitleNp] nvarchar(300) NOT NULL,
    [Slug] nvarchar(300) NOT NULL,
    [Description] nvarchar(max) NULL,
    [DescriptionNp] nvarchar(max) NULL,
    [Image] nvarchar(500) NULL,
    [Location] nvarchar(300) NULL,
    [EventDate] datetime2 NOT NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Events] PRIMARY KEY ([Id])
);

CREATE TABLE [SiteSettings] (
    [Key] nvarchar(100) NOT NULL,
    [Value] nvarchar(1000) NULL,
    [ValueNp] nvarchar(1000) NULL,
    [Category] nvarchar(100) NULL,
    CONSTRAINT [PK_SiteSettings] PRIMARY KEY ([Key])
);

CREATE TABLE [Teachers] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(200) NOT NULL,
    [NameNp] nvarchar(200) NOT NULL,
    [Designation] nvarchar(100) NULL,
    [DesignationNp] nvarchar(100) NULL,
    [Photo] nvarchar(500) NULL,
    [Bio] nvarchar(max) NULL,
    [BioNp] nvarchar(max) NULL,
    [Facebook] nvarchar(200) NULL,
    [Instagram] nvarchar(200) NULL,
    [LinkedIn] nvarchar(200) NULL,
    [DisplayOrder] int NOT NULL,
    [IsActive] bit NOT NULL,
    [IsFeatured] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Teachers] PRIMARY KEY ([Id])
);

CREATE TABLE [Courses] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(200) NOT NULL,
    [NameNp] nvarchar(200) NOT NULL,
    [Slug] nvarchar(200) NOT NULL,
    [Description] nvarchar(max) NULL,
    [DescriptionNp] nvarchar(max) NULL,
    [Image] nvarchar(500) NULL,
    [CategoryId] int NOT NULL,
    [CountryId] int NULL,
    [Duration] nvarchar(100) NULL,
    [Fees] decimal(18,2) NULL,
    [IsFeatured] bit NOT NULL,
    [DisplayOrder] int NOT NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Courses] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Courses_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Categories] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Courses_Countries_CountryId] FOREIGN KEY ([CountryId]) REFERENCES [Countries] ([Id]) ON DELETE SET NULL
);

CREATE TABLE [CourseTeachers] (
    [CourseId] int NOT NULL,
    [TeacherId] int NOT NULL,
    CONSTRAINT [PK_CourseTeachers] PRIMARY KEY ([CourseId], [TeacherId]),
    CONSTRAINT [FK_CourseTeachers_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_CourseTeachers_Teachers_TeacherId] FOREIGN KEY ([TeacherId]) REFERENCES [Teachers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Students] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(200) NOT NULL,
    [Email] nvarchar(200) NOT NULL,
    [Phone] nvarchar(20) NULL,
    [CourseId] int NULL,
    [CountryId] int NULL,
    [Message] nvarchar(max) NULL,
    [Status] nvarchar(50) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Students] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Students_Countries_CountryId] FOREIGN KEY ([CountryId]) REFERENCES [Countries] ([Id]),
    CONSTRAINT [FK_Students_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id])
);

CREATE TABLE [Testimonials] (
    [Id] int NOT NULL IDENTITY,
    [StudentName] nvarchar(200) NOT NULL,
    [StudentNameNp] nvarchar(200) NOT NULL,
    [StudentPhoto] nvarchar(500) NULL,
    [Message] nvarchar(max) NOT NULL,
    [MessageNp] nvarchar(max) NULL,
    [CourseId] int NULL,
    [IsActive] bit NOT NULL,
    [DisplayOrder] int NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Testimonials] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Testimonials_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id])
);

CREATE INDEX [IX_Courses_CategoryId] ON [Courses] ([CategoryId]);

CREATE INDEX [IX_Courses_CountryId] ON [Courses] ([CountryId]);

CREATE INDEX [IX_CourseTeachers_TeacherId] ON [CourseTeachers] ([TeacherId]);

CREATE INDEX [IX_Students_CountryId] ON [Students] ([CountryId]);

CREATE INDEX [IX_Students_CourseId] ON [Students] ([CourseId]);

CREATE INDEX [IX_Testimonials_CourseId] ON [Testimonials] ([CourseId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260426160134_InitialCreate', N'10.0.0');

COMMIT;
GO

BEGIN TRANSACTION;
ALTER TABLE [Teachers] ADD [UpdatedAt] datetime2 NULL;

ALTER TABLE [Courses] ADD [UpdatedAt] datetime2 NULL;

ALTER TABLE [Countries] ADD [UpdatedAt] datetime2 NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260427164208_AddUpdatedAtToEntities', N'10.0.0');

COMMIT;
GO

