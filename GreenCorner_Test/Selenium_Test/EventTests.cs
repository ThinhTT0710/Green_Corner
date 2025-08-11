using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace GreenCorner_Test.Selenium_Test
{
    public class EventTests : IClassFixture<SeleniumFixture>
    {
        private readonly SeleniumFixture _fixture;

        public EventTests(SeleniumFixture fixture)
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
        public void CreateEvent_ShouldCreateSuccessfully_WhenValidData()
        {
            // Arrange
            LoginUser("nampdce172019@fpt.edu.vn", "Nam@123");

            _fixture.NavigateToUrl("/Event/CreateCleanupEvent");

            var header = _fixture.WaitForElement(By.CssSelector("h2.content-title"));
            header.Text.Should().Be("Tạo sự kiện mới");

        }
        [Fact]
        public void DeleteEventReview()
        {
            // Arrange
            LoginUser("qgbeo711@gmail.com", "Nam@12345");

            _fixture.NavigateToUrl("/Event/DeleteEventReview?eventReviewId=7");

            var header = _fixture.WaitForElement(By.CssSelector("h3.mb-15"));
            header.Text.Should().Be("Xóa đánh giá sự kiện");

        }
        [Fact]
        public void DeleteLeaderReview()
        {
            // Arrange
            LoginUser("qgbeo711@gmail.com", "Nam@12345");

            _fixture.NavigateToUrl("/Event/DeleteLeaderReview?leaderReviewId=3");
            _fixture.WaitForPageToLoad();
            var header = _fixture.WaitForElement(By.CssSelector("h3.mb-15"));
            header.Text.Should().Be("Xóa đánh giá đội trưởng");

        }
        [Fact]
        public void EditEventReview()
        {
            // Arrange
            LoginUser("qgbeo711@gmail.com", "Nam@12345");

            _fixture.NavigateToUrl("/Event/EditEventReview?eventReviewId=7");

            var header = _fixture.WaitForElement(By.CssSelector("h3.mb-15"));
            header.Text.Should().Be("Cập nhật đánh giá sự kiện");
        }
        [Fact]
        public void EventReviewHistory()
        {
            // Arrange
            LoginUser("qgbeo711@gmail.com", "Nam@12345");

            _fixture.NavigateToUrl("/Event/EventReviewHistory");

            var header = _fixture.WaitForElement(By.CssSelector("h2.content-title"));
            header.Text.Should().Be("Đánh giá sự kiện");
        }

        [Fact]
        public void GetAllEvent()
        {
            // Arrange
            LoginUser("nampdce172019@fpt.edu.vn", "Nam@123");

            _fixture.NavigateToUrl("/Event/GetAllEvent");
            _fixture.WaitForPageToLoad();
            var header = _fixture.WaitForElement(By.CssSelector("h2.content-title"));
            
            header.Text.Should().Be("Danh sách sự kiện");
        }
        [Fact]
        public void GetEventById()
        {
            // Arrange
            LoginUser("qgbeo711@gmail.com", "Nam@12345");

            _fixture.NavigateToUrl("/Event/GetEventById?eventId=3");
            _fixture.WaitForPageToLoad();
            //var header = _fixture.WaitForElement(By.CssSelector("h2.content-title"));

            //header.Text.Should().Be("Chi tiết sự kiện");
        }
        [Fact]
        public void GetOpenEventsByTeamLeader()
        {
            // Arrange
            LoginUser("qgbeo711@gmail.com", "Nam@12345");

            _fixture.NavigateToUrl("/Event/GetOpenEventsByTeamLeader");
            _fixture.WaitForPageToLoad();
            //var header = _fixture.WaitForElement(By.CssSelector("h1.cdisplay-2"));

            //header.Text.Should().Be("Sự kiện bạn là đội trưởng");
        }
        [Fact]
        public void Index()
        {
            // Arrange
            LoginUser("qgbeo711@gmail.com", "Nam@12345");

            _fixture.NavigateToUrl("/Event");
            _fixture.WaitForPageToLoad();
            //var header = _fixture.WaitForElement(By.CssSelector("h1.cdisplay-2"));

            //header.Text.Should().Be("Danh sách sự kiện");
        }
        [Fact]
        public void RegisterTeamLeader()
        {
            // Arrange
            LoginUser("qgbeo711@gmail.com", "Nam@12345");

            _fixture.NavigateToUrl("/Event/RegisterTeamLeader?eventId=2");
            _fixture.WaitForPageToLoad();
            var header = _fixture.WaitForElement(By.CssSelector("h2.content-title"));
            header.Text.Should().Be("Đăng ký Team Leader");
        }
        [Fact]
        public void RegisterVolunteer()
        {
            // Arrange
            LoginUser("qgbeo711@gmail.com", "Nam@12345");

            _fixture.NavigateToUrl("/Event/RegisterVolunteer?eventId=4");
            _fixture.WaitForPageToLoad();
            var header = _fixture.WaitForElement(By.CssSelector("h2.content-title"));
            header.Text.Should().Be("Đăng ký tình nguyện cho sự kiện");
        }
        [Fact]
        public void UpdateEvent()
        {
            // Arrange
            LoginUser("nampdce172019@fpt.edu.vn", "Nam@123");

            _fixture.NavigateToUrl("/Event/UpdateCleanupEvent?eventId=1");

            var header = _fixture.WaitForElement(By.CssSelector("h2.content-title"));
            header.Text.Should().Be("Cập nhật sự kiện");

        }
        [Fact]
        public void ViewEventVolunteerListCheck()
        {
            // Arrange
            LoginUser("nampdce172019@fpt.edu.vn", "Nam@123");

            _fixture.NavigateToUrl("/Event/ViewEventVolunteerListCheck?eventId=3");

            

        }
        [Fact]
        public void ViewEventVolunteerList()
        {
            // Arrange
            LoginUser("nampdce172019@fpt.edu.vn", "Nam@123");

            _fixture.NavigateToUrl("/Event/ViewEventVolunteerList?eventId=3");

            var header = _fixture.WaitForElement(By.CssSelector("h2.content-title"));
            header.Text.Should().Be("Danh sách tình nguyện viên");

        }

    }
}

