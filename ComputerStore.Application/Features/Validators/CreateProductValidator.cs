using ComputerStore.Application.Features.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ComputerStore.Application.Features.Validators {
    public class CreateProductValidator : AbstractValidator<CreateProductCommand> {
        public CreateProductValidator() {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(200);
            RuleFor(x => x.Price)
                .GreaterThan(0);
            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0);
        }
    }
}
