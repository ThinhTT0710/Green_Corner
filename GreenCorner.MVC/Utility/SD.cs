namespace GreenCorner.MVC.Utility
{
    public class SD
    {
        public static string AuthAPIBase { get; set; }
        public static string EcommerceAPIBase { get; set; }
        public static string BlogAPIBase { get; set; }
        public static string EventAPIBase { get; set; }
        public static string RewardAPIBase { get; set; }
      
        public const string RoleCustomer = "CUSTOMER";

        public const string RoleStaff = "STAFF";

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
