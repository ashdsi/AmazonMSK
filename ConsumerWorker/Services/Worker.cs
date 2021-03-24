using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Confluent.Kafka;
using Consumer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Consumer.Models.OrderResponse;

namespace ConsumerWorker
{
    public class Worker : BackgroundService
    {

        private IAmazonSimpleNotificationService _sns;
        private IConfiguration _configuration;
        public Worker(IAmazonSimpleNotificationService sns, IConfiguration configuration)
        {
            _sns = sns;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                GroupId = _configuration["consumer:groupid"],
                BootstrapServers = _configuration["consumer:bootstrapservers"],
                // Note: The AutoOffsetReset property determines the start offset in the event
                // there are not yet any committed offsets for the consumer group for the
                // topic/partitions of interest. By default, offsets are committed
                // automatically, so in this example, consumption will only start from the
                // earliest message in the topic 'my-topic' the first time you run the program.
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            string topicName = _configuration["consumer:topicname"];

            Console.WriteLine("OrderProcessing Service Started");

            using (var builder = new ConsumerBuilder<Ignore, string>(config).Build())
            {

                builder.Subscribe(topicName);


                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = builder.Consume();
                        string orderrequest = consumeResult.Message.Value;

                        //Deserilaize
                        OrderResponse order = JsonConvert.DeserializeObject<OrderResponse>(orderrequest);

                        //TODO::Process Order
                        Console.WriteLine($"Info: OrderHandler => Processing the order for {order.productname}");
                        order.status = OrderStatus.IN_PROGRESS;
                        Console.WriteLine($"The order is {order.status}");

                        //Write to SNS
                        var request = new PublishRequest()
                        {
                            Subject = "Order Placed successfully",
                            Message = orderrequest,
                            TopicArn = _configuration["consumer:snstopicarn"]
                    };
                        var response = await _sns.PublishAsync(request);
                        Console.WriteLine($"The message ID is {response.MessageId}");
                        order.status = OrderStatus.COMPLETED;
                        Console.WriteLine($"The order is {order.status}");

                    }

                    catch (ConsumeException e)
                    {
                        Console.WriteLine($"Error occured: {e.Error.Reason}");
                    
                    }


                }
            }
        }
    }
}
