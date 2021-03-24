using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Producer.Models
{
    public class OrderRequest
    {
            public int id { get; set; }
            public string productname { get; set; }
            public int quantity { get; set; }

    }
}
