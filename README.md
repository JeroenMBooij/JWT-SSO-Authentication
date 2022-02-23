# Authentication Server

This repository contains the source code for an API where developers can manage their authentication needs. A Developer can use this API to design multiple JWT's to fit needs and assign it to their applications. Furthermore, with the refresh endpoint you can switch between JWT designs incase your application communicates with multiple services which require different JWT's. This scheme allows a user to log in with a single ID and password to any of several related, yet independent, software systems.
An instance of this application is running on: <a href="https://auth.aapie.xyz">https://auth.aapie.xyz/index.html</a> 

The application software architecture is designed using a clean code approach with Dependency Inversion Principle and Domain-Driven Design


<img src="https://i.imgur.com/Toqkgg9.png" width="1000" style="margin-left: auto; margin-right:auto;"/>

<p><b>Web layer | API layer</b> contains contains all controllers for endpoints and is responsible for handling all the requests made to the API</p> <br>

<p><b>Domain layer</b> contains contains all the definitions for the database entities</p> <br>

<p><b>Persistance layer</b> contains the data access definition. By using a repository pattern the data can be manipulated by the logic layer</p> <br>

<p><b>Service layer</b> contains all external services like the email service</p><br>

<p><b>Common layer</b> contains all request en response models, as well as all the interfaces so every layer knows what the application can do without having a dependency on any of the higher level layers. This way the Logic layer can use the functionality from the Persistance or Service layers without having a tight coupling between the layers</p><br>

