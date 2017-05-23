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
    public class shoppingCartsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: shoppingCarts
        [Authorize]
        public ActionResult Index()
        {
            var myId = User.Identity.GetUserId();
            var user = db.Users.Find(myId);
            var shoppingCarts = db.ShoppingCarts.Where(s => s.CustomerId == user.Id).ToList();
            shoppingCart ShoppingCart = new shoppingCart();
            ShoppingCart.CustomerId = user.Id;
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
            return View(shoppingCarts);
        }

        // GET: shoppingCarts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            shoppingCart shoppingCart = db.ShoppingCarts.Find(id);
            if (shoppingCart == null)
            {
                return HttpNotFound();
            }
            return View(shoppingCart);
        }

        #region shoppingCarts Create GET and POST old code
        //GET: shoppingCarts/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        // POST: shoppingCarts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,ItemId,Count,CreationDate")] shoppingCart shoppingCart)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.ShoppingCarts.Add(shoppingCart);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(shoppingCart);
        //}
        #endregion

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int Itemid)
        {
            //This code presupposes that the user is logged in. User.Identity.GetUserId acts upon a logged in user...
            var user = db.Users.Find(User.Identity.GetUserId());

            //If the user is not logged in this line of code will throw an exception because user.Id is null
            var shoppingCart = db.ShoppingCarts.Where(s => s.CustomerId == user.Id && s.ItemId == Itemid).ToList();

            //If and only if the Users cart does not already have this particular item we will execute this code 
            if (shoppingCart.Count == 0)
            {
                var item = db.Items.Find(Itemid);
                shoppingCart shoppingCart1 = new shoppingCart();
                shoppingCart1.CustomerId = user.Id;
                shoppingCart1.MediaURL = item.MediaURL;
                shoppingCart1.Name = item.Name;
                shoppingCart1.Price = item.Price;
                shoppingCart1.ItemId = Itemid;
                shoppingCart1.Item = db.Items.FirstOrDefault(i => i.Id == Itemid);
                shoppingCart1.Count = 1;
                shoppingCart1.CreationDate = System.DateTime.Now;
                db.ShoppingCarts.Add(shoppingCart1);
                db.SaveChanges();
                return RedirectToAction("Index", "shoppingCarts");
            }

            //Otherwise we will execute this code block which will simply increment the item count
            foreach (var items in shoppingCart)
            {
                items.Count++;
                db.Entry(items).Property("Count").IsModified = true;
            };

            db.SaveChanges();

            return RedirectToAction("Index", "shoppingCarts");

        }

        // GET: shoppingCarts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = db.Users.Find(User.Identity.GetUserId());

            shoppingCart shoppingCart = db.ShoppingCarts.Find(id);
            if (shoppingCart == null)
            {
                return HttpNotFound();
            }
            return View(shoppingCart);
        }

        // POST: shoppingCarts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,ItemId,Count,MediaURL,CreationDate,Price")] shoppingCart shoppingCart)
        {
            var items = db.Items.ToList();
            var user = db.Users.Find(User.Identity.GetUserId());

            shoppingCart.CustomerId = user.Id;
            shoppingCart.Item = db.Items.Find(shoppingCart.ItemId);
            if (ModelState.IsValid)
            {
                db.ShoppingCarts.Attach(shoppingCart);
                db.Entry(shoppingCart).Property("Count").IsModified = true;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(shoppingCart);
        }

        // GET: shoppingCarts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            shoppingCart shoppingCart = db.ShoppingCarts.Find(id);
            if (shoppingCart == null)
            {
                return HttpNotFound();
            }
            return View(shoppingCart);
        }

        // POST: shoppingCarts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            shoppingCart shoppingCart = db.ShoppingCarts.Find(id);
            db.ShoppingCarts.Remove(shoppingCart);
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

        public ActionResult Quantity(int Itemid, FormCollection collection)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var shoppingCart = db.ShoppingCarts.Where(s => s.CustomerId == user.Id && s.ItemId == Itemid).ToList();

            foreach (var items in shoppingCart)
            {
                var value = collection["quantity"];
                items.Count = Int32.Parse(value);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "ShoppingCart");
        }
    }
}