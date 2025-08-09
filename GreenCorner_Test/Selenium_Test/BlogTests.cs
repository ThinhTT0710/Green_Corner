using OpenQA.Selenium;
using FluentAssertions;
using DocumentFormat.OpenXml.Bibliography;
using GreenCorner_Test.Selenium_Test;
using OpenQA.Selenium.Support.UI;

namespace GreenCorner_Test.Selenium_Test
{
    public class BlogTests : BaseSeleniumTest
    {
        [Fact]
        public void UserCanBrowseBlogPosts()
        {
            // Arrange & Act
            NavigateToUrl("/BlogPost");

            // Assert
            WaitForPageToLoad();
            var pageTitle = Driver.Title;
            pageTitle.Should().Contain("Góc chia sẻ", "Page title should contain 'Blog'");

            var blogContainer = WaitForElement(By.Id("blogContainer"));
            blogContainer.Should().NotBeNull("Blog container should be present");
        }

        [Fact]
        public void BlogPageShouldDisplayBlogItems()
        {
            // Arrange & Act
            NavigateToUrl("/BlogPost");

            // Assert
            WaitForPageToLoad();

            // Chờ blog-item xuất hiện tối đa 10 giây
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            var blogItems = wait.Until(d =>
            {
                var elements = d.FindElements(By.ClassName("blog-item"));
                return elements.Count > 0 ? elements : null;
            });

            blogItems.Should().NotBeEmpty("Blog items should be present");

            // Check first blog item has required elements
            var firstBlog = blogItems.First();

            var blogTitle = firstBlog.FindElement(By.ClassName("post-title"));
            var blogExcerpt = firstBlog.FindElement(By.ClassName("post-exerpt"));
            var blogDate = firstBlog.FindElement(By.ClassName("blog-date"));
            var readMoreButton = firstBlog.FindElement(By.ClassName("btn"));

            blogTitle.Should().NotBeNull("Blog title should be displayed");
            blogExcerpt.Should().NotBeNull("Blog excerpt should be displayed");
            blogDate.Should().NotBeNull("Blog date should be displayed");
            readMoreButton.Should().NotBeNull("Read more button should be present");
        }


        [Fact]
        public void UserCanReadBlogPost()
        {
            // Arrange
            NavigateToUrl("/BlogPost");

            // Act
            WaitForPageToLoad();
            var blogLinks = Driver.FindElements(By.ClassName("blog-link"));

            if (blogLinks.Any())
            {
                blogLinks.First().Click();
                WaitForPageToLoad();

                // Assert
                var blogDetail = WaitForElement(By.Id("blogDetail"));
                blogDetail.Should().NotBeNull("Blog detail section should be present");

                // Check for required elements
                var blogTitle = Driver.FindElement(By.ClassName("blog-detail-title"));
                var blogContent = Driver.FindElement(By.ClassName("blog-detail-content"));
                var blogAuthor = Driver.FindElement(By.ClassName("blog-detail-author"));
                var blogDate = Driver.FindElement(By.ClassName("blog-detail-date"));

                blogTitle.Should().NotBeNull("Blog title should be displayed");
                blogContent.Should().NotBeNull("Blog content should be displayed");
                blogAuthor.Should().NotBeNull("Blog author should be displayed");
                blogDate.Should().NotBeNull("Blog date should be displayed");
            }
        }

        [Fact]
        public void UserCanSearchForBlogPosts()
        {
            // Arrange
            NavigateToUrl("/BlogPost");

            // Act
            WaitForPageToLoad();
            var searchInput = WaitForElement(By.Id("blogSearchInput"));
            var searchButton = WaitForElementToBeClickable(By.Id("blogSearchButton"));

            searchInput.SendKeys("environment");
            searchButton.Click();

            // Assert
            WaitForPageToLoad();
            var searchResults = Driver.FindElements(By.ClassName("blog-card"));
            searchResults.Should().NotBeEmpty("Search should return results");
        }

