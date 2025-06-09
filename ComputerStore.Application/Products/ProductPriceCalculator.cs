using ComputerStore.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerStore.Application.Products {
    /// <summary>
    /// business logic - apply discount to base price
    /// </summary>
    public class ProductPriceCalculator {
        private readonly IDiscountService _discountService;
        public ProductPriceCalculator(IDiscountService discountService) {
            _discountService = discountService;
        }

        /// <summary>
        /// calculates final price = basePrice * (1 - discountPct/100)
        /// rounded to 2 decimal points
        /// </summary>
        public decimal CalculateFinalPrice(decimal basePrice, string discountCode) {
            var pct = _discountService.GetDiscountPercentage(discountCode);
            return Math.Round(basePrice * (1 - pct / 100m), 2);
        }
    }
}
