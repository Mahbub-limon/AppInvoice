using System;
using System.Collections.Generic;
using System.Text;

namespace AppInvoice.Models
{
    public class Order
    {
        public string? OrderId { get; set; }
        public string? CustomerId { get; set; }

        public List<OrderDetail>? OrderDetails { get; set; }= new List<OrderDetail>();
    }
}
