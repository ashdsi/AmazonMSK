using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Producer.Models;
using Producer.Services;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Producer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IConfiguration _configuration;

        public OrderController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // POST api/order
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] OrderRequest value)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = _configuration["producer:bootstrapservers"]
            };

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Serialize 
            string serializedOrder = JsonConvert.SerializeObject(value);

            Console.WriteLine("========");
            Console.WriteLine("Info: OrderController => Post => Recieved a new purchase order:");
            Console.WriteLine(serializedOrder);
            Console.WriteLine("=========");

            string topicname = _configuration["producer:topicname"];

            var producer = new ProducerService(config, topicname);
            await producer.writeMessage(serializedOrder);

            return Created("Transaction Id", "Your order is in progress");
        }
    }
}
