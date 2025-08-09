using OpenQA.Selenium;
using FluentAssertions;
using DocumentFormat.OpenXml.Bibliography;
using GreenCorner_Test.Selenium_Test;

namespace GreenCorner_Test.Selenium_Test
{
    public class EventTests : BaseSeleniumTest
    {
        [Fact]
        public void UserCanBrowseEvents()
        {
            // Arrange & Act
            NavigateToUrl("/Event");

            // Assert
            WaitForPageToLoad();
            var pageTitle = Driver.Title;
            pageTitle.Should().Contain("Events", "Page title should contain 'Events'");

            var eventsContainer = WaitForElement(By.Id("eventsContainer"));
            eventsContainer.Should().NotBeNull("Events container should be present");
        }

        [Fact]
        public void EventsPageShouldDisplayEventCards()
        {
            // Arrange & Act
            NavigateToUrl("/Event");

            // Assert
            WaitForPageToLoad();

            var eventCards = Driver.FindElements(By.ClassName("event-card"));
            eventCards.Should().NotBeEmpty("Event cards should be present");

            // Check first event card has required elements
            var firstEvent = eventCards.First();
            var eventTitle = firstEvent.FindElement(By.ClassName("event-title"));
            var eventDate = firstEvent.FindElement(By.ClassName("event-date"));
            var eventLocation = firstEvent.FindElement(By.ClassName("event-location"));
            var registerButton = firstEvent.FindElement(By.ClassName("register-event-btn"));

            eventTitle.Should().NotBeNull("Event title should be displayed");
            eventDate.Should().NotBeNull("Event date should be displayed");
            eventLocation.Should().NotBeNull("Event location should be displayed");
            registerButton.Should().NotBeNull("Register button should be present");
        }

        [Fact]
        public void UserCanFilterEventsByCategory()
        {
            // Arrange
            NavigateToUrl("/Event");

            // Act
            WaitForPageToLoad();
            var categoryFilters = Driver.FindElements(By.ClassName("event-category-filter"));

            if (categoryFilters.Any())
            {
                var firstCategory = categoryFilters.First();
                firstCategory.Click();

                // Assert
                WaitForPageToLoad();
                var filteredEvents = Driver.FindElements(By.ClassName("event-card"));
                filteredEvents.Should().NotBeEmpty("Filtered events should be displayed");
            }
        }

        [Fact]
        public void UserCanSearchForEvents()
        {
            // Arrange
            NavigateToUrl("/Event");

            // Act
            WaitForPageToLoad();
            var searchInput = WaitForElement(By.Id("eventSearchInput"));
            var searchButton = WaitForElementToBeClickable(By.Id("eventSearchButton"));

            searchInput.SendKeys("cleanup");
            searchButton.Click();

            // Assert
            WaitForPageToLoad();
            var searchResults = Driver.FindElements(By.ClassName("event-card"));
            searchResults.Should().NotBeEmpty("Search should return results");
        }

        [Fact]
        public void UserCanViewEventDetails()
        {
            // Arrange
            NavigateToUrl("/Event");

            // Act
            WaitForPageToLoad();
            var eventLinks = Driver.FindElements(By.ClassName("event-link"));

            if (eventLinks.Any())
            {
                eventLinks.First().Click();
                WaitForPageToLoad();

                // Assert
                var eventDetail = WaitForElement(By.Id("eventDetail"));
                eventDetail.Should().NotBeNull("Event detail section should be present");

                // Check for required elements
                var eventTitle = Driver.FindElement(By.ClassName("event-detail-title"));
                var eventDescription = Driver.FindElement(By.ClassName("event-detail-description"));
                var eventDate = Driver.FindElement(By.ClassName("event-detail-date"));
                var eventLocation = Driver.FindElement(By.ClassName("event-detail-location"));
                var registerButton = Driver.FindElement(By.ClassName("event-detail-register"));

                eventTitle.Should().NotBeNull("Event title should be displayed");
                eventDescription.Should().NotBeNull("Event description should be displayed");
                eventDate.Should().NotBeNull("Event date should be displayed");
                eventLocation.Should().NotBeNull("Event location should be displayed");
                registerButton.Should().NotBeNull("Register button should be present");
            }
        }

        [Fact]
        public void UserCanRegisterForEvent()
        {
            // Arrange
            NavigateToUrl("/Event");

            // Act
            WaitForPageToLoad();
            var registerButtons = Driver.FindElements(By.ClassName("register-event-btn"));

            if (registerButtons.Any())
            {
                var firstRegisterButton = registerButtons.First();
                firstRegisterButton.Click();

                // Assert
                WaitForPageToLoad();
                var currentUrl = Driver.Url;
                currentUrl.Should().Contain("Register", "Should navigate to event registration page");
            }
        }

