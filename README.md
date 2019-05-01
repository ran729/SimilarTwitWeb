# SimilarTwitWeb

Server - TODO

## Endpoints 

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


# Architecture 
**Technologies:** .NET CORE 2, C#, SQLITE, ENTITY FRAMEWORK, DOCKER, AWS EC2, GITHUB
**DB:** i have decided to go with Entity framework and SQlite, entity framework provides high level abstractions of the choosen database which will allow me to switch from SQlite very quickly and easily, which is important since SQLite is nice for a POC but not the right tool for a production web app, SQlite is a very lightweight SQL DB and that is why i choose it for this task, for the sake of simplicity in set up and deployment.
I used Entity framework to create the db file and schema, and used it to setup the tables, primary keys and indexes, 
reference - DatabaseContext.cs file.
**Deploy:** i have deployed my code to an AWS EC2 instance, using this git repository and a docker container which is registered to DockerHub.
**Code:** 

