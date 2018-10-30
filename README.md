# RESTful Web APIs
[![Build status](https://abdelkrimdev.visualstudio.com/RESTful%20Web%20APIs/_apis/build/status/RESTful%20Web%20APIs%20Main%20Pipeline)](https://abdelkrimdev.visualstudio.com/RESTful%20Web%20APIs/_build/latest?definitionId=4)
[![GitHub](https://img.shields.io/github/license/mashape/apistatus.svg?style=popout)](https://github.com/abdelkrimdev/restful-web-apis/blob/master/LICENSE)

Boilerplate code for creating your next enterprise scale microservices-based application written in `.Net Core` Web APIs and `MongoDB` NoSQL database, with `Docker` containers support. Using best practices and industry standards.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See deployment for notes on how to deploy the project on a live system.

```
docker-compose up
```

## Running the Tests

### Unit Tests

```
dotnet test --logger trx
```

### Integration Tests

```
docker-compose -f docker-compose.yml -f docker-compose.test.yml up --abort-on-container-exit
```

## Deployment

Set the following environment variables on your production docker host

```
DOCKER_REGISTRY=your_docker_hub_username

MONGO_ROOT_USERNAME=root_username
MONGO_ROOT_PASSWORD=very_secure_password

TODO_MONGO_HOST=tododb
TODO_MONGO_PORT=27017
TODO_MONGO_DB=TodoDatabase
TODO_MONGO_USER=TodoAPI
TODO_MONGO_PASS=Secret
```

Once you’ve got everything setup and ready to go run this command

```
docker-compose -f docker-compose.yml -f docker-compose.prod.yml up -d
```

## Built With

* [.NET Core](https://docs.microsoft.com/en-us/dotnet/core/) - The software framework used
* [MongoDB](https://docs.mongodb.com/) - NoSQL document-oriented database
* [NUnit](https://github.com/nunit/docs/wiki/) - Unit testing framework for Microsoft .NET

## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/abdelkrimdev/restful-web-apis/tags). 

## Authors

* **Abdelkrim Dib** - *Initial work* - [abdelkrimdev](https://github.com/abdelkrimdev)

See also the list of [contributors](https://github.com/abdelkrimdev/restful-web-apis/graphs/contributors) who participated in this project.

## License

This project is licensed under the MIT License - see the [LICENSE.md](https://github.com/abdelkrimdev/restful-web-apis/blob/master/LICENSE) file for details
