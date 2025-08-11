using FluentAssertions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenCorner_Test.Selenium_Test
{
    public class TrashEventTests : IClassFixture<SeleniumFixture>
    {
        private readonly SeleniumFixture _fixture;

        public TrashEventTests(SeleniumFixture fixture)
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
        public void Index()
        {
            // Arrange
            LoginUser("nampdce172019@fpt.edu.vn", "Nam@123");
            _fixture.NavigateToUrl("/TrashEvent");
            _fixture.WaitForPageToLoad();
            var header = _fixture.WaitForElement(By.CssSelector("h2.content-title"));
            header.Text.Should().Be("Danh sách báo cáo");
        }
        [Fact]
        public void Detail()
        {
            // Arrange
            LoginUser("nampdce172019@fpt.edu.vn", "Nam@123");
            _fixture.NavigateToUrl("/TrashEvent/Detail?trashReportId=1");
            _fixture.WaitForPageToLoad();
            var header = _fixture.WaitForElement(By.CssSelector("h2.content-title"));
            header.Text.Contains("Chi tiết báo cáo");
        }
        [Fact]
        public void ReportTrashEvent()
        {
            // Arrange
            LoginUser("qgbeo711@gmail.com", "Nam@12345");
            _fixture.NavigateToUrl("/TrashEvent/ReportTrashEvent");
            _fixture.WaitForPageToLoad();
        }
        [Fact]
        public void UpdateTrashEvent()
        {
            // Arrange
            LoginUser("qgbeo711@gmail.com", "Nam@12345");
            _fixture.NavigateToUrl("/TrashEvent/UpdateTrashEvent?trashReportId=1");
            _fixture.WaitForPageToLoad();
        }
        [Fact]
        public void DeleteTrashEvent()
        {
            // Arrange
            LoginUser("qgbeo711@gmail.com", "Nam@12345");
            _fixture.NavigateToUrl("/TrashEvent/DeleteTrashEvent?trashReportId=1");
            _fixture.WaitForPageToLoad();
        }
    }
}
