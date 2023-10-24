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
