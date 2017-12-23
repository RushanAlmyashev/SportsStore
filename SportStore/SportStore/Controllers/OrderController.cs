using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SportStore.Models;
using System.Linq;


namespace SportStore.Controllers
{
    public class OrderController :Controller
    {
        private IOrderRepository repository;
        private Cart cart;

        public OrderController(IOrderRepository repo, Cart cartService)
        {
            repository = repo;
            cart = cartService;
        }

        public ViewResult List()
        {
            return View(repository.Orders.Where(o => !o.Shipped));
        }

        public IActionResult MarkShipped(int orderId)
        {
            Order order = repository.Orders.Where(o => o.OrderID == orderId).FirstOrDefault();
            if (order != null)
            {
                order.Shipped = true;
                repository.SaveOrder(order);
            }
            return RedirectToAction(nameof(List));
        }

        public ViewResult Checkout()
        {
            return View(new Order());
        }

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }
            if (ModelState.IsValid)
            {
                order.Lines = cart.Lines.ToArray();
                repository.SaveOrder(order);
                return RedirectToAction(nameof(Completed));
            }
            else
            {
                return View(order);
            }
        }

        public ViewResult Completed()
        {
            cart.Clear();
            return View();
        }
    }
}
