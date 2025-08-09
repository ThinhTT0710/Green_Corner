using OpenQA.Selenium;
using FluentAssertions;
using DocumentFormat.OpenXml.Bibliography;
using GreenCorner_Test.Selenium_Test;

namespace GreenCorner_Test.Selenium_Test
{
    public class HomePageTests : BaseSeleniumTest
    {
        [Fact]
        public void HomePageShouldLoadSuccessfully()
        {
            // Arrange & Act
            NavigateToUrl("/");

            // Assert
            WaitForPageToLoad();
            var pageTitle = Driver.Title;
            pageTitle.Should().NotBeNullOrEmpty("Page title should not be empty");

            var currentUrl = Driver.Url;
            currentUrl.Should().Be(BaseUrl + "/", "Should be on the home page");
        }

        [Fact]
        public void HomePageShouldDisplayMainNavigationMenu()
        {
            // Arrange & Act
            NavigateToUrl("/");

            // Assert
            WaitForPageToLoad();

            // Check for main navigation elements
            var navigationElements = new[] { "Home", "Events", "Blog", "Shop", "Rewards" };

            foreach (var navItem in navigationElements)
            {
                var element = Driver.FindElement(By.LinkText(navItem));
                element.Should().NotBeNull($"Navigation item '{navItem}' should be present");
                element.Displayed.Should().BeTrue($"Navigation item '{navItem}' should be visible");
            }
        }

        [Fact]
        public void HomePageShouldDisplayHeroSection()
        {
            // Arrange & Act
            NavigateToUrl("/");

            // Assert
            WaitForPageToLoad();

            // Look for hero section elements
            var heroSection = Driver.FindElement(By.ClassName("hero-section"));
            heroSection.Should().NotBeNull("Hero section should be present");
            heroSection.Displayed.Should().BeTrue("Hero section should be visible");
        }

        [Fact]
        public void HomePageShouldDisplayFeaturedEvents()
        {
            // Arrange & Act
            NavigateToUrl("/");

            // Assert
            WaitForPageToLoad();

            // Look for featured events section
            var featuredEventsSection = Driver.FindElement(By.Id("featuredEvents"));
            featuredEventsSection.Should().NotBeNull("Featured events section should be present");
            featuredEventsSection.Displayed.Should().BeTrue("Featured events section should be visible");
        }

        [Fact]
        public void HomePageShouldDisplayLatestBlogPosts()
        {
            // Arrange & Act
            NavigateToUrl("/");

            // Assert
            WaitForPageToLoad();

            // Look for latest blog posts section
            var latestBlogSection = Driver.FindElement(By.Id("latestBlogPosts"));
            latestBlogSection.Should().NotBeNull("Latest blog posts section should be present");
            latestBlogSection.Displayed.Should().BeTrue("Latest blog posts section should be visible");
        }

        [Fact]
        public void HomePageShouldDisplayFooter()
        {
            // Arrange & Act
            NavigateToUrl("/");

            // Assert
            WaitForPageToLoad();

            var footer = Driver.FindElement(By.TagName("footer"));
            footer.Should().NotBeNull("Footer should be present");
            footer.Displayed.Should().BeTrue("Footer should be visible");
        }

        [Fact]
        public void NavigationLinksShouldWorkCorrectly()
        {
            // Arrange
            NavigateToUrl("/");

            // Act & Assert - Test each navigation link
            var navigationTests = new[]
            {
                new { LinkText = "Events", ExpectedUrl = "/Event" },
                new { LinkText = "Blog", ExpectedUrl = "/BlogPost" },
                new { LinkText = "Shop", ExpectedUrl = "/Product" },
                new { LinkText = "Rewards", ExpectedUrl = "/RewardPoint" }
            };

            foreach (var test in navigationTests)
            {
                // Navigate back to home page for each test
                NavigateToUrl("/");

                var link = WaitForElementToBeClickable(By.LinkText(test.LinkText));
                link.Click();

                WaitForPageToLoad();
                var currentUrl = Driver.Url;
                currentUrl.Should().Contain(test.ExpectedUrl, $"Clicking '{test.LinkText}' should navigate to {test.ExpectedUrl}");
            }
        }

        [Fact]
        public void HomePageShouldBeResponsive()
        {
            // Arrange
            NavigateToUrl("/");

            // Act & Assert - Test different screen sizes
            var screenSizes = new[]
            {
                new { Width = 1920, Height = 1080, Name = "Desktop" },
                new { Width = 768, Height = 1024, Name = "Tablet" },
                new { Width = 375, Height = 667, Name = "Mobile" }
            };

            foreach (var size in screenSizes)
            {
                Driver.Manage().Window.Size = new System.Drawing.Size(size.Width, size.Height);

                // Wait for any responsive adjustments
                Thread.Sleep(1000);

                var body = Driver.FindElement(By.TagName("body"));
                body.Displayed.Should().BeTrue($"Page should be visible on {size.Name} screen size");
            }
        }

        [Fact]
        public void HomePageShouldHaveProperMetaTags()
        {
            // Arrange & Act
            NavigateToUrl("/");

            // Assert
            WaitForPageToLoad();

            var metaDescription = Driver.FindElement(By.CssSelector("meta[name='description']"));
            metaDescription.Should().NotBeNull("Meta description should be present");

            var metaViewport = Driver.FindElement(By.CssSelector("meta[name='viewport']"));
            metaViewport.Should().NotBeNull("Meta viewport should be present");
        }

        [Fact]
        public void HomePageShouldLoadQuickly()
        {
            // Arrange
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            // Act
            NavigateToUrl("/");
            WaitForPageToLoad();

            // Assert
            stopwatch.Stop();
            var loadTime = stopwatch.ElapsedMilliseconds;
            loadTime.Should().BeLessThan(5000, "Home page should load within 5 seconds");
        }

        [Fact]
        public void HomePageShouldDisplaySearchFunctionality()
        {
            // Arrange & Act
            NavigateToUrl("/");

            // Assert
            WaitForPageToLoad();

            var searchBox = Driver.FindElement(By.Id("searchBox"));
            searchBox.Should().NotBeNull("Search box should be present");
            searchBox.Displayed.Should().BeTrue("Search box should be visible");
        }

        [Fact]
        public void HomePageShouldDisplayCallToActionButtons()
        {
            // Arrange & Act
            NavigateToUrl("/");

            // Assert
            WaitForPageToLoad();

            // Look for common CTA buttons
            var ctaButtons = Driver.FindElements(By.CssSelector(".btn-primary, .btn-success, .btn-warning"));
            ctaButtons.Should().NotBeEmpty("Call to action buttons should be present");

            foreach (var button in ctaButtons)
            {
                button.Displayed.Should().BeTrue("CTA button should be visible");
                button.Enabled.Should().BeTrue("CTA button should be enabled");
            }
        }
    }
}
