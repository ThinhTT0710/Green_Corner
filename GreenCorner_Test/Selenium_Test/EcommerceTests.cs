using OpenQA.Selenium;
using FluentAssertions;
using DocumentFormat.OpenXml.Bibliography;
using GreenCorner_Test.Selenium_Test;

namespace GreenCorner_Test.Selenium_Test
{
    public class EcommerceTests : BaseSeleniumTest
    {
        [Fact]
        public void UserCanBrowseProducts()
        {
            // Arrange & Act
            NavigateToUrl("/Product");

            // Assert
            WaitForPageToLoad();
            var pageTitle = Driver.Title;
            pageTitle.Should().Contain("Products", "Page title should contain 'Products'");

            var productGrid = WaitForElement(By.Id("productGrid"));
            productGrid.Should().NotBeNull("Product grid should be present");
        }

        [Fact]
        public void ProductPageShouldDisplayProductDetails()
        {
            // Arrange & Act
            NavigateToUrl("/Product");

            // Assert
            WaitForPageToLoad();

            // Look for product cards
            var productCards = Driver.FindElements(By.ClassName("product-card"));
            productCards.Should().NotBeEmpty("Product cards should be present");

            // Check first product card has required elements
            var firstProduct = productCards.First();
            var productName = firstProduct.FindElement(By.ClassName("product-name"));
            var productPrice = firstProduct.FindElement(By.ClassName("product-price"));
            var addToCartButton = firstProduct.FindElement(By.ClassName("add-to-cart-btn"));

            productName.Should().NotBeNull("Product name should be displayed");
            productPrice.Should().NotBeNull("Product price should be displayed");
            addToCartButton.Should().NotBeNull("Add to cart button should be present");
        }

        [Fact]
        public void UserCanAddProductToCart()
        {
            // Arrange
            NavigateToUrl("/Product");

            // Act
            WaitForPageToLoad();
            var addToCartButtons = Driver.FindElements(By.ClassName("add-to-cart-btn"));

            if (addToCartButtons.Any())
            {
                var firstAddToCartButton = addToCartButtons.First();
                firstAddToCartButton.Click();

                // Assert
                WaitForElement(By.ClassName("cart-notification"));
                var cartNotification = Driver.FindElement(By.ClassName("cart-notification"));
                cartNotification.Displayed.Should().BeTrue("Cart notification should be displayed after adding product");
            }
        }

        [Fact]
        public void UserCanViewCart()
        {
            // Arrange & Act
            NavigateToUrl("/Cart");

            // Assert
            WaitForPageToLoad();
            var pageTitle = Driver.Title;
            pageTitle.Should().Contain("Cart", "Page title should contain 'Cart'");

            var cartContainer = WaitForElement(By.Id("cartContainer"));
            cartContainer.Should().NotBeNull("Cart container should be present");
        }

        [Fact]
        public void CartShouldDisplayProductInformation()
        {
            // Arrange & Act
            NavigateToUrl("/Cart");

            // Assert
            WaitForPageToLoad();

            // Check for cart items
            var cartItems = Driver.FindElements(By.ClassName("cart-item"));

            if (cartItems.Any())
            {
                var firstItem = cartItems.First();

                // Check for required elements in cart item
                var productName = firstItem.FindElement(By.ClassName("cart-item-name"));
                var productPrice = firstItem.FindElement(By.ClassName("cart-item-price"));
                var quantityInput = firstItem.FindElement(By.ClassName("cart-item-quantity"));
                var removeButton = firstItem.FindElement(By.ClassName("remove-item-btn"));

                productName.Should().NotBeNull("Cart item should display product name");
                productPrice.Should().NotBeNull("Cart item should display product price");
                quantityInput.Should().NotBeNull("Cart item should have quantity input");
                removeButton.Should().NotBeNull("Cart item should have remove button");
            }
        }

        [Fact]
        public void UserCanUpdateProductQuantity()
        {
            // Arrange
            NavigateToUrl("/Cart");

            // Act
            WaitForPageToLoad();
            var quantityInputs = Driver.FindElements(By.ClassName("cart-item-quantity"));

            if (quantityInputs.Any())
            {
                var firstQuantityInput = quantityInputs.First();
                var originalValue = firstQuantityInput.GetAttribute("value");

                firstQuantityInput.Clear();
                firstQuantityInput.SendKeys("2");
                firstQuantityInput.SendKeys(Keys.Tab); // Trigger change event

                // Assert
                var newValue = firstQuantityInput.GetAttribute("value");
                newValue.Should().Be("2", "Quantity should be updated to 2");
            }
        }

