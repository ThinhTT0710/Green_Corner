using FluentAssertions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenCorner_Test.Selenium_Test
{
    public class OrderTests : IClassFixture<SeleniumFixture>
    {

        private readonly SeleniumFixture _fixture;

        public OrderTests(SeleniumFixture fixture)
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
        public void History()
        {
            // Arrange
            LoginUser("qgbeo711@gmail.com", "Nam@12345");
            _fixture.NavigateToUrl("/Order/History");
            _fixture.WaitForPageToLoad();
        }
        [Fact]
        public void Index()
        {
            // Arrange
            LoginUser("qgbeo711@gmail.com", "Nam@12345");
            _fixture.NavigateToUrl("/Order");
            _fixture.WaitForPageToLoad();
          
        }
        [Fact]
        public void ListOrder()
        {
            // Arrange
            LoginUser("nampdce172019@fpt.edu.vn", "Nam@123");
            _fixture.NavigateToUrl("/Order/ListOrder");
            _fixture.WaitForPageToLoad();
            var emptyMessage = _fixture.WaitForElement(By.CssSelector("h2.content-title"));

            // Assert
            emptyMessage.Text.Should().Be("Danh sách đơn hàng");
        }
        [Fact]
        public void OrderCODComplete()
        {
            // Arrange
            LoginUser("qgbeo711@gmail.com", "Nam@12345");
            _fixture.NavigateToUrl("/Order/OrderCODComplete");
            _fixture.WaitForPageToLoad();
        }
        [Fact]
        public void OrderComplete()
        {
            // Arrange
            LoginUser("qgbeo711@gmail.com", "Nam@12345");
            _fixture.NavigateToUrl("/Order/OrderComplete");
            _fixture.WaitForPageToLoad();
        }
        [Fact]
        public void OrderDetail()
        {
            // Arrange
            LoginUser("nampdce172019@fpt.edu.vn", "Nam@123");
            _fixture.NavigateToUrl("/Order/OrderDetail?id=2");
            _fixture.WaitForPageToLoad();
            var emptyMessage = _fixture.WaitForElement(By.CssSelector("h2.content-title"));

            // Assert
            emptyMessage.Text.Should().Be("Chi tiết đơn hàng");
        }
        [Fact]
        public void OrderFail()
        {
            // Arrange
            LoginUser("qgbeo711@gmail.com", "Nam@12345");
            _fixture.NavigateToUrl("/Order/OrderFail");
            _fixture.WaitForPageToLoad();
        }
    }
}
