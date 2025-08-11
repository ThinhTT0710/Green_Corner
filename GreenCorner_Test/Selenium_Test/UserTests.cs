using FluentAssertions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenCorner_Test.Selenium_Test
{
    public class UserTests : IClassFixture<SeleniumFixture>
    {
        private readonly SeleniumFixture _fixture;

        public UserTests(SeleniumFixture fixture)
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
        public void Achievements()
        {
            // Arrange
            LoginUser("qgbeo711@gmail.com", "Nam@12345");
            _fixture.NavigateToUrl("/User/Achievements");
            _fixture.WaitForPageToLoad();
        }
        [Fact]
        public void ChangePassword()
        {
            // Arrange
            LoginUser("qgbeo711@gmail.com", "Nam@12345");
            _fixture.NavigateToUrl("/User/ChangePassword");
            _fixture.WaitForPageToLoad();
        }
        [Fact]
        public void GetRewardRedemptionHistory()
        {
            // Arrange
            LoginUser("qgbeo711@gmail.com", "Nam@12345");
            _fixture.NavigateToUrl("/User/GetRewardRedemptionHistory");
            _fixture.WaitForPageToLoad();
            var header = _fixture.WaitForElement(By.CssSelector("h3.mb-0"));
            header.Text.Should().Be("Lịch sử đổi thưởng");
        }
        [Fact]
        public void GetUserRewardRedemptionHistory()
        {
            // Arrange
            LoginUser("nampdce172019@fpt.edu.vn", "Nam@123");
            _fixture.NavigateToUrl("/User/GetUserRewardRedemptionHistory?userId=dec654cb-b4f4-4e08-820f-c152bc3d81f1");
            _fixture.WaitForPageToLoad();
            var header = _fixture.WaitForElement(By.CssSelector("h3.mb-0"));
            header.Text.Should().Be("Lịch sử đổi thưởng của người dùng");
        }
        [Fact]
        public void Profile()
        {
            // Arrange
            LoginUser("qgbeo711@gmail.com", "Nam@12345");
            _fixture.NavigateToUrl("/User/Profile");
            _fixture.WaitForPageToLoad();
        }
        [Fact]
        public void ReportHistory()
        {
            // Arrange
            LoginUser("qgbeo711@gmail.com", "Nam@12345");
            _fixture.NavigateToUrl("/User/ReportHistory");
            _fixture.WaitForPageToLoad();
        }
        [Fact]
        public void UpdateProfile()
        {
            // Arrange
            LoginUser("qgbeo711@gmail.com", "Nam@12345");
            _fixture.NavigateToUrl("/User/UpdateProfile");
            _fixture.WaitForPageToLoad();
        }
        [Fact]
        public void ViewActivities()
        {
            // Arrange
            LoginUser("nampdce172019@fpt.edu.vn", "Nam@123");
            _fixture.NavigateToUrl("/User/ViewActivities?userId=dec654cb-b4f4-4e08-820f-c152bc3d81f1");
            _fixture.WaitForPageToLoad();
            var header = _fixture.WaitForElement(By.CssSelector("h2.content-title"));
            header.Text.Should().Be("Lịch sử hoạt động");
        }
        [Fact]
        public void ViewParticipatedActivities()
        {
            // Arrange
            LoginUser("qgbeo711@gmail.com", "Nam@12345");
            _fixture.NavigateToUrl("/User/ViewParticipatedActivities");
            _fixture.WaitForPageToLoad();
            var header = _fixture.WaitForElement(By.CssSelector("h2.content-title"));
            header.Text.Should().Be("Hoạt động đã đăng ký");
        }
    }
}
