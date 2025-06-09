using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerStore.Application.Services {
    public interface IDiscountService {
        /// <summary>
        /// Returns discount percentage for a given code (0–100).
        /// </summary>
        decimal GetDiscountPercentage(string code);
    }
}