        [Fact]
        public void UserCanFilterBlogPostsByCategory()
        {
            // Arrange
            NavigateToUrl("/BlogPost");

            // Act
            WaitForPageToLoad();
            var categoryFilters = Driver.FindElements(By.ClassName("blog-category-filter"));

            if (categoryFilters.Any())
            {
                var firstCategory = categoryFilters.First();
                firstCategory.Click();

                // Assert
                WaitForPageToLoad();
                var filteredPosts = Driver.FindElements(By.ClassName("blog-card"));
                filteredPosts.Should().NotBeEmpty("Filtered blog posts should be displayed");
            }
        }

        [Fact]
        public void UserCanLikeBlogPost()
        {
            // Arrange
            NavigateToUrl("/BlogPost");

            // Act
            WaitForPageToLoad();
            var blogLinks = Driver.FindElements(By.ClassName("blog-link"));

            if (blogLinks.Any())
            {
                blogLinks.First().Click();
                WaitForPageToLoad();

                var likeButton = WaitForElementToBeClickable(By.ClassName("like-blog-btn"));
                var initialLikeCount = likeButton.Text;
                likeButton.Click();

                // Assert
                WaitForElement(By.ClassName("like-notification"));
                var likeNotification = Driver.FindElement(By.ClassName("like-notification"));
                likeNotification.Displayed.Should().BeTrue("Like notification should be displayed");
            }
        }

        [Fact]
        public void UserCanCommentOnBlogPost()
        {
            // Arrange
            NavigateToUrl("/BlogPost");

            // Act
            WaitForPageToLoad();
            var blogLinks = Driver.FindElements(By.ClassName("blog-link"));

            if (blogLinks.Any())
            {
                blogLinks.First().Click();
                WaitForPageToLoad();

                var commentInput = WaitForElement(By.Id("commentInput"));
                var submitCommentButton = WaitForElementToBeClickable(By.Id("submitComment"));

                commentInput.SendKeys("Great article! Very informative.");
                submitCommentButton.Click();

                // Assert
                WaitForElement(By.ClassName("comment-success"));
                var commentSuccess = Driver.FindElement(By.ClassName("comment-success"));
                commentSuccess.Displayed.Should().BeTrue("Comment success message should be displayed");
            }
        }

        [Fact]
        public void UserCanShareBlogPost()
        {
            // Arrange
            NavigateToUrl("/BlogPost");

            // Act
            WaitForPageToLoad();
            var blogLinks = Driver.FindElements(By.ClassName("blog-link"));

            if (blogLinks.Any())
            {
                blogLinks.First().Click();
                WaitForPageToLoad();

                var shareButton = WaitForElementToBeClickable(By.ClassName("share-blog-btn"));
                shareButton.Click();

                // Assert
                var shareModal = WaitForElement(By.Id("shareBlogModal"));
                shareModal.Should().NotBeNull("Share modal should be displayed");
                shareModal.Displayed.Should().BeTrue("Share modal should be visible");
            }
        }

        [Fact]
        public void BlogPageShouldDisplayFeaturedPosts()
        {
            // Arrange & Act
            NavigateToUrl("/BlogPost");

            // Assert
            WaitForPageToLoad();

            var featuredPostsSection = Driver.FindElement(By.Id("featuredPosts"));
            featuredPostsSection.Should().NotBeNull("Featured posts section should be present");
            featuredPostsSection.Displayed.Should().BeTrue("Featured posts section should be visible");
        }

        [Fact]
        public void UserCanViewRelatedPosts()
        {
            // Arrange
            NavigateToUrl("/BlogPost");

            // Act
            WaitForPageToLoad();
            var blogLinks = Driver.FindElements(By.ClassName("blog-link"));

            if (blogLinks.Any())
            {
                blogLinks.First().Click();
                WaitForPageToLoad();

                // Assert
                var relatedPostsSection = Driver.FindElement(By.Id("relatedPosts"));
                relatedPostsSection.Should().NotBeNull("Related posts section should be present");
                relatedPostsSection.Displayed.Should().BeTrue("Related posts section should be visible");
            }
        }

