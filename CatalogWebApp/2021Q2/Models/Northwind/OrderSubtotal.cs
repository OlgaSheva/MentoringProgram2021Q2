﻿using System;
using System.Collections.Generic;

#nullable disable

namespace _2021Q2.Models.Northwind
{
    public partial class OrderSubtotal
    {
        public int OrderId { get; set; }
        public decimal? Subtotal { get; set; }
    }
}
