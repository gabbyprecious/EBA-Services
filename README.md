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
