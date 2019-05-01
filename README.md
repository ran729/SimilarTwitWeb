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
