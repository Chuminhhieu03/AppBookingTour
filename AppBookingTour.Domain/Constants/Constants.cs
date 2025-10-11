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

    }
}
