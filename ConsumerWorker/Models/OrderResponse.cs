﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Consumer.Models
{
    public class OrderResponse
    {

            public int id { get; set; }
            public string productname { get; set; }
            public int quantity { get; set; }


        public OrderStatus status { get; set; }

        public enum OrderStatus
        {
            IN_PROGRESS,
            COMPLETED
           
        }
    }
}
