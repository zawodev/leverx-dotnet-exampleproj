using ComputerStore.Application.Features.Commands;
using ComputerStore.Application.Repositories;
using ComputerStoreAPI.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace ComputerStore.Application.Features.Handlers {
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, int> {
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;

        public CreateProductHandler(IProductRepository repo, IMapper mapper) {
            _repo = repo;
            _mapper = mapper;
        }

        public Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
            => _repo.CreateAsync(_mapper.Map<Product>(request));
    }
}
