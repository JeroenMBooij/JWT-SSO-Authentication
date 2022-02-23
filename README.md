# Authentication Server

This repository contains the source code for an API where developers can manage their authentication needs. A Developer can use this API to design multiple JWT's to fit needs and assign it to their applications. Furthermore, with the refresh endpoint you can switch between JWT designs incase your application communicates with multiple services which require different JWT's. This scheme allows a user to log in with a single ID and password to any of several related, yet independent, software systems.

<h1> Usage </h1>
  * prerequisite - docker installed <br/>
  ** optional - this application depends on <a href="https://github.com/JeroenMBooij/EmailService" target="_blank">my email service</a> repository for all email functionality
  <br/>
  <ul>
   <li> 1. Override secrets in docker-compose with a docker-compose.vs.debug.yml file</li>
   <li> 2. run docker-compose -f docker-compose.yml -f docker-compose.override.yml -f docker-compose.vs.debug.yml up -d</li>
   <li> 3. open localhost:3000</li>
 <ul/>

<h4>docker-compose.vs.debug.yml example file</h4>
```
version: '3.4'

services:

  authenticationserver.web:
    environment:
      DB_HOST: identitydb
      DB_NAME: IdentityDb
      DB_USER: sa
      DB_PASSWORD: SeCret1234
      JWT_SECRETTKEY: -----BEGIN PRIVATE KEY-----\nMIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQC2do8EAkClQlkX\n9NqbF6KA64xvtCfyA+cAZlM/6StvxEs0J8KXJu0ygX31Wr0hLbBkS5i8SPPqGioE\nuTxXXx8Gv2FOduY6/EG5dJPbx5T10ry6k3bf+PDyOw9Bp14F8RpvrDPvGqWT9EIY\njTZsXGHLc4CQTc+rdg3NjXVou6qW5NzyGCINfhc0ftH3itcPdQNRWkuuQTNYe4ys\nJokSG+Xr1qVBlHS4hAkvWAQILzUXu4ofrlKzfx6kjJCC014Y6y8pHPuDGN9qYTMN\noPN1Uk5DX4u1fMTOERh9ylALpdO8ks4CH9oe1v8vZ9cYJh0itBRGL9a4PFKv4jWz\ntF1XhG3VAgMBAAECggEAAWM9RbQdrpQRWORgvZFhspYird53zPhOILDmh6573oBR\nQRmJ+jvOeICccbSptQ+jpIj1pahEC0mn0abgqFpzF4pLkLzzQHYW1Yn8g41M8Eyq\nE5zXqIQ8aDSI8msghNjh+6uag9eW9Efahn9xGe8JmFztw0swMzc1NipnVESpxgwp\nZrP2DU3KdpFHREU6aZXeT9xDfVXAEqkxvWhuVrDyTwxcnlNQwY3xcwbJapWj/Cg7\nJ8mDGpPhLu5qGAxXL4TOLBReri1mFoDOTTtViCCMMET+5j3cFYgiqrj9Ze6T5pge\nRyIm8ffyMOXS/9+w18Z1hzrnEgDPAOJEUwnrD7aU+QKBgQDscn4T+YNEQFOUVKal\nfmnYu+ZHRgL/MCf0CTLPoD3W56inPgjpZNg79/b1BXFH5/D1hYbEjvnEiurtmugR\nmO4NchvYGhzFr+DeGLATkJewNPbGgWzyYTFuv+RgonfzVI8t3snzxBXJ1ia/JLNc\n4F3eDauUWwE1bjYJVazZ2vNXeQKBgQDFjT2kei/C+cfEnAC7/8HQMkjSPDRoFFQ/\nFajoa/FR1U8p9g206WtW7TXFq8gI8NjCOE7nVtJu3uY8Ph0WCs0NZm3ZzxH28Bn2\nr9+/K3C1AbAe1nLew+f+OH0i20JsBj6AYL/3U+tfShgvQxdMwOBOm0EIrCJ/5wHM\nlxaoL6nGPQKBgQDaylr12sVni2qLcAVAUAhboAtG2nb9ca8WtshIrYtrZ7N9Bf8z\nELiyTRI8ygt3sR0b47HAAlkGUFFxCg1B81QcJwGy5v7Gwqd+fDO59usWBvxu1OZe\nJieaxn/qF4yNIirXFDellEVhHgN+jdRW1dqmFdo2DjvBGDlyS9AFSwAvaQKBgQCF\nfNb2WQoE+bsfAzsLzdos0I2cYcoXugTjS8OCqc26uiRv+i9w23kIl+kJ1PWp9PTC\n6EGI2IYBHOT+OAp3Zn0AXQJFd0JwVfV1V4odJ0FVTfqwG8Aq/r24bntAHmBXljCN\nltKgUThufyawaOlJl9r5wrbDIW1+d54jnMRWiT5zEQKBgF7t83NT6+Tg6rGlY+Nh\naKlxT8xs9PP+nyHPts2EndGnwrs3oWEvnsfWG+MVFTLQhGD/3aA9X+Q/ZGqVGYUF\nUkjSeQdlAFuT4EAmx9hJpmPpLbvtDg0E4ImxaWUVQod6w+LuOpAejhjPX3vBqm17\n5s2bJHmw7Psk3VTEoQlHctG0\n-----END PRIVATE KEY-----\n
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

