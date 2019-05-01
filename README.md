# SimilarTwitWeb

Server - http://18.197.92.28

## Endpoints 
the repository contains also a postman collection file in the root folder (SimilarTwitWeb.postman_collection.json).

**Follow -** 
[POST] {{host}}/api/followers

BODY :

{
	"FollowingUserId": 2,
	"FollowedUserId": 1
}

**CreateUser -**
[POST] {{host}}/api/users

BODY: 

"ran"

**PostMessage -**
[POST] {{host}}/api/message

BODY: 

{
	"UserId": 2,
	"MessageText": "testing"
}

**Unfollow -**
[DELETE] {{host}}/api/followers

BODY :

{
	"FollowingUserId": 2,
	"FollowedUserId": 1
}

**GetFeed -**
[GET] {{host}}/api/feed/{userId}?offset={optional}&size={optional}

**GetGlobalFeed -**
[GET] {{host}}/api/feed/global?offset={optional}&size={optional}


# Implementation Notes 

**Technologies:** .NET CORE 2, C#, SQLITE, ENTITY FRAMEWORK, DOCKER, AWS EC2, GITHUB

**DB:** i have decided to go with Entity framework and SQlite, entity framework provides high level abstractions of the choosen database which will allow me to switch from SQlite very quickly and easily, which is important since SQLite is nice for a POC but not the right tool for a production web app, SQlite is a very lightweight SQL DB and that is why i choose it for this task, for the sake of simplicity in set up and deployment.
I used Entity framework to create the db file and schema, and used it to setup the tables, primary keys and indexes, 
reference - DatabaseContext.cs file.

**Deploy:** i have deployed my code to an AWS EC2 instance, using this git repository and a docker container which is registered to DockerHub.

**Code:** 
I have split my code into 2 main projects.

SimilarTwitWeb.Api - 

which is the actual executable project, the web-server.
This project is responsible for http request handling, infrastructures, logging, configuration.
This project is not aware of how it handles the requests, that is the SimilarTwitWeb.Core project responsiblity, but this project is responsible for responsing the appropriate HTTP status codes, responses and error messages.
The controllers uses the dependency injection pattern to keep them isolated from the implementation of how to handle the requests. they are not aware of sqlite,entityframework, or any specific implementation.

SimilarTwitWeb.Core - 

This project is responsible to do the core logic of our app.
most of the work here is DB facing, you can find it under the DAL folder, each resource has its own Repository file which provides it data access to the resource.
Usually i add between the Controller layer and the DAL layer another Business Logic layer but since we have so little logic here besides DB calls i have decided to not create this layer here.
also i will usually not expose DB Objects in the api and instead use convertors from DB objects to API objects to prevent exposure of unnecessary data.

