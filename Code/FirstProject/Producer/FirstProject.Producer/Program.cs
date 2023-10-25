//1. Create a connection
using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();

//2. Create a channel
using var channel=connection.CreateModel();

//3. Declare a queue
channel.QueueDeclare(
    queue:"LetterBox",
    durable:false,
    exclusive:false,
    autoDelete:false,
    arguments:null);

//4. Create a message
var message="This is my first Message";
var encodedMessage=Encoding.UTF8.GetBytes(message);

//5. Publish the message: "" means default exchange
channel.BasicPublish(exchange:"",routingKey:"LetterBox",basicProperties:null,body:encodedMessage);

