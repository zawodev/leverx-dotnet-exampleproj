using ComputerStore.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerStore.Application.Services {
    /// <summary>
    /// example implementation: SUMMER10 → 10%, else 0% (other codes possible this is just a mock)
    /// </summary>
    public class FixedDiscountService : IDiscountService {
        public decimal GetDiscountPercentage(string code) =>
            code switch {
                "SUMMER10" => 10m,
                "VIP20" => 20m,
                _ => 0m
            };
    }
}
