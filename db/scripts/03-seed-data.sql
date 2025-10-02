-- Seed Initial Data
USE AppBookingTour;
GO

-- Insert Payment Methods
IF NOT EXISTS (SELECT 1 FROM payments.PaymentMethods WHERE Code = 'CREDIT_CARD')
BEGIN
    INSERT INTO payments.PaymentMethods (Name, Code, Description, IsActive, CreatedAt)
    VALUES 
        ('Credit Card', 'CREDIT_CARD', 'Visa, MasterCard, American Express', 1, GETUTCDATE()),
        ('Bank Transfer', 'BANK_TRANSFER', 'Direct bank transfer', 1, GETUTCDATE()),
        ('PayPal', 'PAYPAL', 'PayPal online payment', 1, GETUTCDATE()),
        ('Cash', 'CASH', 'Cash payment at office', 1, GETUTCDATE());
    
    PRINT 'Payment methods seeded successfully';
END
GO

-- Insert Sample Cities
IF NOT EXISTS (SELECT 1 FROM catalog.Cities WHERE Name = 'Ho Chi Minh City')
BEGIN
    INSERT INTO catalog.Cities (Name, Description, CountryCode, ImageUrl, IsActive, CreatedAt)
    VALUES 
        ('Ho Chi Minh City', 'The largest city in Vietnam, known for its French colonial landmarks', 'VN', '/images/cities/hcmc.jpg', 1, GETUTCDATE()),
        ('Hanoi', 'The capital city of Vietnam, rich in history and culture', 'VN', '/images/cities/hanoi.jpg', 1, GETUTCDATE()),
        ('Da Nang', 'Coastal city known for its beaches and mountains', 'VN', '/images/cities/danang.jpg', 1, GETUTCDATE()),
        ('Nha Trang', 'Beach resort city on the South Central Coast', 'VN', '/images/cities/nhatrang.jpg', 1, GETUTCDATE()),
        ('Hoi An', 'Ancient town famous for its well-preserved architecture', 'VN', '/images/cities/hoian.jpg', 1, GETUTCDATE());
    
    PRINT 'Cities seeded successfully';
END
GO

-- Insert Tour Categories and Types
IF NOT EXISTS (SELECT 1 FROM tours.TourCategories WHERE Name = 'Adventure')
BEGIN
    DECLARE @AdventureId INT, @CulturalId INT, @BeachId INT, @CityId INT;
    
    INSERT INTO tours.TourCategories (Name, Description, ImageUrl, IsActive, CreatedAt)
    VALUES 
        ('Adventure', 'Thrilling outdoor activities and exploration', '/images/categories/adventure.jpg', 1, GETUTCDATE()),
        ('Cultural', 'Immerse in local culture and traditions', '/images/categories/cultural.jpg', 1, GETUTCDATE()),
        ('Beach & Island', 'Relax on beautiful beaches and islands', '/images/categories/beach.jpg', 1, GETUTCDATE()),
        ('City Tours', 'Explore vibrant cities and urban attractions', '/images/categories/city.jpg', 1, GETUTCDATE());
    
    SET @AdventureId = (SELECT Id FROM tours.TourCategories WHERE Name = 'Adventure');
    SET @CulturalId = (SELECT Id FROM tours.TourCategories WHERE Name = 'Cultural');
    SET @BeachId = (SELECT Id FROM tours.TourCategories WHERE Name = 'Beach & Island');
    SET @CityId = (SELECT Id FROM tours.TourCategories WHERE Name = 'City Tours');
    
    INSERT INTO tours.TourTypes (Name, Description, ImageUrl, IsActive, CategoryId, CreatedAt)
    VALUES 
        ('Trekking', 'Mountain and nature trekking adventures', '/images/types/trekking.jpg', 1, @AdventureId, GETUTCDATE()),
        ('Cycling', 'Bicycle tours through scenic routes', '/images/types/cycling.jpg', 1, @AdventureId, GETUTCDATE()),
        ('Heritage', 'UNESCO World Heritage site visits', '/images/types/heritage.jpg', 1, @CulturalId, GETUTCDATE()),
        ('Food Tour', 'Culinary experiences and local cuisine', '/images/types/food.jpg', 1, @CulturalId, GETUTCDATE()),
        ('Island Hopping', 'Visit multiple islands in one trip', '/images/types/island.jpg', 1, @BeachId, GETUTCDATE()),
        ('Beach Resort', 'Relaxing beach resort holidays', '/images/types/beach.jpg', 1, @BeachId, GETUTCDATE()),
        ('Walking Tour', 'Guided walking tours of city highlights', '/images/types/walking.jpg', 1, @CityId, GETUTCDATE()),
        ('Photography', 'Photography-focused city tours', '/images/types/photo.jpg', 1, @CityId, GETUTCDATE());
    
    PRINT 'Tour categories and types seeded successfully';
END
GO

-- Insert Promotions
IF NOT EXISTS (SELECT 1 FROM promotions.Promotions WHERE Code = 'WELCOME10')
BEGIN
    INSERT INTO promotions.Promotions (Code, Name, Description, Type, Value, MinimumAmount, MaximumDiscount, StartDate, EndDate, MaxUsages, CurrentUsages, IsActive, CreatedAt)
    VALUES 
        ('WELCOME10', 'Welcome Discount', '10% off for new customers', 1, 10.00, 500000, 200000, GETUTCDATE(), DATEADD(YEAR, 1, GETUTCDATE()), 1000, 0, 1, GETUTCDATE()),
        ('SUMMER25', 'Summer Special', '25% off summer tours', 1, 25.00, 1000000, 500000, GETUTCDATE(), DATEADD(MONTH, 3, GETUTCDATE()), 500, 0, 1, GETUTCDATE()),
        ('FAMILY50', 'Family Package', '50,000 VND off family bookings', 2, 50000, 2000000, NULL, GETUTCDATE(), DATEADD(MONTH, 6, GETUTCDATE()), NULL, 0, 1, GETUTCDATE());
    
    PRINT 'Promotions seeded successfully';
END
GO

PRINT 'Database seeding completed successfully';