using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenCorner_Test.Selenium_Test
{
    public class CartTests : IClassFixture<SeleniumFixture>
    {
        private readonly SeleniumFixture _fixture;

        public CartTests(SeleniumFixture fixture)
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
        public void MyCartView_ShouldShowEmptyMessage_WhenCartIsEmpty()
        {
            // Arrange
            LoginUser("qgbeo711@gmail.com", "Nam@12345");
            _fixture.NavigateToUrl("/Cart");

            // Act
            var emptyMessage = _fixture.WaitForElement(By.CssSelector("h2.mb-20"));

            // Assert
            emptyMessage.Text.Should().Be("Giỏ hàng của bạn hiện đang trống");
        }
        
    }
}
