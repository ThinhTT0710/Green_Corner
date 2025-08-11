using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenCorner_Test.Selenium_Test
{
    public class SeleniumFixture : IDisposable
    {
        public IWebDriver Driver { get; private set; }
        public WebDriverWait Wait { get; private set; }
        public const string BaseUrl = "https://localhost:7000";
        
        public SeleniumFixture()
        {
            var options = new ChromeOptions();

            // Add options for better test stability
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--disable-extensions");
            options.AddArgument("--disable-web-security");
            options.AddArgument("--allow-running-insecure-content");
            // options.AddArgument("--headless=new");

            Driver = new ChromeDriver(options);
            Driver.Manage().Window.Maximize();
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(60);
            Driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(60);

            Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(60));
        }

        public void NavigateToUrl(string relativeUrl = "")
        {
            var fullUrl = string.IsNullOrEmpty(relativeUrl) ? BaseUrl : $"{BaseUrl}{relativeUrl}";
            Driver.Navigate().GoToUrl(fullUrl);
        }

        public IWebElement WaitForElement(By by, TimeSpan? timeout = null)
        {
            var wait = new WebDriverWait(Driver, timeout ?? TimeSpan.FromSeconds(60));
            return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(by));
        }

        public IWebElement WaitForElementToBeClickable(By by)
        {
            return Wait.Until(ExpectedConditions.ElementToBeClickable(by));
        }

        public IWebElement WaitForElementToBeVisible(By by)
        {
            return Wait.Until(ExpectedConditions.ElementIsVisible(by));
        }
        public void NavigateAndEnsureTitle(string url, string expectedTitle)
        {
            Driver.Navigate().GoToUrl(BaseUrl + url);

            // Đợi trang load hoàn toàn
            new WebDriverWait(Driver, TimeSpan.FromSeconds(60)).Until(
                d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete")
            );

            // Đợi tiêu đề xuất hiện và đúng text
            WaitForTextInElement(By.CssSelector("h2.content-title"), expectedTitle);
        }
        public bool WaitForElementToDisappear(By by)
        {
            return Wait.Until(ExpectedConditions.InvisibilityOfElementLocated(by));
        }
        public void WaitForTextInElement(By by, string expectedText, TimeSpan? timeout = null)
        {
            var wait = new WebDriverWait(Driver, timeout ?? TimeSpan.FromSeconds(60));
            wait.Until(d =>
            {
                try
                {
                    var element = d.FindElement(by);
                    return element.Text.Contains(expectedText, StringComparison.OrdinalIgnoreCase);
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });
        }

        public void WaitForPageToLoad()
        {
            Wait.Until(driver => ((IJavaScriptExecutor)driver)
                .ExecuteScript("return document.readyState").Equals("complete"));
        }

        public void TakeScreenshot(string testName)
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
