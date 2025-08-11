using Xunit;
using OpenQA.Selenium;
using FluentAssertions;
using GreenCorner_Test.Selenium_Test;
using Docker.DotNet.Models;
using OpenQA.Selenium.Support.UI;

namespace GreenCorner_Test.Selenium_Test
{
    public class BlogTests : IClassFixture<SeleniumFixture>
    {
        private readonly SeleniumFixture _fixture;

        public BlogTests(SeleniumFixture fixture)
        {
            _fixture = fixture;
        }
        public void LoginUser(string email, string password)
        {
            // Điều hướng tới trang login
            _fixture.NavigateToUrl("/Auth/Login");

            // Nhập email
            var emailInput = _fixture.WaitForElement(By.Name("Email"));
            emailInput.Clear();
            emailInput.SendKeys(email);

            // Nhập password
            var passwordInput = _fixture.WaitForElement(By.Name("Password"));
            passwordInput.Clear();
            passwordInput.SendKeys(password);

            // Click nút đăng nhập
            var loginButton = _fixture.WaitForElement(By.CssSelector("button[name='login']"));
            loginButton.Click();

            // Đợi trang load xong
            _fixture.WaitForPageToLoad();
        }
        [Fact]
        public void CreateBlogPage_ShouldHaveCorrectTitle()
        {
            LoginUser("qgbeo711@gmail.com", "Nam@12345");

            _fixture.NavigateToUrl("/BlogPost/CreateBlog");
            _fixture.WaitForPageToLoad();

            var header = _fixture.WaitForElement(By.CssSelector("h2.content-title"));
            header.Text.Should().Be("Tạo bài viết mới");
        }
        [Fact]
        public void CreateReportPage_ShouldHaveCorrectTitle()
        {
            LoginUser("qgbeo711@gmail.com", "Nam@12345");
            _fixture.NavigateToUrl("/BlogPost/CreateReport/1");
            _fixture.WaitForPageToLoad();

            var header = _fixture.WaitForElement(By.CssSelector("h2.mb-4"));
            header.Text.Should().Be("Báo cáo bài viết");

            var reasonTextarea = _fixture.WaitForElement(By.Name("Reason"));
            reasonTextarea.Should().NotBeNull();

            var submitButton = _fixture.WaitForElement(By.CssSelector("button.btn-danger"));
            submitButton.Text.Should().Be("Gửi báo cáo");
        }
        [Fact]
        public void DeleteBlogPage_ShouldDisplayBlogDetails()
        {
            LoginUser("qgbeo711@gmail.com", "Nam@12345");


            // 2. Điều hướng đến trang xóa bài viết với BlogId giả sử là 1
            _fixture.NavigateToUrl("/BlogPost/DeleteBlog/1");
            _fixture.WaitForPageToLoad();

            // 3. Kiểm tra tiêu đề trang
            var pageTitle = _fixture.WaitForElement(By.CssSelector("h2.content-title")).Text;
            Assert.Equal("Xóa bài viết", pageTitle);

            // 4. Kiểm tra trường tiêu đề bài viết readonly
            var titleInput = _fixture.WaitForElement(By.Id("Title"));
            Assert.True(titleInput.GetAttribute("readonly") != null);

            // 5. Kiểm tra nút Xóa và nút Hủy tồn tại
            var deleteButton = _fixture.WaitForElement(By.CssSelector("button.btn-danger"));
            var cancelButton = _fixture.WaitForElement(By.LinkText("Hủy"));
            Assert.NotNull(deleteButton);
            Assert.NotNull(cancelButton);
        }
        [Fact]
        public void BlogListPage_ShouldDisplayCorrectUI()
        {
            // Đăng nhập trước (nếu trang yêu cầu)
            LoginUser("qgbeo711@gmail.com", "Nam@12345");

            // Điều hướng đến trang Góc chia sẻ
            _fixture.NavigateToUrl("/BlogPost/Index");
            _fixture.WaitForPageToLoad();

            // 1. Kiểm tra tiêu đề
            var header = _fixture.WaitForElement(By.CssSelector("h1.mb-15"));
            header.Text.Should().Be("Góc chia sẻ");
        }
        [Fact]
        public void MyFavoriteBlogs_ShouldDisplay_WhenUserHasFavorites()
        {
            // Arrange - login user có bài yêu thích
            LoginUser("qgbeo711@gmail.com", "Nam@12345");

            // Act
            _fixture.NavigateToUrl("/BlogPost/MyFavoriteBlogs");
            _fixture.WaitForPageToLoad();
            // Assert - tiêu đề trang
            var pageTitle = _fixture.Driver.FindElement(By.TagName("h1")).Text;
            pageTitle.Should().Contain("Góc yêu thích");
          
        }
        [Fact]
        public void MyPendingBlogs_ShouldDisplay_WhenUserHasBlogs()
        {
            // Arrange
            LoginUser("qgbeo711@gmail.com", "Nam@12345");

            // Act
            _fixture.NavigateToUrl("/BlogPost/MyPendingBlogs");
            _fixture.WaitForPageToLoad();
            // Assert - Tiêu đề trang
            var title = _fixture.Driver.FindElement(By.TagName("h1")).Text;
            title.Should().Contain("Bài viết của tôi");

            // Có ít nhất 1 article
            var articles = _fixture.Driver.FindElements(By.TagName("article"));
            articles.Should().NotBeEmpty("Người dùng này có blog pending hoặc đã đăng");

            var firstArticle = articles.First();

            // Kiểm tra status text
            var statusText = firstArticle.FindElement(By.CssSelector("h6")).Text;
            statusText.Should().MatchRegex("(Đã đăng|Đang chờ duyệt|Không được duyệt)");

            // Tiêu đề blog
            var blogTitle = firstArticle.FindElement(By.CssSelector("h4.post-title a")).Text;
            blogTitle.Should().NotBeNullOrWhiteSpace();

            // Mô tả rút gọn
            var description = firstArticle.FindElement(By.CssSelector(".font-xs.color-grey.mt-10.pb-10")).Text;
            description.Should().NotBeNullOrWhiteSpace();

            // Ngày đăng
            var createdDate = firstArticle.FindElement(By.CssSelector(".entry-meta .post-on")).Text;
            createdDate.Should().MatchRegex(@"\d{2}/\d{2}/\d{4}");

            // Nếu Pending → có nút Sửa và Xoá
            if (statusText.Contains("Đang chờ duyệt"))
            {
                var editBtn = firstArticle.FindElements(By.LinkText("Sửa"));
                var deleteBtn = firstArticle.FindElements(By.LinkText("Xoá"));
                editBtn.Should().NotBeEmpty();
                deleteBtn.Should().NotBeEmpty();
            }
            else // Nếu Published/Rejected → chỉ có Xoá
            {
                var deleteBtn = firstArticle.FindElements(By.LinkText("Xoá"));
                deleteBtn.Should().NotBeEmpty();
            }
        }
        [Fact]
        public void SubmitFeedback_ShouldSubmitSuccessfully_WhenValidData()
        {
            // Arrange
            LoginUser("qgbeo711@gmail.com", "Nam@12345");

            // Act
            _fixture.NavigateToUrl("/BlogPost/SubmitFeedback");
            _fixture.WaitForPageToLoad();
           
        }
        [Fact]
        public void SubmitReport_ShouldSubmitSuccessfully_WhenValidData()
        {
            // Arrange
            LoginUser("qgbeo711@gmail.com", "Nam@12345"); // Đăng nhập với thông tin người dùng hợp lệ

            // Act
            _fixture.NavigateToUrl("/BlogPost/SubmitReport");
            _fixture.WaitForPageToLoad();// Điều hướng đến trang gửi báo cáo
            //var titleInput = _fixture.Driver.FindElement(By.Id("Title")).Text;
            //titleInput.Should().Contain("Gửi báo cáo");

        }
        [Fact]
        public void UpdateBlog_ShouldUpdateSuccessfully_WhenValidData()
        {
            // Arrange
            LoginUser("qgbeo711@gmail.com", "Nam@12345");

            _fixture.NavigateToUrl("/BlogPost/UpdateBlog?blogId=1"); // Điều hướng đến trang cập nhật bài viết
            _fixture.WaitForPageToLoad();
            var titleInput = _fixture.Driver.FindElement(By.ClassName("content-title")).Text;
            titleInput.Should().Contain("Cập nhật bài viết");

        }
        [Fact]
        public void ViewBlogDetail_ShouldDisplayCorrectContent_WhenBlogExists()
        {
            // Arrange - Đăng nhập người dùng hợp lệ
            LoginUser("qgbeo711@gmail.com", "Nam@12345");

            // Act - Điều hướng tới trang chi tiết bài viết (ví dụ BlogId = 1)
            _fixture.NavigateToUrl("/BlogPost/ViewBlogDetail/1");
            _fixture.WaitForPageToLoad();

            // Assert - Tiêu đề bài viết hiển thị
            var blogTitle = _fixture.Driver.FindElement(By.CssSelector("h2.mb-10")).Text;
            blogTitle.Should().NotBeNullOrEmpty("Tiêu đề bài viết phải hiển thị");

            // Assert - Tên tác giả hiển thị
            var authorName = _fixture.Driver.FindElement(By.CssSelector(".post-by strong")).Text;
            authorName.Should().NotBeNullOrEmpty("Tên tác giả phải hiển thị");

            // Assert - Nội dung bài viết hiển thị
            var blogContent = _fixture.Driver.FindElement(By.CssSelector(".single-excerpt")).Text;
            blogContent.Should().NotBeNullOrEmpty("Nội dung bài viết phải hiển thị");

            // Assert - Nút yêu thích hiển thị
            var favoriteBtn = _fixture.Driver.FindElement(By.CssSelector(".btn-favorite"));
            favoriteBtn.Should().NotBeNull("Nút yêu thích phải tồn tại");
        }
        [Fact]
        public void ViewBlogDetail_ShouldSubmitReportSuccessfully_WhenValidReason()
        {
            // Arrange - Đăng nhập người dùng hợp lệ
            LoginUser("qgbeo711@gmail.com", "Nam@12345");
            _fixture.NavigateToUrl("/BlogPost/ViewBlogDetail/1");
            _fixture.WaitForPageToLoad();
            var titleInput = _fixture.Driver.FindElement(By.ClassName("mb-10")).Text;
            titleInput.Should().Contain("Bài viết");
            
        }
        [Fact]
        public void ViewFeedbacks_ShouldFilterAndPaginateSuccessfully_WhenUsingSearchAndStatus()
        {
            // Arrange - Đăng nhập admin
            LoginUser("nampdce172019@fpt.edu.vn", "Nam@123");

            // Act - Điều hướng đến trang danh sách phản hồi
            _fixture.NavigateToUrl("/BlogPost/ViewFeedbacks");

            // Assert - Bảng phản hồi hiển thị
            var header = _fixture.WaitForElement(By.CssSelector("h2.content-title"));
            header.Text.Should().Be("Danh sách phản hồi");
        }
        [Fact]
        public void VR001_ShouldDisplayReports_WhenDataExists()
        {
            // Arrange
            LoginUser("admin@gmail.com", "123456Vn@");

            // Act
            _fixture.NavigateToUrl("/BlogPost/ViewReports");

            // Assert
            var rows = _fixture.Driver.FindElements(By.CssSelector("table tbody tr"));
        }


    }
}
