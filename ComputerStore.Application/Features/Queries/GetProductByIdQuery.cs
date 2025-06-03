using ComputerStoreAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;

namespace ComputerStore.Application.Features.Queries {
    public record GetProductByIdQuery(int Id) : IRequest<Product?>;
}
