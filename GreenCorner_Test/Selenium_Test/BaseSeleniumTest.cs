using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using Xunit;

namespace GreenCorner_Test.Selenium_Test
{
    public abstract class BaseSeleniumTest : IDisposable
    {
        protected IWebDriver Driver { get; private set; }
        protected WebDriverWait Wait { get; private set; }
        protected const string BaseUrl = "https://localhost:7000";

        protected BaseSeleniumTest()
        {
            InitializeWebDriver();
        }

        private void InitializeWebDriver()
        {
            var options = new ChromeOptions();

            // Add options for better test stability
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--disable-extensions");
            options.AddArgument("--disable-web-security");
            options.AddArgument("--allow-running-insecure-content");

            // Uncomment the line below to run in headless mode
            // options.AddArgument("--headless");

            Driver = new ChromeDriver(options);
            Driver.Manage().Window.Maximize();
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            Driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);

            Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(20));
        }

        protected void NavigateToUrl(string relativeUrl = "")
        {
            var fullUrl = string.IsNullOrEmpty(relativeUrl) ? BaseUrl : $"{BaseUrl}{relativeUrl}";
            Driver.Navigate().GoToUrl(fullUrl);
        }

        protected IWebElement WaitForElement(By by)
        {
            return Wait.Until(driver => driver.FindElement(by));
        }

        protected IWebElement WaitForElementToBeClickable(By by)
        {
            return Wait.Until(ExpectedConditions.ElementToBeClickable(by));
        }

        protected IWebElement WaitForElementToBeVisible(By by)
        {
            return Wait.Until(ExpectedConditions.ElementIsVisible(by));
        }

        protected bool WaitForElementToDisappear(By by)
        {
            return Wait.Until(ExpectedConditions.InvisibilityOfElementLocated(by));
        }

        protected void WaitForPageToLoad()
        {
            Wait.Until(driver => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
        }

        protected void TakeScreenshot(string testName)
        {
            try
            {
                var screenshot = ((ITakesScreenshot)Driver).GetScreenshot();
                var fileName = $"{testName}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Screenshots", fileName);

                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                screenshot.SaveAsFile(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to take screenshot: {ex.Message}");
            }
        }

        public void Dispose()
        {
            Driver?.Quit();
            Driver?.Dispose();
        }
    }
}
