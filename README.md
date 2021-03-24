# Amazon MSK

This project has .NET Core producer and consumer clients for Amazon MSK
It also has integration with Amazon SNS 

## Prerequisites

1. Create Amazon MSK Cluster with two private subnets (for test set encryption to plaintext when creating cluster)
#Broker for plaintext communication uses port 9092

2. Use Kafka client to create a topic in the cluster 
https://docs.aws.amazon.com/msk/latest/developerguide/create-topic.html

3. Create the following SSM parameters 

For Producer:
=========
/amazonmsk/kafkaconfig/producer/bootstrapservers       #make sure to add value as brokername:port
/amazonmsk/kafkaconfig/producer/topicname

For ConsumerWorker:
===============
/amazonmsk/kafkaconfig/consumer/groupid
/amazonmsk/kafkaconfig/consumer/bootstrapservers
/amazonmsk/kafkaconfig/consumer/topicname
/amazonmsk/kafkaconfig/consumer/snstopicarn

4. Run the Producer app in private subnet
5. Run the ConsumerWorker app in private subnet

6. API request path: http://<hostname>:<port>/api/order

HTTP POST with request body
{
    "id": 1,
    "productname": "Chips",
    "quantity": 10
}







