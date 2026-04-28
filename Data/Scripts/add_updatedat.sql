IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Courses') AND name = 'UpdatedAt')
BEGIN
    ALTER TABLE Courses ADD UpdatedAt datetime2 NULL;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Countries') AND name = 'UpdatedAt')
BEGIN
    ALTER TABLE Countries ADD UpdatedAt datetime2 NULL;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Teachers') AND name = 'UpdatedAt')
BEGIN
    ALTER TABLE Teachers ADD UpdatedAt datetime2 NULL;
END
