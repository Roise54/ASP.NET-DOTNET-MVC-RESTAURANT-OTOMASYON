using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestoranOtomasyonu.Data;
using RestoranOtomasyonu.Models;

namespace RestoranOtomasyonu.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems!)
                    .ThenInclude(oi => oi.Product!)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
            return View(orders);
        }

        public IActionResult Create()
        {
            ViewBag.Products = _context.Products
                .Include(p => p.Category!)
                .OrderBy(p => p.Category!.Name)
                .ToList();

            var order = new Order
            {
                OrderDate = DateTime.Now,
                Status = OrderStatus.Preparing
            };

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Order order, int[] productIds, int[] quantities)
        {
            if (productIds != null && quantities != null && productIds.Length == quantities.Length)
            {
                order.OrderDate = DateTime.Now;
                order.Status = OrderStatus.Preparing;

                for (int i = 0; i < productIds.Length; i++)
                {
                    if (quantities[i] > 0)
                    {
                        var product = await _context.Products.FindAsync(productIds[i]);
                        if (product != null)
                        {
                            order.OrderItems.Add(new OrderItem
                            {
                                ProductId = product.Id,
                                Quantity = quantities[i],
                                UnitPrice = product.Price
                            });
                        }
                    }
                }

                order.TotalAmount = order.OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice);

                if (order.OrderItems.Any())
                {
                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { id = order.Id });
                }
            }

            ViewBag.Products = await _context.Products
                .Include(p => p.Category!)
                .OrderBy(p => p.Category!.Name)
                .ToListAsync();

            ModelState.AddModelError("", "Lütfen en az bir ürün seçin");
            return View(order);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.OrderItems!)
                    .ThenInclude(oi => oi.Product!)
                        .ThenInclude(p => p.Category!)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            ViewBag.Products = await _context.Products
                .Include(p => p.Category!)
                .OrderBy(p => p.Category!.Name)
                .ToListAsync();

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> AddItem(int orderId, int productId, int quantity)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems!)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            var existingItem = order.OrderItems
                .FirstOrDefault(oi => oi.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                order.OrderItems.Add(new OrderItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    UnitPrice = product.Price
                });
            }

            order.TotalAmount = order.OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = order.Id });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveItem(int orderId, int orderItemId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems!)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return NotFound();
            }

            var orderItem = order.OrderItems
                .FirstOrDefault(oi => oi.Id == orderItemId);

            if (orderItem != null)
            {
                order.OrderItems.Remove(orderItem);
                order.TotalAmount = order.OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice);
                
                // Eğer son ürün de silindiyse ve sipariş hazırlanıyor durumundaysa
                if (!order.OrderItems.Any() && order.Status == OrderStatus.Preparing)
                {
                    _context.Orders.Remove(order);
                }
                
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Details), new { id = order.Id });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int orderId, OrderStatus status)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return NotFound();
            }

            order.Status = status;
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }
    }
} 