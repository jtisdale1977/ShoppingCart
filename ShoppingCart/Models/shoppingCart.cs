using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCart.Models
{
    public class shoppingCart
    {
        public int Id { get; set; }

        public int ItemId { get; set; }

        public string MediaURL { get; set; }

        public string Name { get; set; }

        [AllowHtml]
        public string Description { get; set; }

        public decimal Price { get; set; }

        public virtual Item Item { get; set; }

        public int Count { get; set; }

        public DateTime CreationDate { get; set; }

        public string CustomerId { get; set; }

        public int OrderId { get; internal set; }

    }
}