# WorkingWithRabbitMq

## 1. Installation
- Erlang download:

  [https://erlang.org/download/otp_versi...](https://erlang.org/download/otp_versions_tree.html)

  after installation go to
  
  - Controllpannel/System/Advanced system settings/Environment Variables
    
  then click New and then fill in :
  - Variable Name:ERLANG_HOME
  - Variable value:{erlang installation path}

  to be sure about variable creation type in cmd:
  - ECHO %ERLANG_HOME%

  and you will get the variable value which is erlang installation path.

- RabbitMQ Github download:

  - go to :
    [https://github.com/rabbitmq/rabbitmq-...](https://github.com/rabbitmq/rabbitmq-server/)
  - download RabbitMq zip file from Release section on right pans.
  - Once the download is completed, extract it to our desired destination. Inside the extracted folder, Open CMD and go to ```Sbin``` folder.
  - to start rabbitmq server run this command line:
    * rabbitmq-server.bat
  - to see managment plugin navigate to
    * localhost:15672
    * with user and password both "guest"
    
- Chocolaty download:
  * first download chocoloty from
    - [https://chocolatey.org/install#instal...](https://chocolatey.org/install#install-step2)
  * then run this cmd command:
    ``` Set-ExecutionPolicy Bypass -Scope Process -Force; [System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072; iex ((New-Object System.Net.WebClient).DownloadString('https://community.chocolatey.org/install.ps1'))```
  * If you don't see any errors, you are ready to use Chocolatey! Type ```choco``` or ```choco -?``` now
  * then run cmd command to install rabbitmq:
    ``` choco install rabbitmq ```

    this will install rabbit mq and erlang as well.

- Docker run command:

  ``` docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.9-management```

## 2. Core Concepts

When we talk about rabbitmq,these include:
- Brokers
- Exchanges
- Queues
- Producers

and we real need to know what they are and how they interact.
Rabbitmq itselfe is often called **'Message Broker'** . if we compare it to a real life example, we might think of the broker, kind of a postal service. It is responsible for getting our letter from **A** to **B**, In the same way that rabbitMq is responsible for getting our messages from **Consumer** to **Producer**.

We also have the ideas of **Producers** and **Consumers** in rabbitmq:
- A **Producer** is something that is publishing a message into rabbitmq. In the post office example we can think of a producer as someone who is posting a letter. They just want to drop the letter off safe in the knowledge it will get to its final destination.
- The **MessageBroker**  or in the postal example the postal service knows how to forward our message to its final destination.
- When we drop off a message in the post office, we dont need to hang around all day to make sure that our message gets delivered. Instead we can go about our day safe in the knowledge that the postal service will take care of this for us and if we receive a reply, it will come into our letter box. We dont need to go back to the post office every single day to check if anything has arrived for us. The same is true in rabbitmq and this is what's known as **Asynchronous Comunication**.
- **Synchronouse Comunication** will be something like making a http get request for a web based Api. We would have to wait around until the Api responds and often cant do anything until it does. This can end up eating alot of resources in our system
- We then also have **Consumers** which are the entities or programs listening to messages that come off the message broker.
- We can have **Multiple Consumers** listening to messages off the message broker, also we can also have **Multiple Producers**, pushing messages into the same message broker.
- Both ways the comunication is **Asynchronous** which means producers dont have to wait for message to be delivered and consumers dont have to wait untile messages get sent.

### Exchanges
To undrestand how the message broker gets a message from producer to consumer, we will have to look at **Exchanges** and **Queues** to see how they fit into our system. 

- An **Exchange** is the brains behind rabbitmq. It knows how we want to route our message from producer to consumer. We can think of an exchange like the inner workings of the postal service. Say for instance like a single postal office.Once it gets a letter, it looks at the address or any other metadata and make decisions on how to proceed and who gets the letter.
- There are many diffrent types of exchanges to cover many diffrent types of scenarios, just like the way the postal service has many diffrent types of postal office that offer many diffrent types of services, such as express posts, registered posts or even a service that posts a letter to multiple people. A **Message Broker** might have many diffrent **Exchanges** and an exchange is what a producer always sends its message to.
- The other core concept is that of a **Queue**. An exchange will push messages into one or more queues. Messages will sit in these queues until they are read or consumed by an intrested party. We might think of a queue kind of our letterbox. It might receive multiple envelopes or messages into it throughout the day. We might check it from time to time and read those messages or we might leave them there to build up over time a consumer is responsible for reading the message in its queue. The same way a person is responsible for opening the letters int its letterbox.
- Like an exchange a message broker might have multiple queues. Queues are tied to exchanges in what is known as a **Binding**.

### Queues
An exchange can be tied to many queues and also a queue can be tied to many exchanges. 

In this example we can see that some queues are tied up to a single exchange but other queues are tied to multiple exchanges.
![image](https://github.com/Tajoreh/WorkingWithRabbitMq/assets/20103416/ebd4bc61-e619-4648-88c8-d4b228973ae7)

We might even have the case where queue is tied to no exchanges. In this case the queue will not receive any messages from a producer. 
![image](https://github.com/Tajoreh/WorkingWithRabbitMq/assets/20103416/239ba70a-66ab-4d00-8a51-bc36148e1131)

Consumers listen to messages that are pushed on to queues. 
![image](https://github.com/Tajoreh/WorkingWithRabbitMq/assets/20103416/ee448115-f918-426b-8d70-6f06deb104f9)

Again a consumer might not listen to any messages or might listen to messages from multiple queues. In this case we can see that our first consumer, consumes of 3 diffrent queues but the second, only consumes of one.
![image](https://github.com/Tajoreh/WorkingWithRabbitMq/assets/20103416/832f9518-e9ef-45d1-8158-4c4b5090d520)

With diffrent types of exchanges and using bindings and queues, rabbitmq  gives us alot of flexibility in how we set up our message broker. We can duplicate messages by sending them from an exchange onto multiple queues or we can make sure only one queue ever gets a massage from an exchange.

Two other terms that might be mentioned frequently in the context of rabbitmq are **Connections** and **Channels**. Every producer or consumer should open a TCP connection to our rabbitmq broker. A connection however can have multiple channels. 

By using a connection with multiple channels, a producer for example, might be able to produce and push messages onto our message broker using diffrent threads. But because each thread uses a diffrent channel, these messages are isolated from one another. By using channels, and not opening multiple connections, we can save a lot of resources. The same is true for consumers who will only have one connection but might have multiple channels.


## 3. First Program

Before we get start writing our code, we'll give a brief introduction on what we are trying to achive. All we are trying to achive is to publish a message into our rabbitmq broker from our producer and then consume that message from a consumer. So we will have a **Producer** and a **Consumer**. The consumer will consume the message from a queue. As we disscussed in previous part, the queue is like a post box for the consumer and will listen to messages from this queue. We have to give the queue a name and in this example we will call our queue *LetterBox* .

In rabbitmq we can ***NEVER*** publish a message directly to a queue. It has to go through an exchange. In this very simple example the exchange is not that important, so we can just use the *Default Exchange* which is represented by a blank string. So the producer will push a message onto the *Default Exchange* and then *Default Exchange* will push that message into our LetterBox queue and finnally , as we have seen, the consumer will consume the message from  the LetterBox queue.

![image](https://github.com/Tajoreh/WorkingWithRabbitMq/assets/20103416/8d2eaa23-0a3a-439d-83eb-0467a71b2abe)

## 4.AMQP for RabbitMQ

Before we move on and look at more complicated examples, we are going to take a quick detour and look at rabbitmq's relationship with the advanced message queuing protocol or **AMQP**. While knowledge of AMQP is not essential for using rabbitmq, a decent grounding in it is often useful when trying to undrestand some of rabbit's concepts or debugging tricky issues.

[**AMQP**](https://www.amqp.org/) is an open standard for passing messages between applications or organazations and rabbitmq is not the only message broker that uses AMQP. Many other brokers including:
- Apache ActiveMq
- Azure Service Bus
  
make use of AMQP. AMQP uses a *remote procedure call pattern* to allow one computer, for example *the client*, to execute programs or methods on another computer, say the *Broker*. This comunication is 2-way and both, the *broker* and *the client*, can use to run programs or call methods on each other. In a similar way to object-oriented languages you might be familiar with, rabbitmq uses commands which consist of classes and methods to comunicate between clients and the broker. For example we might send an exchange declare command that tells the broker we want to create a new exchange. In this example the exchange is the class and declare is the method when a command is sent to or from a rabbitmq broker. All the data required to execute the command is included in a data structure called a **Frame**. **Frames** have a standard structure and we will go through each of the frame types. There are 4 frame types defined by AMQP:
- Method Frame
- Content Header Frame
- Body Frame
- Heartbeat Frame

Each frame folows a common structure. 

![image](https://github.com/Tajoreh/WorkingWithRabbitMq/assets/20103416/e5ea42b3-f91c-4e6d-9ec1-e3e1ca472738)

Every frame starts with a byte that represents the *Frame Type* that is being sent. In this case the number "1" says that this AMQP frame is a method frame. This is followed by *two bytes* which represent *the Channel* that the frame is being sent on. In this case we can see, we are sending the frame on channel "12". Next we have *4 bytes* that represent the size of the message. Sending the size of the message like this, allows us to determine how big the message will be before we have to process it. Next becomes *the Frame Specific Content* which we will talk about in turn for each frame. Finally we have *a byte* representing that *the frame has ended*. The size of the message excludes the frame end byte. Using the frame end byte , in conjunction with the size, allows us to verify the frame specific content and make sure it is accurate and nothing has been corrupted. 

The AMQP protocol, defines exactly what sould be contained in each byte of a frame. For example here we can see that the frame type should be the very first or the zeroth byte of our frame. Channel should always take up the first and second bytes and size should take up the 3rd, 4th, 5th and 6th bytes. The frame specisifc content should run between the 7th byte and the *size defined + 7*, while the frame end is always the last byte which is *size +8*.

### Method Frame
First we will look at the specifics of a method frame. The method frame, is what has the class and method we discussed earlier. Like a core frame the frame spesific content for a method frame follows a very strict structure. The first thing in the frame spesific content of a method frame is a number of bytes representing the class in question.

in this case we can see that the bytes are 40 which represents the exchange class. Next we have a number of bytes that represent the method. In this case the number 10 which represents to declare method on the exchange class. So the method we are calling here is *exchange declare*. 

The [docs](https://www.amqp.org/sites/amqp.org/files/amqp0-9-1.zip) for the amqp specification give us a full rundown of which ids are for which methods as well as a short description of what each method is expected to do. 

![image](https://github.com/Tajoreh/WorkingWithRabbitMq/assets/20103416/6aea25af-116a-4b05-b4c9-674add235198)

we can see here that for connection the class id is 10 and the various different methods for that class include *start* which is also method id 10, *open* method id 40. And we can see there's a ton of other different methods and classes defined by the AMQP specification. After the class and method ids, we need to send a number of arguments specific for that method. The arguments we have to send are again defined in the amqp specification. Looking at the specification for the method in question, *exchange declare* we can see the number of different arguments that we are expected to send as part of this method. 

![image](https://github.com/Tajoreh/WorkingWithRabbitMq/assets/20103416/b6fd498e-bd00-4de2-98d5-5ecbb8e69a62)

Each argument has a parameter name and then a value and has to be passed in a specific order. In the case of *exchange declare* we can see that there are 9 different arguments passed with various different values and carrying various different
meanings. Some of these are reserved but other
ones contain data around:
- type of exchange
- what the exchange name is
- whether it's a durable exchange

  ![image](https://github.com/Tajoreh/WorkingWithRabbitMq/assets/20103416/489e4a20-e017-482e-a542-ee3111062e57)

The amqp specification contains full details on how to process all of these arguments and how they should be formatted. For example *long strings* have a 32 bit representing their length at the front, followed by zero or more bytes of data. 

The specification also gives details around how other different types such as timestamps and field tables can be created and processed. finally the *Arguments* make up the remainder of an AMQP method frame. 

When sending or receiving a message through rabbitmq, the first frame always sent or received is a method frame which has a method that corresponds to sending or receiving a message. For instance, if we want to send a message to a rabbitmq broker, we will first send the method frame *basic publish* or equivalent. After we send this method frame it has to be preceded by a content header frame. This carries the message properties and tells rabbitmq how big the message body is and how many body frames will precede it. Similar to a method frame a content frame is broken up into several different sections, each with a predefined purpose and length in the AMPQ specification
  


