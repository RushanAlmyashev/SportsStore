﻿using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Xunit;
using SportStore.Controllers;
using SportStore.Models;
using SportStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace SportStore.Tests
{
    public class ProductControllerTests
    {
        [Fact]
        public void Can_Paginate()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
                {
                    new Product {ProductID = 1, Name = "P1" },
                    new Product {ProductID = 2, Name = "P2" },
                    new Product {ProductID = 3, Name = "P3" },
                    new Product {ProductID = 4, Name = "P4" },
                    new Product {ProductID = 5, Name = "P5" }
                });
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;
            //IEnumerable<Product> result = controller.List(2).ViewData.Model as IEnumerable<Product>;
            ProductListViewModel result = controller.List(null, 2).ViewData.Model as ProductListViewModel;

            Product[] productArray = result.Products.ToArray();
            Assert.True(productArray.Length == 2);
            Assert.Equal("P4", productArray[0].Name);
            Assert.Equal("P5", productArray[1].Name);
        }

        [Fact]
        public void Can_Send_Pagination_View_Model()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1" },
                new Product {ProductID = 2, Name = "P2" },
                new Product {ProductID = 3, Name = "P3" },
                new Product {ProductID = 4, Name = "P4" },
                new Product {ProductID = 5, Name = "P5" }
            });

            ProductController controller = new ProductController(mock.Object)
            {
                PageSize = 3
            };

            ProductListViewModel result = controller.List(null, 2).ViewData.Model as ProductListViewModel;

            PagingInfo pageInfo = result.PagingInfo;
            Assert.Equal(2, pageInfo.CurrentPage);
            Assert.Equal(3, pageInfo.ItemsPerPage);
            Assert.Equal(5, pageInfo.TotalItems);
            Assert.Equal(2, pageInfo.TotalPages);
        }

        [Fact]
        public void Can_Filter_Products()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1", Category = "Cat1"  },
                new Product {ProductID = 2, Name = "P2", Category = "Cat2"  },
                new Product {ProductID = 3, Name = "P3", Category = "Cat1"  },
                new Product {ProductID = 4, Name = "P4", Category = "Cat2"  },
                new Product {ProductID = 5, Name = "P5", Category = "Cat3"  }
            });

            ProductController controller = new ProductController(mock.Object)
            {
                PageSize = 3
            };
            Product[] result = 
                (controller.List("Cat2", 1).ViewData.Model as ProductListViewModel).Products.ToArray();

            Assert.Equal(2, result.Count());
            Assert.True(result[0].Name == "P2" && result[0].Category == "Cat2");
            Assert.True(result[1].Name == "P4" && result[1].Category == "Cat2");

        }

        [Fact]
        public void Generate_Category_Specific_Product_Count()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1", Category = "Cat1"  },
                new Product {ProductID = 2, Name = "P2", Category = "Cat2"  },
                new Product {ProductID = 3, Name = "P3", Category = "Cat1"  },
                new Product {ProductID = 4, Name = "P4", Category = "Cat2"  },
                new Product {ProductID = 5, Name = "P5", Category = "Cat3"  }
            });
            ProductController target = new ProductController(mock.Object);
            target.PageSize = 3;
            Func<ViewResult, ProductListViewModel> GetModel = result => result?.ViewData?.Model as ProductListViewModel;

            int? res1 = GetModel(target.List("Cat1")).PagingInfo.TotalItems;
            int? res2 = GetModel(target.List("Cat2")).PagingInfo.TotalItems;
            int? res3 = GetModel(target.List("Cat3")).PagingInfo.TotalItems;
            int? resAll = GetModel(target.List(null)).PagingInfo.TotalItems;

            Assert.Equal(2, res1);
            Assert.Equal(2, res2);
            Assert.Equal(1, res3);
            Assert.Equal(5, resAll);

        }
    }
}