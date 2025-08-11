using FluentAssertions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenCorner_Test.Selenium_Test
{
    public class SaleStaffTests : IClassFixture<SeleniumFixture>
    {
        private readonly SeleniumFixture _fixture;

        public SaleStaffTests(SeleniumFixture fixture)
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
        public void ProductList()
        {
            // Arrange
            LoginUser("nampdce172019@fpt.edu.vn", "Nam@123");
            _fixture.NavigateToUrl("/SaleStaff/ProductList");
            _fixture.WaitForPageToLoad();
            var header = _fixture.WaitForElement(By.CssSelector("h2.content-title"));
            header.Text.Should().Be("Danh sách sản phẩm");
        }
        [Fact]
        public void ProductDetails()
        {
            // Arrange
            LoginUser("nampdce172019@fpt.edu.vn", "Nam@123");
            _fixture.NavigateToUrl("/SaleStaff/ProductDetails?productId=3");
            _fixture.WaitForPageToLoad();
            var header = _fixture.WaitForElement(By.CssSelector("h2.content-title"));
            header.Text.Should().Be("Chi tiết sản phẩm");
        }
        [Fact]
        public void OrderDetails()
        {
            // Arrange
            LoginUser("nampdce172019@fpt.edu.vn", "Nam@123");
            _fixture.NavigateToUrl("/SaleStaff/OrderDetails?id=1");
            _fixture.WaitForPageToLoad();
        }
        [Fact]
        public void ProductList_ReturnFullProduct()
        {
            // Arrange
            LoginUser("nampdce172019@fpt.edu.vn", "Nam@123");
            _fixture.NavigateToUrl("/SaleStaff/ProductList");
            _fixture.WaitForPageToLoad();
            var header = _fixture.WaitForElement(By.CssSelector("h2.content-title"));
            header.Text.Should().Be("Danh sách sản phẩm");
        }
    }
}
