using System.ComponentModel;

namespace AppBookingTour.Domain.Enums;

/// <summary>
/// Shared enums for the domain layer
/// Centralized location for all business enums
/// </summary>

#region User Enums
public enum UserStatus
{
    Active = 1,
    Inactive = 2,
    Suspended = 3,
    Deleted = 4
}

public enum UserType
{
    Customer = 1,
    Staff = 2,
    Admin = 3,
    Guide = 4
}
#endregion

#region Tour Enums
public enum TourPriceLevel
{
    Budget = 1,
    Standard = 2,
    Premium = 3,
    Luxury = 4
}

public enum TourDepartureStatus
{
    Scheduled = 1,
    Available = 2,
    Full = 3,
    Cancelled = 4,
    Completed = 5
}
#endregion

#region Booking Enums
public enum BookingType
{
    Tour = 1,
    Hotel = 2,
    Combo = 3,
    Flight = 4
}

public enum BookingStatus
{
    Pending = 1,
    Confirmed = 2,
    Paid = 3,
    Cancelled = 4,
    Completed = 5,
    Refunded = 6
}

public enum PaymentType
{
    FullPayment = 1,
    Deposit = 2,
    Installment = 3
}

public enum ParticipantType
{
    Adult = 1,
    Child = 2,
    Infant = 3,
    Senior = 4
}
#endregion

#region Payment Enums
public enum PaymentStatus
{
    Pending = 1,
    Processing = 2,
    Completed = 3,
    Failed = 4,
    Cancelled = 5,
    Refunded = 6,
    PartiallyRefunded = 7
}

public enum PaymentMethodType
{
    Cash = 1,
    Credit = 2,
    BankTransfer = 3,
    DigitalWallet = 4,
    Cryptocurrency = 5
}

public enum PaymentGateway
{
    VNPay = 1,
    Momo = 2,
    ZaloPay = 3,
    PayPal = 4,
    Stripe = 5
}
#endregion

#region Promotion Enums
public enum PromotionStatus
{
    Draft = 1,
    Active = 2,
    Expired = 3,
    Suspended = 4,
    Deleted = 5
}

public enum DiscountType
{
    Percentage = 1,
    FixedAmount = 2,
    FreeShipping = 3,
    BuyOneGetOne = 4
}

public enum ApplicableTo
{
    All = 1,
    Tours = 2,
    Hotels = 3,
    Combos = 4,
    SpecificItems = 5
}

public enum PromotionType
{
    General = 1,
    FirstTime = 2,
    Loyalty = 3,
    Seasonal = 4,
    Flash = 5
}
#endregion

#region Content Enums
public enum ContentStatus
{
    Draft = 1,
    Published = 2,
    Archived = 3,
    Deleted = 4
}

public enum ReviewStatus
{
    Pending = 1,
    Approved = 2,
    Rejected = 3,
    Hidden = 4
}

public enum ReviewType
{
    Tour = 1,
    Hotel = 2,
    Guide = 3,
    Service = 4
}
#endregion

#region General Enums
public enum Region
{
    [Description("Miền Bắc")]
    North = 1,

    [Description("Miền Trung")]
    Central = 2,

    [Description("Nam Trung Bộ")]
    SouthCentral = 3,

    [Description("Tây Nam Bộ")]
    SouthWest = 4
}

public enum Vehicle
{
    Car = 1,
    Plane = 2
}

public enum Currency
{
    VND = 1,
    USD = 2,
    EUR = 3,
    JPY = 4
}

public enum Language
{
    Vietnamese = 1,
    English = 2,
    Japanese = 3,
    Korean = 4,
    Chinese = 5
}
#endregion


public enum BlogStatus
{
    Draft = 1,
    Published = 2,
    Archived = 3
}

public enum Gender
{
    Male = 1,
    Female = 2,
    Other = 3
}

public enum ComboStatus
{
    Available = 1,
    Full = 2,
    Cancelled = 3
}

public enum ItemType
{
    Tour = 1,
    Accommodation = 2,
    Combo = 3
}

public enum ReviewItemType
{
    Tour = 1,
    Hotel = 2,
    Combo = 3,
}

public enum DepartureStatus
{
    Available = 1,
    Full = 2,
    Cancelled = 3
}

public enum PriceLevel
{
    Budget = 1,
    Standard = 2,
    Premium = 3
}

public enum EntityType
{
    Accommodation = 1,
    Tour = 2,
    Combo = 3,
    User = 4,
    Review = 5,
    RoomType = 6
}

public enum FeatureCode
{
    [Description("RoomTypeAmenity")]
    RoomTypeAmenity,
    [Description("AccommodationAmenity")]
    AccommodationAmenity
}