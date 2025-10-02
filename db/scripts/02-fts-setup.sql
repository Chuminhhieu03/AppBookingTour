-- Full-Text Search Setup
-- Run this script after migrations are applied
-- Requires ALTER permissions on database

USE AppBookingTour;
GO

-- Create Full-Text Catalog
IF NOT EXISTS (SELECT * FROM sys.fulltext_catalogs WHERE name = 'AppBookingTourCatalog')
BEGIN
    CREATE FULLTEXT CATALOG AppBookingTourCatalog AS DEFAULT;
    PRINT 'Full-Text Catalog created successfully';
END
GO

-- Create Full-Text Index on Tours table
IF NOT EXISTS (SELECT * FROM sys.fulltext_indexes WHERE object_id = OBJECT_ID('tours.Tours'))
BEGIN
    CREATE FULLTEXT INDEX ON tours.Tours
    (
        Name LANGUAGE 'English',
        Description LANGUAGE 'English'
    )
    KEY INDEX PK_Tours
    ON AppBookingTourCatalog
    WITH CHANGE_TRACKING AUTO;
    
    PRINT 'Full-Text Index on Tours table created successfully';
END
GO

-- Create Full-Text Index on BlogPosts table
IF NOT EXISTS (SELECT * FROM sys.fulltext_indexes WHERE object_id = OBJECT_ID('content.BlogPosts'))
BEGIN
    CREATE FULLTEXT INDEX ON content.BlogPosts
    (
        Title LANGUAGE 'English',
        Content LANGUAGE 'English'
    )
    KEY INDEX PK_BlogPosts
    ON AppBookingTourCatalog
    WITH CHANGE_TRACKING AUTO;
    
    PRINT 'Full-Text Index on BlogPosts table created successfully';
END
GO

-- Test Full-Text Search
-- Example queries:

-- Search tours by keyword
-- SELECT * FROM tours.Tours 
-- WHERE CONTAINS((Name, Description), 'beach OR mountain');

-- Search blog posts
-- SELECT * FROM content.BlogPosts 
-- WHERE CONTAINS((Title, Content), 'travel AND adventure');

PRINT 'Full-Text Search setup completed successfully';