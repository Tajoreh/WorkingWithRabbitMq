//6. Create a connection

using System.Reflection;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();

//7. Create a channel
using var channel = connection.CreateModel();

//8. Declare a queue
channel.QueueDeclare(
    queue: "LetterBox",
    durable: false,
    exclusive: false,
    autoDelete: false,
    arguments: null);

//9. Create a consumer which listens through channel
var consumer=new EventingBasicConsumer(channel);

//10. Create a callback for when it receives the message
consumer.Received += (model, eventArgs) =>
{
    var body=eventArgs.Body.ToArray();
    var message=Encoding.UTF8.GetString(body);
    Console.WriteLine($"Message Received: {message}");

};


//11. Consumes to the messages
channel.BasicConsume(queue:"LetterBox",autoAck:true,consumer:consumer);

Console.ReadKey();