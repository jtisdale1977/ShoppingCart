using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShoppingCart.Models;
using Microsoft.AspNet.Identity;

namespace ShoppingCart.Controllers
{
    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Orders
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
            var orders = db.Orders.Include(o => o.Customer).Include(o => o.OrderDetails).Include(o => o.Item);
            return View(orders.ToList());
        }

        ////POST: Complete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Complete(int? Itemid)
        {
            var orderdetail = db.Orders.Where(x => x.Id == Itemid).Include("OrderDetails").Include("Items");

            var userid = User.Identity.GetUserId();
            var completeOrder = db.Orders.Find(Itemid);
            var shoppingcarts = db.ShoppingCarts.Where(s => s.CustomerId == userid);
            db.Orders.Remove(completeOrder);
            if (shoppingcarts != null)
            {
                foreach (var shopping in shoppingcarts)
                {
                    db.ShoppingCarts.Remove(shopping);
                }
            }
            db.SaveChanges();
            return View ();
        }

    // GET: Orders/Details/5
    public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
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

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Address,City,State,Zipcode,Country,Phone")] Order order)
        {
            //This code presupposes that the user is logged in. User.Identity.GetUserId acts upon a logged in user...
            var user = db.Users.Find(User.Identity.GetUserId());

            //If the user is not logged in this line of code will throw an exception because user.Id is null
            var shoppingCart = db.ShoppingCarts.Where(s => s.CustomerId == user.Id).ToList();
            decimal totalAmt = 0;
            if (shoppingCart.Count != 0)
            {
                if (ModelState.IsValid)
                {
                    foreach (var product in shoppingCart)
                    {
                        OrderDetails orderdetail = new OrderDetails();
                        orderdetail.ItemId = product.ItemId;
                        orderdetail.OrderId = product.OrderId;
                        orderdetail.Quantity = product.Count;
                        orderdetail.UnitPrice = product.Item.Price;
                        totalAmt += (product.Count * product.Item.Price);

                        db.OrderDetails.Add(orderdetail);
                    }
                    order.Total = totalAmt;
                    order.Completed = false;
                    order.OrderDate = DateTime.Now;
                    order.CustomerId = user.Id;
                    db.Orders.Add(order);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
                return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.ApplicationUsers, "Id", "FirstName", order.CustomerId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Completed,Address,City,State,Zipcode,Country,Phone,OrderDate,Total,CustomerId")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.ApplicationUsers, "Id", "FirstName", order.CustomerId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
