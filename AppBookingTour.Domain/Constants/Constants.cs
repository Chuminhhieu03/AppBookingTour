using AppBookingTour.Domain.Entities;

namespace AppBookingTour.Domain.Constants
{
    public class Constants
    {
        public static class ActiveStatus
        {
            public const int Active = 1;
            public const int Inactive = 0;

            public static readonly Dictionary<int, string> dctName = new Dictionary<int, string>()
            {
                { Active, "Hiệu lực" },
                { Inactive, "Hết hiệu lực" }
            };
        }

        public static class Pagination
        {
            public const int PageSize = 20;
            public const int PageIndex = 0;
        }

        public static class ServiceType
        {
            public const int Accommodation = 1;
            public const int Tour = 2;
            public const int Combo = 3;

            public static readonly Dictionary<int, string> dctName = new()
            {
                { Accommodation, "Cơ sở lưu trú" },
                { Tour, "Tour" },
                { Combo, "Combo" }
            };
        }

        #region Accommodation

        public static class AccommodationType
        {
            public const int Hotel = 1;
            public const int Resort = 2;
            public const int Homestay = 3;

            public static readonly Dictionary<int, string> dctName = new()
            {
                { Hotel, "Khách sạn" },
                { Resort, "Resort" },
                { Homestay, "Homestay" }
            };
        }

        public static class RoomTypeStatus
        {
            public const int Active = 1;
            public const int Inactive = 0;
            public const int Draft = 2;
            
            public static readonly Dictionary<int, string> dctName = new()
            {
                { Active, "Hiệu lực" },
                { Inactive, "Hết hiệu lực" },
                { Draft, "Nháp" }
            };
        }

        #endregion
    }
}
