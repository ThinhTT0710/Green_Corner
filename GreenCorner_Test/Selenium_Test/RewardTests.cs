using OpenQA.Selenium;
using FluentAssertions;

namespace GreenCorner_Test.Selenium_Test
{
    public class RewardTests : BaseSeleniumTest
    {
        [Fact]
        public void UserCanViewRewardsPage()
        {
            // Arrange & Act
            NavigateToUrl("/RewardPoint");

            // Assert
            WaitForPageToLoad();
            var pageTitle = Driver.Title;
            pageTitle.Should().Contain("Rewards", "Page title should contain 'Rewards'");

            var rewardsContainer = WaitForElement(By.Id("rewardsContainer"));
            rewardsContainer.Should().NotBeNull("Rewards container should be present");
        }

        [Fact]
        public void RewardsPageShouldDisplayUserPoints()
        {
            // Arrange & Act
            NavigateToUrl("/RewardPoint");

            // Assert
            WaitForPageToLoad();

            var pointsDisplay = WaitForElement(By.Id("userPoints"));
            pointsDisplay.Should().NotBeNull("User points should be displayed");
            pointsDisplay.Displayed.Should().BeTrue("User points should be visible");
        }

        [Fact]
        public void UserCanViewAvailableRewards()
        {
            // Arrange & Act
            NavigateToUrl("/RewardPoint");

            // Assert
            WaitForPageToLoad();

            var rewardCards = Driver.FindElements(By.ClassName("reward-card"));
            rewardCards.Should().NotBeEmpty("Reward cards should be present");

            // Check first reward card has required elements
            var firstReward = rewardCards.First();
            var rewardName = firstReward.FindElement(By.ClassName("reward-name"));
            var rewardPoints = firstReward.FindElement(By.ClassName("reward-points"));
            var redeemButton = firstReward.FindElement(By.ClassName("redeem-reward-btn"));

            rewardName.Should().NotBeNull("Reward name should be displayed");
            rewardPoints.Should().NotBeNull("Reward points should be displayed");
            redeemButton.Should().NotBeNull("Redeem button should be present");
        }

        [Fact]
        public void UserCanRedeemReward()
        {
            // Arrange
            NavigateToUrl("/RewardPoint");

            // Act
            WaitForPageToLoad();
            var redeemButtons = Driver.FindElements(By.ClassName("redeem-reward-btn"));

            if (redeemButtons.Any())
            {
                var firstRedeemButton = redeemButtons.First();
                firstRedeemButton.Click();

                // Assert
                WaitForElement(By.Id("redeemConfirmationModal"));
                var confirmationModal = Driver.FindElement(By.Id("redeemConfirmationModal"));
                confirmationModal.Should().NotBeNull("Redeem confirmation modal should be displayed");
                confirmationModal.Displayed.Should().BeTrue("Redeem confirmation modal should be visible");
            }
        }

        [Fact]
        public void UserCanViewRewardHistory()
        {
            // Arrange & Act
            NavigateToUrl("/RewardHistory");

            // Assert
            WaitForPageToLoad();
            var pageTitle = Driver.Title;
            pageTitle.Should().Contain("Reward History", "Page title should contain 'Reward History'");

            var historyContainer = WaitForElement(By.Id("rewardHistoryContainer"));
            historyContainer.Should().NotBeNull("Reward history container should be present");
        }

        [Fact]
        public void RewardHistoryShouldDisplayTransactions()
        {
            // Arrange & Act
            NavigateToUrl("/RewardHistory");

            // Assert
            WaitForPageToLoad();

            var transactionRows = Driver.FindElements(By.ClassName("reward-transaction"));

            if (transactionRows.Any())
            {
                var firstTransaction = transactionRows.First();

                // Check for required elements in transaction
                var transactionDate = firstTransaction.FindElement(By.ClassName("transaction-date"));
                var transactionType = firstTransaction.FindElement(By.ClassName("transaction-type"));
                var transactionPoints = firstTransaction.FindElement(By.ClassName("transaction-points"));
                var transactionDescription = firstTransaction.FindElement(By.ClassName("transaction-description"));

                transactionDate.Should().NotBeNull("Transaction date should be displayed");
                transactionType.Should().NotBeNull("Transaction type should be displayed");
                transactionPoints.Should().NotBeNull("Transaction points should be displayed");
                transactionDescription.Should().NotBeNull("Transaction description should be displayed");
            }
        }

        [Fact]
        public void UserCanFilterRewardHistory()
        {
            // Arrange
            NavigateToUrl("/RewardHistory");

            // Act
            WaitForPageToLoad();
            var filterDropdown = Driver.FindElements(By.Id("historyFilterDropdown"));

            if (filterDropdown.Any())
            {
                var dropdown = filterDropdown.First();
                dropdown.Click();

                var earnedOption = WaitForElementToBeClickable(By.CssSelector("option[value='earned']"));
                earnedOption.Click();

                // Assert
                WaitForPageToLoad();
                var filteredTransactions = Driver.FindElements(By.ClassName("reward-transaction"));
                filteredTransactions.Should().NotBeEmpty("Filtered transactions should be displayed");
            }
        }

        [Fact]
        public void UserCanViewPointEarningActivities()
        {
            // Arrange & Act
            NavigateToUrl("/RewardPoint");

            // Assert
            WaitForPageToLoad();

            var earningActivitiesSection = Driver.FindElement(By.Id("earningActivities"));
            earningActivitiesSection.Should().NotBeNull("Earning activities section should be present");
            earningActivitiesSection.Displayed.Should().BeTrue("Earning activities section should be visible");
        }

