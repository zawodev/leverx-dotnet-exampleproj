using AutoMapper;
using ComputerStore.Application.Features.Commands;
using ComputerStoreAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerStore.Infrastructure.Mapping {
    public class ProductProfile : Profile {
        public ProductProfile() {
            CreateMap<CreateProductCommand, Product>();
        }
    }
}
