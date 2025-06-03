using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerStore.Application.Features.Commands {
    public record CreateProductCommand(
        string Name,
        string Description,
        decimal Price,
        int Stock,
        int CategoryId
    ) : IRequest<int>;
}
