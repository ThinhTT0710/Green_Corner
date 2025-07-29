namespace GreenCorner.MVC.Utility
{
    public class SD
    {
        public static string AuthAPIBase { get; set; }
        public static string EcommerceAPIBase { get; set; }
        public static string BlogAPIBase { get; set; }
        public static string EventAPIBase { get; set; }
        public static string RewardAPIBase { get; set; }
        public static string ChatAPIBase { get; set; }

        public const string RoleCustomer = "CUSTOMER";

        public const string RoleAdmin = "ADMIN";
        public const string RoleSaleStaff = "SALESTAFF";
        public const string RoleEventStaff = "EVENTSTAFF";


        public static string TokenCookie = "JWTToken";
        public enum APIType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}