        [Fact]
        public void UserCanViewPointBalance()
        {
            // Arrange & Act
            NavigateToUrl("/RewardPoint");

            // Assert
            WaitForPageToLoad();

            var pointBalance = WaitForElement(By.Id("pointBalance"));
            pointBalance.Should().NotBeNull("Point balance should be displayed");

            var balanceText = pointBalance.Text;
            balanceText.Should().NotBeNullOrEmpty("Point balance should not be empty");
        }

        [Fact]
        public void UserCanViewRewardTiers()
        {
            // Arrange & Act
            NavigateToUrl("/RewardPoint");

            // Assert
            WaitForPageToLoad();

            var tierSection = Driver.FindElement(By.Id("rewardTiers"));
            tierSection.Should().NotBeNull("Reward tiers section should be present");
            tierSection.Displayed.Should().BeTrue("Reward tiers section should be visible");
        }

        [Fact]
        public void UserCanViewNextRewardTier()
        {
            // Arrange & Act
            NavigateToUrl("/RewardPoint");

            // Assert
            WaitForPageToLoad();

            var nextTierSection = Driver.FindElement(By.Id("nextTier"));
            nextTierSection.Should().NotBeNull("Next tier section should be present");
            nextTierSection.Displayed.Should().BeTrue("Next tier section should be visible");
        }

        [Fact]
        public void UserCanViewProgressToNextTier()
        {
            // Arrange & Act
            NavigateToUrl("/RewardPoint");

            // Assert
            WaitForPageToLoad();

            var progressBar = Driver.FindElement(By.Id("tierProgressBar"));
            progressBar.Should().NotBeNull("Tier progress bar should be present");
            progressBar.Displayed.Should().BeTrue("Tier progress bar should be visible");
        }

        [Fact]
        public void UserCanViewRewardDetails()
        {
            // Arrange
            NavigateToUrl("/RewardPoint");

            // Act
            WaitForPageToLoad();
            var rewardLinks = Driver.FindElements(By.ClassName("reward-detail-link"));

            if (rewardLinks.Any())
            {
                rewardLinks.First().Click();
                WaitForPageToLoad();

                // Assert
                var rewardDetail = WaitForElement(By.Id("rewardDetail"));
                rewardDetail.Should().NotBeNull("Reward detail section should be present");

                // Check for required elements
                var rewardName = Driver.FindElement(By.ClassName("reward-detail-name"));
                var rewardDescription = Driver.FindElement(By.ClassName("reward-detail-description"));
                var rewardPoints = Driver.FindElement(By.ClassName("reward-detail-points"));
                var rewardTerms = Driver.FindElement(By.ClassName("reward-detail-terms"));

                rewardName.Should().NotBeNull("Reward name should be displayed");
                rewardDescription.Should().NotBeNull("Reward description should be displayed");
                rewardPoints.Should().NotBeNull("Reward points should be displayed");
                rewardTerms.Should().NotBeNull("Reward terms should be displayed");
            }
        }

        [Fact]
        public void UserCanExportRewardHistory()
        {
            // Arrange
            NavigateToUrl("/RewardHistory");

            // Act
            WaitForPageToLoad();
            var exportButton = WaitForElementToBeClickable(By.Id("exportHistoryButton"));
            exportButton.Click();

            // Assert
            WaitForElement(By.ClassName("export-success"));
            var exportSuccess = Driver.FindElement(By.ClassName("export-success"));
            exportSuccess.Displayed.Should().BeTrue("Export success message should be displayed");
        }

        [Fact]
        public void UserCanViewRewardNotifications()
        {
            // Arrange & Act
            NavigateToUrl("/RewardPoint");

            // Assert
            WaitForPageToLoad();

            var notificationsSection = Driver.FindElement(By.Id("rewardNotifications"));
            notificationsSection.Should().NotBeNull("Reward notifications section should be present");
            notificationsSection.Displayed.Should().BeTrue("Reward notifications section should be visible");
        }

        [Fact]
        public void UserCanViewPointExpiry()
        {
            // Arrange & Act
            NavigateToUrl("/RewardPoint");

            // Assert
            WaitForPageToLoad();

            var expirySection = Driver.FindElement(By.Id("pointExpiry"));
            expirySection.Should().NotBeNull("Point expiry section should be present");
            expirySection.Displayed.Should().BeTrue("Point expiry section should be visible");
        }

        [Fact]
        public void UserCanViewRewardCategories()
        {
            // Arrange & Act
            NavigateToUrl("/RewardPoint");

            // Assert
            WaitForPageToLoad();

            var categoryFilters = Driver.FindElements(By.ClassName("reward-category-filter"));
            categoryFilters.Should().NotBeEmpty("Reward category filters should be present");

            foreach (var category in categoryFilters)
            {
                category.Displayed.Should().BeTrue("Category filter should be visible");
                category.Enabled.Should().BeTrue("Category filter should be enabled");
            }
        }

        [Fact]
        public void UserCanSortRewardsByPoints()
        {
            // Arrange
            NavigateToUrl("/RewardPoint");

            // Act
            WaitForPageToLoad();
            var sortDropdown = Driver.FindElements(By.Id("rewardSortDropdown"));

            if (sortDropdown.Any())
            {
                var dropdown = sortDropdown.First();
                dropdown.Click();

                var pointsOption = WaitForElementToBeClickable(By.CssSelector("option[value='points']"));
                pointsOption.Click();

                // Assert
                WaitForPageToLoad();
                var rewardCards = Driver.FindElements(By.ClassName("reward-card"));
                rewardCards.Should().NotBeEmpty("Rewards should be displayed after sorting");
            }
        }
    }
}
