* Lets assume you want to setup local jenkins container where you have jenkins job that need to be cloning local git repository.
* On your local host system, let's say you are logged in as `alice` where you have checked out this repository from github inside a folder called /home/alice/programming_fun/TestRepo. 
* You have correctly setup your local jenkins container that can run jenkins pipeline. 
* Now, you want to setup a jenkins job inside your running local jenkins container that should checkout code of local git repository that you setup in the step 2. 
* The local jenkins container is running jenkins as `jenkins` user. So, you need to allow this user to access your local git repository that is residing in folder /home/alice/programming_fun/TestRepo.

* First of all, we make sure that your ubuntu box is equipped to allow incoming ssh connections. To make sure it can, first install openssh-server
    ```bash
    $ sudo apt update
    $ sudo apt install openssh-server
    ```
* Now, check the status of ufw. It is ubuntu firewall program. You need to allow port 22 (ssh port) to be able to get through ufw.
    ```bash
    $ sudo ufw enable
    $ sudo ufw allow 22/tcp
    $ sudo ufw reload
    $ sudo ufw status verbose
    ```
* With this, the ubuntu box should allow ssh incoming connection. That is one layer solved. Now, need to solve next layer of copying ssh key to `alice` directory where you are logged in as user on local host system.
* For this, execute following command to generate ssh key on jenkins container. FYI: you will log in as jenkins user because jenkins job will be executed as jenkins user so need to go with the same user to generate ssh key. Just press enter for all prompted questions.
    ```bash
    $ docker exec -it jenkins_local /bin/bash
    $ ssh-keygen
    $ ssh-copy-id alice@<local host ip address> -v
    ```

* Overall idea: Jenkins user will need to checkout git repository residing at /home/alice/programming_fun/TestRepo. So, jenkins user need to copy its public SSH key to .ssh folder on alice user.
* This is the reason why you need to copy key with `alice` username. 
* All you need to do now is to get the private key of corresponding public key: 
    ```bash
    $ cat /var/jenkins_home/.ssh/id_rsa
    ```
    Copy this private key into jenkins as `SSH Username with Private key`
