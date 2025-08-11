using FluentAssertions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenCorner_Test.Selenium_Test
{
    public class PointTransactionTests : IClassFixture<SeleniumFixture>
    {
        private readonly SeleniumFixture _fixture;

        public PointTransactionTests(SeleniumFixture fixture)
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
        public void PointTransaction()
        {
            // Arrange
            LoginUser("nampdce172019@fpt.edu.vn", "Nam@123");
            _fixture.NavigateToUrl("/PointTransaction/PointTransaction");
            _fixture.WaitForPageToLoad();
        }
        [Fact]
        public void PointTransactionList()
        {
            // Arrange
            LoginUser("nampdce172019@fpt.edu.vn", "Nam@123");
            _fixture.NavigateToUrl("/PointTransaction/PointTransactionList");
            _fixture.WaitForPageToLoad();
            var header = _fixture.WaitForElement(By.CssSelector("h2.content-title"));
            header.Text.Should().Be("Danh sách lịch sử trao đổi");
        }
        [Fact]
        public void UserTransaction()
        {
            // Arrange
            LoginUser("qgbeo711@gmail.com", "Nam@12345");
            _fixture.NavigateToUrl("/PointTransaction/UserTransactions");
            _fixture.WaitForPageToLoad();
            var header = _fixture.WaitForElement(By.CssSelector("h2"));
            header.Text.Should().Be("Lịch sử giao dịch điểm");
        }
        [Fact]
        public void ViewTransaction()
        {
            // Arrange
            LoginUser("nampdce172019@fpt.edu.vn", "Nam@123");
            _fixture.NavigateToUrl("/PointTransaction/ViewTransaction?userId=c9b3ed26-a681-4c29-ab72-b3a947c859d1");
            _fixture.WaitForPageToLoad();
            var header = _fixture.WaitForElement(By.CssSelector("h2"));
            header.Text.Should().Be("Transaction Details");
        }
        [Fact]
        public void PointTransactionList_ReturnValidList()
        {
            // Arrange
            LoginUser("nampdce172019@fpt.edu.vn", "Nam@123");
            _fixture.NavigateToUrl("/PointTransaction/PointTransactionList");
            _fixture.WaitForPageToLoad();
            var header = _fixture.WaitForElement(By.CssSelector("h2.content-title"));
            header.Text.Should().Be("Danh sách lịch sử trao đổi");
        }
    }
}