        [Fact]
        public void BlogPageShouldHavePagination()
        {
            // Arrange & Act
            NavigateToUrl("/BlogPost");

            // Assert
            WaitForPageToLoad();

            var pagination = Driver.FindElements(By.ClassName("pagination"));
            if (pagination.Any())
            {
                var firstPagination = pagination.First();
                firstPagination.Displayed.Should().BeTrue("Pagination should be visible when there are multiple pages");
            }
        }

        [Fact]
        public void UserCanSortBlogPostsByDate()
        {
            // Arrange
            NavigateToUrl("/BlogPost");

            // Act
            WaitForPageToLoad();
            var sortDropdown = Driver.FindElements(By.Id("blogSortDropdown"));

            if (sortDropdown.Any())
            {
                var dropdown = sortDropdown.First();
                dropdown.Click();

                var dateOption = WaitForElementToBeClickable(By.CssSelector("option[value='date']"));
                dateOption.Click();

                // Assert
                WaitForPageToLoad();
                var blogCards = Driver.FindElements(By.ClassName("blog-card"));
                blogCards.Should().NotBeEmpty("Blog posts should be displayed after sorting");
            }
        }

        [Fact]
        public void UserCanViewBlogAuthorProfile()
        {
            // Arrange
            NavigateToUrl("/BlogPost");

            // Act
            WaitForPageToLoad();
            var blogLinks = Driver.FindElements(By.ClassName("blog-link"));

            if (blogLinks.Any())
            {
                blogLinks.First().Click();
                WaitForPageToLoad();

                var authorLink = WaitForElementToBeClickable(By.ClassName("blog-author-link"));
                authorLink.Click();

                // Assert
                WaitForPageToLoad();
                var currentUrl = Driver.Url;
                currentUrl.Should().Contain("Author", "Should navigate to author profile page");
            }
        }

        [Fact]
        public void BlogPostShouldDisplayReadingTime()
        {
            // Arrange
            NavigateToUrl("/BlogPost");

            // Act
            WaitForPageToLoad();
            var blogLinks = Driver.FindElements(By.ClassName("blog-link"));

            if (blogLinks.Any())
            {
                blogLinks.First().Click();
                WaitForPageToLoad();

                // Assert
                var readingTime = Driver.FindElement(By.ClassName("reading-time"));
                readingTime.Should().NotBeNull("Reading time should be displayed");
            }
        }

        [Fact]
        public void UserCanSubscribeToBlog()
        {
            // Arrange
            NavigateToUrl("/BlogPost");

            // Act
            WaitForPageToLoad();
            var subscribeInput = WaitForElement(By.Id("subscribeEmail"));
            var subscribeButton = WaitForElementToBeClickable(By.Id("subscribeButton"));

            subscribeInput.SendKeys("test@example.com");
            subscribeButton.Click();

            // Assert
            WaitForElement(By.ClassName("subscribe-success"));
            var subscribeSuccess = Driver.FindElement(By.ClassName("subscribe-success"));
            subscribeSuccess.Displayed.Should().BeTrue("Subscribe success message should be displayed");
        }

        [Fact]
        public void BlogPageShouldDisplayTags()
        {
            // Arrange
            NavigateToUrl("/BlogPost");

            // Act
            WaitForPageToLoad();
            var blogLinks = Driver.FindElements(By.ClassName("blog-link"));

            if (blogLinks.Any())
            {
                blogLinks.First().Click();
                WaitForPageToLoad();

                // Assert
                var tagsSection = Driver.FindElement(By.ClassName("blog-tags"));
                tagsSection.Should().NotBeNull("Blog tags should be displayed");
                tagsSection.Displayed.Should().BeTrue("Blog tags should be visible");
            }
        }
    }
}
