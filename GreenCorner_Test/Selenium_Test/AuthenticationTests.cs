using OpenQA.Selenium;
using FluentAssertions;
using Xunit;
using DocumentFormat.OpenXml.Bibliography;
using Docker.DotNet.Models;

namespace GreenCorner_Test.Selenium_Test
{
    public class AuthenticationTests : IClassFixture<SeleniumFixture>
    {
        private readonly SeleniumFixture _fixture;

        public AuthenticationTests(SeleniumFixture fixture)
        {
            _fixture = fixture;
        }
       


        [Fact]
        public void UserCanNavigateToLoginPage()
        {
            _fixture.NavigateToUrl("/Auth/Logout");
            _fixture.NavigateToUrl("/Auth/Login");

            _fixture.WaitForPageToLoad();
            var pageTitle = _fixture.Driver.Title;
            pageTitle.Should().Contain("Đăng nhập");
            var loginForm = _fixture.WaitForElement(By.TagName("form"));
            loginForm.Should().NotBeNull();
        }

        [Fact]
        public void UserCanNavigateToRegisterPage()
        {
            _fixture.NavigateToUrl("/Auth/Logout");
            _fixture.NavigateToUrl("/Auth/Register");
            _fixture.WaitForPageToLoad();
        }
      
        
       
        [Fact]
        public void UserCanLogoutSuccessfully()
        {

            _fixture.NavigateToUrl("/Auth/Login");

            // TODO: login here if needed for your flow

            _fixture.NavigateToUrl("/Auth/Logout");

            _fixture.WaitForPageToLoad();
            var currentUrl = _fixture.Driver.Url;
        }
      

        [Fact]
        public void RegisterPageShouldHaveRequiredFields()
        {
            _fixture.NavigateToUrl("/Auth/Logout");
            _fixture.NavigateToUrl("/Auth/Register");

            var requiredFields = new[] { "Email", "FullName", "PhoneNumber", "Address", "Password" };

            foreach (var fieldId in requiredFields)
            {
                var field = _fixture.WaitForElement(By.Id(fieldId));
                field.Should().NotBeNull();
            }
        }

        [Fact]
        public void PasswordFieldShouldBeMasked()
        {
            _fixture.NavigateToUrl("/Auth/Logout");
            _fixture.NavigateToUrl("/Auth/Login");

            var passwordInput = _fixture.WaitForElement(By.Id("Password"));
            passwordInput.GetAttribute("type").Should().Be("password");
        }

        [Fact]
        public void UserCanAccessForgotPasswordPage()
        {
            _fixture.NavigateToUrl("/Auth/Logout");
            _fixture.NavigateToUrl("/Auth/ForgotPassword");

            _fixture.WaitForPageToLoad();
            var pageTitle = _fixture.Driver.Title;
            pageTitle.Should().Contain("Quản lí tài khoản");

            var forgotPasswordForm = _fixture.WaitForElement(By.CssSelector("form[action*='ForgotPassword']"));

            forgotPasswordForm.Should().NotBeNull();
        }

        [Fact]
        public void ForgotPasswordPageShouldDisplayCorrectForm()
        {
            _fixture.NavigateToUrl("/Auth/Logout");
            // Arrange & Act
            _fixture.NavigateToUrl("/Auth/ForgotPassword");

            _fixture.WaitForPageToLoad();

            // Assert - tìm form với action chứa 'ForgotPassword'
            var forgotPasswordForm = _fixture.WaitForElement(By.CssSelector("form[action*='ForgotPassword']"));
            forgotPasswordForm.Should().NotBeNull("Forgot password form should be present");

            // Kiểm tra input ẩn
            forgotPasswordForm.FindElement(By.Name("UserId")).Should().NotBeNull("Hidden UserId should be present");
            forgotPasswordForm.FindElement(By.Name("Token")).Should().NotBeNull("Hidden Token should be present");

            // Kiểm tra input mật khẩu
            var passwordInput = forgotPasswordForm.FindElement(By.Name("Password"));
            passwordInput.Should().NotBeNull("Password input should be present");

            var confirmPasswordInput = forgotPasswordForm.FindElement(By.Name("ConfirmPassword"));
            confirmPasswordInput.Should().NotBeNull("ConfirmPassword input should be present");

            // Kiểm tra nút submit
            var submitButton = forgotPasswordForm.FindElement(By.CssSelector("button[type='submit']"));
            submitButton.Should().NotBeNull("Submit button should be present");
        }


    }
}
