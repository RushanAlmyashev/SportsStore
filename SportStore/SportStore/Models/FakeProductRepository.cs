﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportStore.Models
{
    public class FakeProductRepository : IProductRepository
    {
        public IEnumerable<Product> Products => new List<Product>
        {
            new Product {Name = "FootBall", Price = 25},
            new Product {Name = "Surf Board", Price = 179},
            new Product {Name = "Running Shoes", Price = 95}
        };
    }
}