        [Fact]
        public void EventRegistrationPageShouldDisplayForm()
        {
            // Arrange & Act
            NavigateToUrl("/Event/Register/1"); // Assuming event ID 1 exists

            // Assert
            WaitForPageToLoad();
            var registrationForm = WaitForElement(By.Id("eventRegistrationForm"));
            registrationForm.Should().NotBeNull("Event registration form should be present");

            // Check for form fields
            var nameInput = Driver.FindElement(By.Id("participantName"));
            var emailInput = Driver.FindElement(By.Id("participantEmail"));
            var phoneInput = Driver.FindElement(By.Id("participantPhone"));
            var submitButton = Driver.FindElement(By.Id("submitRegistration"));

            nameInput.Should().NotBeNull("Name input should be present");
            emailInput.Should().NotBeNull("Email input should be present");
            phoneInput.Should().NotBeNull("Phone input should be present");
            submitButton.Should().NotBeNull("Submit button should be present");
        }

        [Fact]
        public void UserCanViewMyEvents()
        {
            // Arrange & Act
            NavigateToUrl("/Event/MyEvents");

            // Assert
            WaitForPageToLoad();
            var pageTitle = Driver.Title;
            pageTitle.Should().Contain("My Events", "Page title should contain 'My Events'");

            var myEventsContainer = WaitForElement(By.Id("myEventsContainer"));
            myEventsContainer.Should().NotBeNull("My events container should be present");
        }

        [Fact]
        public void UserCanCancelEventRegistration()
        {
            // Arrange
            NavigateToUrl("/Event/MyEvents");

            // Act
            WaitForPageToLoad();
            var cancelButtons = Driver.FindElements(By.ClassName("cancel-registration-btn"));

            if (cancelButtons.Any())
            {
                var initialEventCount = Driver.FindElements(By.ClassName("my-event-card")).Count;
                var firstCancelButton = cancelButtons.First();
                firstCancelButton.Click();

                // Assert
                WaitForElementToDisappear(By.ClassName("my-event-card"));
                var finalEventCount = Driver.FindElements(By.ClassName("my-event-card")).Count;
                finalEventCount.Should().BeLessThan(initialEventCount, "Event count should decrease after cancellation");
            }
        }

        [Fact]
        public void EventsPageShouldDisplayUpcomingEvents()
        {
            // Arrange & Act
            NavigateToUrl("/Event");

            // Assert
            WaitForPageToLoad();

            var upcomingEventsSection = Driver.FindElement(By.Id("upcomingEvents"));
            upcomingEventsSection.Should().NotBeNull("Upcoming events section should be present");
            upcomingEventsSection.Displayed.Should().BeTrue("Upcoming events section should be visible");
        }

        [Fact]
        public void UserCanViewEventCalendar()
        {
            // Arrange & Act
            NavigateToUrl("/Event/Calendar");

            // Assert
            WaitForPageToLoad();
            var pageTitle = Driver.Title;
            pageTitle.Should().Contain("Calendar", "Page title should contain 'Calendar'");

            var calendarContainer = WaitForElement(By.Id("eventCalendar"));
            calendarContainer.Should().NotBeNull("Event calendar should be present");
        }

        [Fact]
        public void EventDetailPageShouldShowParticipantCount()
        {
            // Arrange
            NavigateToUrl("/Event");

            // Act
            WaitForPageToLoad();
            var eventLinks = Driver.FindElements(By.ClassName("event-link"));

            if (eventLinks.Any())
            {
                eventLinks.First().Click();
                WaitForPageToLoad();

                // Assert
                var participantCount = Driver.FindElement(By.ClassName("participant-count"));
                participantCount.Should().NotBeNull("Participant count should be displayed");
            }
        }

        [Fact]
        public void UserCanShareEvent()
        {
            // Arrange
            NavigateToUrl("/Event");

            // Act
            WaitForPageToLoad();
            var eventLinks = Driver.FindElements(By.ClassName("event-link"));

            if (eventLinks.Any())
            {
                eventLinks.First().Click();
                WaitForPageToLoad();

                var shareButton = WaitForElementToBeClickable(By.ClassName("share-event-btn"));
                shareButton.Click();

                // Assert
                var shareModal = WaitForElement(By.Id("shareEventModal"));
                shareModal.Should().NotBeNull("Share modal should be displayed");
                shareModal.Displayed.Should().BeTrue("Share modal should be visible");
            }
        }

        [Fact]
        public void EventsPageShouldHavePagination()
        {
            // Arrange & Act
            NavigateToUrl("/Event");

            // Assert
            WaitForPageToLoad();

            var pagination = Driver.FindElements(By.ClassName("pagination"));
            if (pagination.Any())
            {
                var firstPagination = pagination.First();
                firstPagination.Displayed.Should().BeTrue("Pagination should be visible when there are multiple pages");
            }
        }

        [Fact]
        public void UserCanSortEventsByDate()
        {
            // Arrange
            NavigateToUrl("/Event");

            // Act
            WaitForPageToLoad();
            var sortDropdown = Driver.FindElements(By.Id("eventSortDropdown"));

            if (sortDropdown.Any())
            {
                var dropdown = sortDropdown.First();
                dropdown.Click();

                var dateOption = WaitForElementToBeClickable(By.CssSelector("option[value='date']"));
                dateOption.Click();

                // Assert
                WaitForPageToLoad();
                var eventCards = Driver.FindElements(By.ClassName("event-card"));
                eventCards.Should().NotBeEmpty("Events should be displayed after sorting");
            }
        }
    }
}
