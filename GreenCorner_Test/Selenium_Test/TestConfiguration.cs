namespace GreenCorner_Test.Selenium_Test
{
    public static class TestConfiguration
    {
        // Test URLs
        public const string BaseUrl = "http://localhost:7000";
        public const string LoginUrl = "/Auth/Login";
        public const string RegisterUrl = "/Auth/Register";
        public const string HomeUrl = "/";
        public const string ProductsUrl = "/Product";
        public const string CartUrl = "/Cart";
        public const string EventsUrl = "/Event";
        public const string BlogUrl = "/BlogPost";
        public const string RewardsUrl = "/RewardPoint";

        // Test Data
        public static class TestUsers
        {
            public const string ValidEmail = "test@example.com";
            public const string ValidPassword = "TestPassword123!";
            public const string InvalidEmail = "invalid-email";
            public const string InvalidPassword = "123";
        }

        // Timeouts
        public const int DefaultTimeout = 20;

        public const int ShortTimeout = 5;
        public const int LongTimeout = 30;

        // Browser Settings
        public const bool HeadlessMode = false;
        public const bool TakeScreenshots = true;
        public const string ScreenshotDirectory = "Screenshots";

        // Test Categories
        public static class TestCategories
        {
            public const string Smoke = "Smoke";
            public const string Regression = "Regression";
            public const string UI = "UI";
            public const string Functional = "Functional";
            public const string Integration = "Integration";
        }

        // Element Selectors (Common)
        public static class Selectors
        {
            // Common Elements
            public const string LoadingSpinner = ".loading-spinner";
            public const string ErrorMessage = ".error-message";
            public const string SuccessMessage = ".success-message";
            public const string Modal = ".modal";
            public const string ModalClose = ".modal-close";

            // Navigation
            public const string NavigationMenu = "#mainNavigation";
            public const string LoginLink = "a[href*='Login']";
            public const string RegisterLink = "a[href*='Register']";
            public const string LogoutLink = "a[href*='Logout']";

            // Forms
            public const string Form = "form";
            public const string SubmitButton = "button[type='submit']";
            public const string CancelButton = "button[type='button']";

            // Validation
            public const string ValidationError = ".text-danger";
            public const string RequiredField = "[required]";
        }

        // Test Environment
        public static class Environment
        {
            public const string Development = "Development";
            public const string Staging = "Staging";
            public const string Production = "Production";
        }

        // Browser Options
        public static class BrowserOptions
        {
            public static readonly string[] ChromeArguments = new[]
            {
                "--no-sandbox",
                "--disable-dev-shm-usage",
                "--disable-gpu",
                "--disable-extensions",
                "--disable-web-security",
                "--allow-running-insecure-content"
            };
        }
    }
}
