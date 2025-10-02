-- Enable READ_COMMITTED_SNAPSHOT for better concurrency
-- Run this as an admin user with ALTER DATABASE permissions

USE master;
GO

-- Set database to single user mode temporarily
ALTER DATABASE AppBookingTour SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
GO

-- Enable READ_COMMITTED_SNAPSHOT
ALTER DATABASE AppBookingTour SET READ_COMMITTED_SNAPSHOT ON;
GO

-- Return to multi-user mode
ALTER DATABASE AppBookingTour SET MULTI_USER;
GO

USE AppBookingTour;
GO

PRINT 'READ_COMMITTED_SNAPSHOT enabled successfully';