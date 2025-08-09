using OpenQA.Selenium;
using FluentAssertions;
using DocumentFormat.OpenXml.Bibliography;

namespace GreenCorner_Test.Selenium_Test
{
    public class AuthenticationTests : BaseSeleniumTest
    {
        [Fact]
        public void UserCanNavigateToLoginPage()
        {
            // Arrange & Act
            NavigateToUrl("/Auth/Login");

            // Assert
            WaitForPageToLoad();
            var pageTitle = Driver.Title;
            pageTitle.Should().Contain("Login", "Page title should contain 'Login'");

            var loginForm = WaitForElement(By.Id("loginForm"));
            loginForm.Should().NotBeNull("Login form should be present on the page");
        }

        [Fact]
        public void UserCanNavigateToRegisterPage()
        {
            // Arrange & Act
            NavigateToUrl("/Auth/Register");

            // Assert
            WaitForPageToLoad();
            var pageTitle = Driver.Title;
            pageTitle.Should().Contain("Register", "Page title should contain 'Register'");

            var registerForm = WaitForElement(By.Id("registerForm"));
            registerForm.Should().NotBeNull("Register form should be present on the page");
        }

        [Fact]
        public void LoginFormShouldDisplayValidationErrorsForInvalidInput()
        {
            // Arrange
            NavigateToUrl("/Auth/Login");

            // Act
            var emailInput = WaitForElement(By.Id("Email"));
            var passwordInput = WaitForElement(By.Id("Password"));
            var loginButton = WaitForElementToBeClickable(By.Id("loginButton"));

            emailInput.SendKeys("invalid-email");
            passwordInput.SendKeys("");
            loginButton.Click();

            // Assert
            WaitForElement(By.ClassName("text-danger"));
            var validationErrors = Driver.FindElements(By.ClassName("text-danger"));
            validationErrors.Should().NotBeEmpty("Validation errors should be displayed for invalid input");
        }

        [Fact]
        public void RegisterFormShouldDisplayValidationErrorsForInvalidInput()
        {
            // Arrange
            NavigateToUrl("/Auth/Register");

            // Act
            var emailInput = WaitForElement(By.Id("Email"));
            var passwordInput = WaitForElement(By.Id("Password"));
            var confirmPasswordInput = WaitForElement(By.Id("ConfirmPassword"));
            var registerButton = WaitForElementToBeClickable(By.Id("registerButton"));

            emailInput.SendKeys("invalid-email");
            passwordInput.SendKeys("123");
            confirmPasswordInput.SendKeys("456");
            registerButton.Click();

            // Assert
            WaitForElement(By.ClassName("text-danger"));
            var validationErrors = Driver.FindElements(By.ClassName("text-danger"));
            validationErrors.Should().NotBeEmpty("Validation errors should be displayed for invalid input");
        }

        [Fact]
        public void UserCanLogoutSuccessfully()
        {
            // Arrange - First login (you might need to adjust this based on your actual login flow)
            NavigateToUrl("/Auth/Login");

            // This test assumes there's a way to login programmatically or with test credentials
            // You might need to modify this based on your actual authentication setup

            // Act - Navigate to logout
            NavigateToUrl("/Auth/Logout");

            // Assert
            WaitForPageToLoad();
            var currentUrl = Driver.Url;
            currentUrl.Should().Contain("Login", "User should be redirected to login page after logout");
        }

        [Fact]
        public void UnauthorizedUserShouldBeRedirectedToLoginPage()
        {
            // Arrange & Act - Try to access a protected page
            NavigateToUrl("/User/Profile");

            // Assert
            WaitForPageToLoad();
            var currentUrl = Driver.Url;
            currentUrl.Should().Contain("Login", "Unauthorized user should be redirected to login page");
        }

        [Fact]
        public void LoginPageShouldHaveRememberMeOption()
        {
            // Arrange & Act
            NavigateToUrl("/Auth/Login");

            // Assert
            var rememberMeCheckbox = WaitForElement(By.Id("RememberMe"));
            rememberMeCheckbox.Should().NotBeNull("Remember me checkbox should be present");
            rememberMeCheckbox.GetAttribute("type").Should().Be("checkbox", "Remember me should be a checkbox");
        }

        [Fact]
        public void RegisterPageShouldHaveRequiredFields()
        {
            // Arrange & Act
            NavigateToUrl("/Auth/Register");

            // Assert
            var requiredFields = new[] { "Email", "Password", "ConfirmPassword", "FirstName", "LastName" };

            foreach (var fieldId in requiredFields)
            {
                var field = WaitForElement(By.Id(fieldId));
                field.Should().NotBeNull($"Required field {fieldId} should be present");
                field.GetAttribute("required").Should().NotBeNull($"Field {fieldId} should be required");
            }
        }

        [Fact]
        public void PasswordFieldShouldBeMasked()
        {
            // Arrange & Act
            NavigateToUrl("/Auth/Login");

            // Assert
            var passwordInput = WaitForElement(By.Id("Password"));
            passwordInput.GetAttribute("type").Should().Be("password", "Password field should be masked");
        }

        [Fact]
        public void UserCanAccessForgotPasswordPage()
        {
            // Arrange & Act
            NavigateToUrl("/Auth/ForgotPassword");

            // Assert
            WaitForPageToLoad();
            var pageTitle = Driver.Title;
            pageTitle.Should().Contain("Forgot Password", "Page title should contain 'Forgot Password'");

            var forgotPasswordForm = WaitForElement(By.Id("forgotPasswordForm"));
            forgotPasswordForm.Should().NotBeNull("Forgot password form should be present");
        }

        [Fact]
        public void HomePageShouldDisplayLoginAndRegisterLinks()
        {
            // Arrange & Act
            NavigateToUrl("/");

            // Assert
            WaitForPageToLoad();

            // Look for login and register links in the navigation
            var loginLink = Driver.FindElement(By.LinkText("Login"));
            var registerLink = Driver.FindElement(By.LinkText("Register"));

            loginLink.Should().NotBeNull("Login link should be present on home page");
            registerLink.Should().NotBeNull("Register link should be present on home page");
        }
    }
}
