using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Producer.Services
{
    public class ProducerService
    {
        private string _topicName;
        private ProducerConfig _producerConfig;

        public ProducerService(ProducerConfig producerConfig, string topicName)
        {
            _topicName = topicName;
            _producerConfig = producerConfig;
        }
        public async Task writeMessage(string message)
        {
            using (var producer = new ProducerBuilder<Null, string>(_producerConfig).Build())
            {

                try
                {
                    var dr = await producer.ProduceAsync(_topicName, new Message<Null, string>()
                    {
                        Value = message
                    });
                    Console.WriteLine($"KAFKA => Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
                }


                catch (Exception ex)
                {
                    Console.WriteLine("Application Crashed: " + ex.Message);
                }
            }

        }
    }
}
