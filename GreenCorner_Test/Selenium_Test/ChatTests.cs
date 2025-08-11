using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenCorner_Test.Selenium_Test
{
    public class ChatTests : IClassFixture<SeleniumFixture>
    {
        private readonly SeleniumFixture _fixture;

        public ChatTests(SeleniumFixture fixture)
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
        public void ChatView_ShouldDisplayAndSendMessagesSuccessfully()
        {
            // Arrange
            LoginUser("qgbeo711@gmail.com", "Nam@12345");

            // Điều hướng tới trang chat của sự kiện (ví dụ eventId = 1)
            _fixture.NavigateToUrl($"/Chat?eventId=1");
            _fixture.WaitForPageToLoad();
            var messageList = _fixture.Driver.FindElement(By.Id("messageList"));
            var messageInput = _fixture.Driver.FindElement(By.Id("messageInput"));
            var sendButton = _fixture.Driver.FindElement(By.Id("sendButton"));

            // Assert: Danh sách tin nhắn ban đầu phải hiển thị
            Assert.True(messageList.Displayed, "Danh sách tin nhắn không hiển thị");

            // Act: Gửi một tin nhắn mới
            string testMessage = "Tin nhắn test " + DateTime.Now.Ticks;
            messageInput.Clear();
            messageInput.SendKeys(testMessage);
            sendButton.Click();

            // Wait: Đợi tin nhắn mới xuất hiện trong danh sách
            var wait = new WebDriverWait(_fixture.Driver, TimeSpan.FromSeconds(5));
            var newMessageElement = wait.Until(driver =>
                driver.FindElements(By.CssSelector(".message .content"))
                      .FirstOrDefault(el => el.Text == testMessage));

            // Assert: Tin nhắn mới đã được hiển thị
            Assert.NotNull(newMessageElement);
            Assert.Equal(testMessage, newMessageElement.Text);
        }
    }
}
