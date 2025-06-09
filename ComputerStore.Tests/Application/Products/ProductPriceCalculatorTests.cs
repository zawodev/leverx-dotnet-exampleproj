using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using ComputerStore.Application.Products;
using ComputerStore.Application.Services;

namespace ComputerStoreAPI.Tests.Application.Products {
    public class ProductPriceCalculatorTests {
        [Fact]
        public void CalculateFinalPrice_NoDiscount_ReturnsBasePrice() {
            // Arrange
            var mockDiscount = new Mock<IDiscountService>();
            mockDiscount
                .Setup(s => s.GetDiscountPercentage("NOPE"))
                .Returns(0m);

            var calc = new ProductPriceCalculator(mockDiscount.Object);
            decimal basePrice = 123.45m;

            // Act
            var result = calc.CalculateFinalPrice(basePrice, "NOPE");

            // Assert
            Assert.Equal(123.45m, result);
            mockDiscount.Verify(s => s.GetDiscountPercentage("NOPE"), Times.Once);
        }

        [Theory]
        [InlineData(100, "SUMMER10", 90.00)]
        [InlineData(200, "VIP20", 160.00)]
        public void CalculateFinalPrice_WithValidCodes_AppliesCorrectDiscount(
            decimal basePrice,
            string code,
            decimal expected) {
            // Arrange
            var discountService = new FixedDiscountService();
            var calc = new ProductPriceCalculator(discountService);

            // Act
            var result = calc.CalculateFinalPrice(basePrice, code);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
