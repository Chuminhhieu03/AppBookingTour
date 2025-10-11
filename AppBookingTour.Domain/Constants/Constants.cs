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

        public static class Category
        {
            public const int Hotel = 1;
            public const int Tour = 2;
            public const int Combo = 3;

            public static readonly Dictionary<int, string> dctName = new()
            {
                { Hotel, "Khách sạn" },
                { Tour, "Tour" },
                { Combo, "Combo" }
            };
        }
    }
}
