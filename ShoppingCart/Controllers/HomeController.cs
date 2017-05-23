using ShoppingCart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCart.Controllers
{
    public class HomeController : Controller
    {

        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var count = db.ShoppingCarts.Count();
            if (count == 0)
            {
                ViewBag.Message = 0;
            }
            else
            {
                ViewBag.count = db.ShoppingCarts.Sum(c => c.Item.Count + c.Count);
                ViewBag.Cart = ViewBag.count;
                ViewBag.num = db.ShoppingCarts.Sum(m => m.Item.Price * m.Count);
                ViewBag.total = ViewBag.num;
            }
            return View();
        }

        public ActionResult cart()
        {
            var count = db.ShoppingCarts.Count();
            if (count == 0)
            {
                ViewBag.Message = 0;
            }
            else
            {
                ViewBag.count = db.ShoppingCarts.Sum(c => c.Item.Count + c.Count);
                ViewBag.Cart = ViewBag.count;
                ViewBag.num = db.ShoppingCarts.Sum(m => m.Item.Price * m.Count);
                ViewBag.total = ViewBag.num;
            }
            return View();
        }

        public ActionResult checkout()
        {
            return View();
        }

        public ActionResult shop()
        {
            var count = db.ShoppingCarts.Count();
            if (count == 0)
            {
                ViewBag.Message = 0;
            }
            else
            {
                ViewBag.count = db.ShoppingCarts.Sum(c => c.Item.Count + c.Count);
                ViewBag.Cart = ViewBag.count;
                ViewBag.num = db.ShoppingCarts.Sum(m => m.Item.Price * m.Count);
                ViewBag.total = ViewBag.num;
            }
            return View(db.Items.ToList());
        }

        public ActionResult singleproduct(int id)
        {
            var count = db.ShoppingCarts.Count();
            if (count == 0)
            {
                ViewBag.Message = 0;
            }
            else
            {
                ViewBag.count = db.ShoppingCarts.Sum(c => c.Item.Count + c.Count);
                ViewBag.Cart = ViewBag.count;
                ViewBag.num = db.ShoppingCarts.Sum(m => m.Item.Price * m.Count);
                ViewBag.total = ViewBag.num;
            }
            var product = db.Items.Find(id);
            return View(product);
        }

        public ActionResult Contact()
        {
            var count = db.ShoppingCarts.Count();
            if (count == 0)
            {
                ViewBag.Message = 0;
            }
            else
            {
                ViewBag.count = db.ShoppingCarts.Sum(c => c.Item.Count + c.Count);
                ViewBag.Cart = ViewBag.count;
                ViewBag.num = db.ShoppingCarts.Sum(m => m.Item.Price * m.Count);
                ViewBag.total = ViewBag.num;
            }
            return View();
        }
    }
}