using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCart.Models
{
    public class Item
    {
        public int Count { get; set; }

        public int Id { get; set; }

        public DateTimeOffset Created { get; set; }

        public DateTimeOffset? Updated { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string MediaURL { get; set; }

        //public string OtherImg { get; set; }

        //public string Thumbnail { get; set; }

        [AllowHtml]
        public string Description { get; set; }
    }
}