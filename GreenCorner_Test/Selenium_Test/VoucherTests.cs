using FluentAssertions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenCorner_Test.Selenium_Test
{
    public class VoucherTests : IClassFixture<SeleniumFixture>
    {
        private readonly SeleniumFixture _fixture;

        public VoucherTests(SeleniumFixture fixture)
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
            _fixture.NavigateToUrl("/Voucher");
            _fixture.WaitForPageToLoad();
            var header = _fixture.WaitForElement(By.CssSelector("h2.content-title"));
            header.Text.Should().Be("Danh sách Voucher");
        }
        [Fact]
        public void Create()
        {
            // Arrange
            LoginUser("nampdce172019@fpt.edu.vn", "Nam@123");
            _fixture.NavigateToUrl("/Voucher/Create");
            _fixture.WaitForPageToLoad();
            var header = _fixture.WaitForElement(By.CssSelector("h2.content-title"));
            header.Text.Should().Be("Tạo Voucher mới");
        }
        [Fact]
        public void Detail()
        {
            // Arrange
            LoginUser("nampdce172019@fpt.edu.vn", "Nam@123");
            _fixture.NavigateToUrl("/Voucher/Detail?voucherId=1");
            _fixture.WaitForPageToLoad();
            var header = _fixture.WaitForElement(By.CssSelector("h2.content-title"));
            header.Text.Should().Be("Chi tiết voucher");
        }
        [Fact]
        public void Edit()
        {
            // Arrange
            LoginUser("nampdce172019@fpt.edu.vn", "Nam@123");
            _fixture.NavigateToUrl("/Voucher/Edit?voucherId=1");
            _fixture.WaitForPageToLoad();
            var header = _fixture.WaitForElement(By.CssSelector("h2.content-title"));
            header.Text.Should().Be("Cập nhật Voucher");
        }
        [Fact]
        public void Delete()
        {
            // Arrange
            LoginUser("nampdce172019@fpt.edu.vn", "Nam@123");
            _fixture.NavigateToUrl("/Voucher/Detail?voucherId=1");
            _fixture.WaitForPageToLoad();
            var header = _fixture.WaitForElement(By.CssSelector("h2.content-title"));
            header.Text.Should().Be("Xóa Voucher");
        }
    }
}
