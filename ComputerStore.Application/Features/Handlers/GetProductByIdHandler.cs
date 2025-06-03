using ComputerStore.Application.Features.Queries;
using ComputerStore.Application.Repositories;
using ComputerStoreAPI.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerStore.Application.Features.Handlers {
    public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, Product?> {
        private readonly IProductRepository _repo;
        public GetProductByIdHandler(IProductRepository repo) => _repo = repo;

        public Task<Product?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
            => _repo.GetByIdAsync(request.Id);
    }
}
