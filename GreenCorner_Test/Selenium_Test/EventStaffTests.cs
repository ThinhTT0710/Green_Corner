using FluentAssertions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenCorner_Test.Selenium_Test
{
    public class EventStaffTests : IClassFixture<SeleniumFixture>
    {

        private readonly SeleniumFixture _fixture;

        public EventStaffTests(SeleniumFixture fixture)
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
        public void ViewPendingPostDetail()
        {
            // Arrange
            LoginUser("nampdce172019@fpt.edu.vn", "Nam@123");

            _fixture.NavigateToUrl("/EventStaff/ViewPendingPostDetail?id=14");
            _fixture.WaitForPageToLoad();
            var header = _fixture.WaitForElement(By.CssSelector("h2.content-title"));
            header.Text.Should().Be("Chi tiết bài viết");

        }
        [Fact]
        public void ViewPendingPost()
        {
            // Arrange
            LoginUser("nampdce172019@fpt.edu.vn", "Nam@123");

            _fixture.NavigateToUrl("/EventStaff/ViewPendingPosts");
            _fixture.WaitForPageToLoad();
            var header = _fixture.WaitForElement(By.CssSelector("h2.content-title"));
            header.Text.Should().Be("Danh sách bài viết");

        }
        [Fact]
        public void ViewPointsRewardHistory()
        {
            // Arrange
            LoginUser("nampdce172019@fpt.edu.vn", "Nam@123");

            _fixture.NavigateToUrl("/EventStaff/ViewPointsRewardHistory");
            _fixture.WaitForPageToLoad();
            var header = _fixture.WaitForElement(By.CssSelector("h2.content-title"));
            header.Text.Should().Be("Lịch sử thưởng điểm");

        }
        [Fact]
        public void ViewTeamLeaderRegistration()
        {
            // Arrange
            LoginUser("nampdce172019@fpt.edu.vn", "Nam@123");

            _fixture.NavigateToUrl("/EventStaff/ViewTeamLeaderRegistration");
            _fixture.WaitForPageToLoad();
            var header = _fixture.WaitForElement(By.CssSelector("h2.content-title"));
            header.Text.Should().Be("Danh sách đơn đăng ký làm Đội trưởng");

        }
        [Fact]
        public void ViewTeamLeaderRegistrationDetail()
        {
            // Arrange
            LoginUser("nampdce172019@fpt.edu.vn", "Nam@123");

            _fixture.NavigateToUrl("/EventStaff/ViewTeamLeaderRegistrationDetail?id=49");
            _fixture.WaitForPageToLoad();
            var header = _fixture.WaitForElement(By.CssSelector("h2.content-title"));
            header.Text.Should().Be("Chi tiết đăng ký đội trưởng");

        }
        [Fact]
        public void ViewUsersWithParticipation()
        {
            // Arrange
            LoginUser("nampdce172019@fpt.edu.vn", "Nam@123");

            _fixture.NavigateToUrl("/EventStaff/ViewUsersWithParticipation");
            _fixture.WaitForPageToLoad();
            var header = _fixture.WaitForElement(By.CssSelector("h2.content-title"));
            header.Text.Should().Be("Người dùng đã tham gia hoạt động");

        }
        [Fact]
        public void ViewVolunteerRegistrationDetail()
        {
            // Arrange
            LoginUser("nampdce172019@fpt.edu.vn", "Nam@123");

            _fixture.NavigateToUrl("/EventStaff/ViewVolunteerRegistrationDetail?id=48");
            _fixture.WaitForPageToLoad();
            var header = _fixture.WaitForElement(By.CssSelector("h2.content-title"));
            header.Text.Should().Be("Chi tiết đăng ký tình nguyện viên");

        }
        [Fact]
        public void ViewVolunteerRegistration()
        {
            // Arrange
            LoginUser("nampdce172019@fpt.edu.vn", "Nam@123");

            _fixture.NavigateToUrl("/EventStaff/ViewVolunteerRegistration");
            _fixture.WaitForPageToLoad();
            var header = _fixture.WaitForElement(By.CssSelector("h2.content-title"));
            header.Text.Should().Be("Danh sách đơn đăng ký tình nguyện viên");

        }
    }
}
