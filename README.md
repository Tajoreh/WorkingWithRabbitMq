# WorkingWithRabbitMq
WorkingWithRabbitMq

*1. Installation*
-----------------------------------------------------------------------------------------------------------------------
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
-----------------------------------------------------------------------------------------------------------------------
*2. Core Concepts*
-----------------------------------------------------------------------------------------------------------------------
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

