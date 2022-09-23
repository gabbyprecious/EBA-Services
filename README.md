# Event Based System 
This project is powered by Docker. 
It has 5 Containers:
2 MicroServices:
- API: User Service
- WriterService: Create txt file on Creation
And...
- MongoDb: Hosting DB
- MongoExpress: Interacting with DB directly
- RabbitMQ: Handling events 

## Setup 
The only setup required to run project is to have docker installed. 

## Running 

There are two environments

Development: 
- `docker-compose -f docker-compose.dev.yml up --build`

Production 
- `docker-compose up --build`  

After all containers are running:
The Server will be running on `http://localhost:8083/api/``

Endpoints:
- Registration: `POST` `http://localhost:8083/api/User/`
- Login: `POST` `http://localhost:8083/api/User/login` 
    body: `{"username": "username", "password": "password"}``
- GET USER: `GET` `http://localhost:8083/api/User/{id}`
- UPDATE USER: `PUT` `http://localhost:8083/api/User/{id}`
- DELETE USER: `DELETE` `http://localhost:8083/api/User/{id}`

- GET USERs: `GET` `http://localhost:8083/api/User/`
query params: 
- Filters
 - - lastName
 - - firstName
 - - atleastAConnection -options: yes or no

 - Ordering
 - - order: CreationDate, LastConnectionDate, LastName
 - - orderby: options: desc, asc



