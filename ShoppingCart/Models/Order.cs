﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Models
{
    public class Order
    {
        public int Id { get; set; }

        public bool Completed { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string Zipcode { get; set; }

        [Required]
        public string Country { get; set; }

        public string Phone { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal Total { get; set; }

        public string CustomerId { get; set; }

        public virtual Item Item { get; set; }

        public virtual ApplicationUser Customer { get; set; }

        public virtual ICollection<OrderDetails> OrderDetails { get; set; }

    }
}