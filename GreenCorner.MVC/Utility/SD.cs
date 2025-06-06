namespace GreenCorner.MVC.Utility
{
    public class SD
    {
        public static string AuthAPIBase { get; set; }
        public static string ProductAPIBase { get; set; }
        public static string EventAPIBase { get; set; }
        public const string RoleCustomer = "CUSTOMER";

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
