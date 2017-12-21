using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Moq;
using SportStore.Components;
using SportStore.Models;
using Xunit;

namespace SportStore.Tests
{
    public class NavigationMenuViewComponentTest
    {
        [Fact]
        public void Can_Select_Category()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1", Category = "Apples"  },
                new Product {ProductID = 2, Name = "P2", Category = "Apples"  },
                new Product {ProductID = 3, Name = "P3", Category = "Plums"  },
                new Product {ProductID = 4, Name = "P4", Category = "Oranges"  }
            });

            NavigationMenuViewComponent target = new NavigationMenuViewComponent(mock.Object);

            string[] results = ((IEnumerable<string>)(target.Invoke() as ViewViewComponentResult)
                .ViewData.Model).ToArray();

            Assert.True(Enumerable.SequenceEqual(new string[] { "Apples", "Oranges", "Plums" }, results));

        }
        
        
    }
}
