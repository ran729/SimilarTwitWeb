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

"ran" // USERNAME

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
[GET] {{host}}/api/feed/{userId}

**GetGlobalFeed -**
[GET] {{host}}/api/feed/global


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
also i will usually not expose DB Objects in the api and instead use convertors from DB objects to API objects to prevent exposure of unnecessary data.

**Feed Architecture:** 

The general idea - 

A new message is posted to the api, the api saves it to the db, then an offline processor processes the new message and adds it to a data structure of feeds of all the posting user's followers, this data is saved in some shared-memory db cluster like Redis.
When Celebrities post a message ( celebrities are defined as users with more than 5K followers), their messages are saved on the shared memory too but not indexed on all of the followers feeds, since there are too many we dont want to update all the followers feeds.
when a user requests his feed, we query his feed from the Redis queue, and then add to it the latest messages of the celebrities he is following.
in-memory Feeds are saved only to users who are active in the last 15 days.

Actual implementation -

instead of Redis i did this in-memory.
instead of offline processing of the feed i have processed the feed upon receiving a new message from the api.
didnt implement a system to remove old feeds of inactive users.



