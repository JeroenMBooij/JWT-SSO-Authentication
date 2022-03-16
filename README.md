# Authentication Server

This repository contains the source code for an API where developers can manage their authentication needs. A Developer can use this API to design multiple JWT's to fit needs and assign it to their applications. Furthermore, with the refresh endpoint you can switch between JWT designs incase your application communicates with multiple services which require different JWT's. This scheme allows a user to log in with a single ID and password to any of several related, yet independent, software systems.

<h1> Usage </h1>
  * prerequisite - docker installed <br/>
  ** optional - this application depends on <a href="https://github.com/JeroenMBooij/EmailService" target="_blank">my email service</a> repository for all email functionality
  <br/>
  <br/>
  <b>steps</b>
   <p> 1. Override secrets in docker-compose with a docker-compose.vs.debug.yml file or define the secrets as environment variables in your pipeline</p>
   <p> 2. run docker-compose -f docker-compose.yml -f docker-compose.override.yml -f docker-compose.vs.debug.yml up -d</p>
   <p> 3. open localhost:3000</p>

<h4>docker-compose.vs.debug.yml example file</h4>

```

version: '3.8'

services:

  authenticationserver.web:
    environment:
      DB_HOST: identitydb
      DB_NAME: IdentityDb
      DB_USER: sa
      DB_PASSWORD: [your password]
      JWT_SECRETTKEY: [your secret PKCS #8 key ]
      JWT_ISSUER: [You]
      EMAIL_APPKEY: [Your Google Email app key]

  identitydb:
    environment:
      - SA_PASSWORD=SeCret1234
      
```

<h1>design</h1>

The application software architecture is designed using a clean code approach with Dependency Inversion Principle and Domain-Driven Design


<img src="https://i.imgur.com/Toqkgg9.png" width="1000" style="margin-left: auto; margin-right:auto;"/>

<p><b>Web layer | API layer</b> contains contains all controllers for endpoints and is responsible for handling all the requests made to the API</p> <br>

<p><b>Domain layer</b> contains contains all the definitions for the database entities</p> <br>

<p><b>Persistance layer</b> contains the data access definition. By using a repository pattern the data can be manipulated by the logic layer</p> <br>

<p><b>Service layer</b> contains all external services like the email service</p><br>

<p><b>Common layer</b> contains all request en response models, as well as all the interfaces so every layer knows what the application can do without having a dependency on any of the higher level layers. This way the Logic layer can use the functionality from the Persistance or Service layers without having a tight coupling between the layers</p><br>