        [Fact]
        public void UserCanRemoveProductFromCart()
        {
            // Arrange
            NavigateToUrl("/Cart");

            // Act
            WaitForPageToLoad();
            var removeButtons = Driver.FindElements(By.ClassName("remove-item-btn"));

            if (removeButtons.Any())
            {
                var initialItemCount = Driver.FindElements(By.ClassName("cart-item")).Count;
                var firstRemoveButton = removeButtons.First();
                firstRemoveButton.Click();

                // Assert
                WaitForElementToDisappear(By.ClassName("cart-item"));
                var finalItemCount = Driver.FindElements(By.ClassName("cart-item")).Count;
                finalItemCount.Should().BeLessThan(initialItemCount, "Item count should decrease after removal");
            }
        }

        [Fact]
        public void UserCanProceedToCheckout()
        {
            // Arrange
            NavigateToUrl("/Cart");

            // Act
            WaitForPageToLoad();
            var checkoutButton = WaitForElementToBeClickable(By.Id("checkoutButton"));
            checkoutButton.Click();

            // Assert
            WaitForPageToLoad();
            var currentUrl = Driver.Url;
            currentUrl.Should().Contain("Checkout", "Should navigate to checkout page");
        }

        [Fact]
        public void CheckoutPageShouldDisplayOrderSummary()
        {
            // Arrange & Act
            NavigateToUrl("/Order/Checkout");

            // Assert
            WaitForPageToLoad();
            var orderSummary = WaitForElement(By.Id("orderSummary"));
            orderSummary.Should().NotBeNull("Order summary should be present");

            var subtotal = orderSummary.FindElement(By.ClassName("subtotal"));
            var total = orderSummary.FindElement(By.ClassName("total"));

            subtotal.Should().NotBeNull("Subtotal should be displayed");
            total.Should().NotBeNull("Total should be displayed");
        }

        [Fact]
        public void CheckoutPageShouldHavePaymentForm()
        {
            // Arrange & Act
            NavigateToUrl("/Order/Checkout");

            // Assert
            WaitForPageToLoad();
            var paymentForm = WaitForElement(By.Id("paymentForm"));
            paymentForm.Should().NotBeNull("Payment form should be present");

            // Check for payment method options
            var paymentMethods = Driver.FindElements(By.Name("paymentMethod"));
            paymentMethods.Should().NotBeEmpty("Payment methods should be available");
        }

        [Fact]
        public void UserCanSearchForProducts()
        {
            // Arrange
            NavigateToUrl("/Product");

            // Act
            WaitForPageToLoad();
            var searchInput = WaitForElement(By.Id("searchInput"));
            var searchButton = WaitForElementToBeClickable(By.Id("searchButton"));

            searchInput.SendKeys("eco-friendly");
            searchButton.Click();

            // Assert
            WaitForPageToLoad();
            var searchResults = Driver.FindElements(By.ClassName("product-card"));
            searchResults.Should().NotBeEmpty("Search should return results");
        }

        [Fact]
        public void UserCanFilterProductsByCategory()
        {
            // Arrange
            NavigateToUrl("/Product");

            // Act
            WaitForPageToLoad();
            var categoryFilters = Driver.FindElements(By.ClassName("category-filter"));

            if (categoryFilters.Any())
            {
                var firstCategory = categoryFilters.First();
                firstCategory.Click();

                // Assert
                WaitForPageToLoad();
                var filteredProducts = Driver.FindElements(By.ClassName("product-card"));
                filteredProducts.Should().NotBeEmpty("Filtered products should be displayed");
            }
        }

        [Fact]
        public void ProductDetailPageShouldDisplayCompleteInformation()
        {
            // Arrange
            NavigateToUrl("/Product");

            // Act
            WaitForPageToLoad();
            var productLinks = Driver.FindElements(By.ClassName("product-link"));

            if (productLinks.Any())
            {
                productLinks.First().Click();
                WaitForPageToLoad();

                // Assert
                var productDetail = WaitForElement(By.Id("productDetail"));
                productDetail.Should().NotBeNull("Product detail section should be present");

                // Check for required elements
                var productName = Driver.FindElement(By.ClassName("product-detail-name"));
                var productDescription = Driver.FindElement(By.ClassName("product-detail-description"));
                var productPrice = Driver.FindElement(By.ClassName("product-detail-price"));
                var addToCartButton = Driver.FindElement(By.ClassName("product-detail-add-to-cart"));

                productName.Should().NotBeNull("Product name should be displayed");
                productDescription.Should().NotBeNull("Product description should be displayed");
                productPrice.Should().NotBeNull("Product price should be displayed");
                addToCartButton.Should().NotBeNull("Add to cart button should be present");
            }
        }

        [Fact]
        public void EmptyCartShouldDisplayAppropriateMessage()
        {
            // Arrange & Act
            NavigateToUrl("/Cart");

            // Assert
            WaitForPageToLoad();

            var cartItems = Driver.FindElements(By.ClassName("cart-item"));

            if (!cartItems.Any())
            {
                var emptyCartMessage = Driver.FindElement(By.ClassName("empty-cart-message"));
                emptyCartMessage.Should().NotBeNull("Empty cart message should be displayed");
                emptyCartMessage.Displayed.Should().BeTrue("Empty cart message should be visible");
            }
        }
    }
}
