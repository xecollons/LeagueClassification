# LeagueClassification

Here is an implementation of the assignment given in [https://github.com/gamebasics/assessment/wiki/Development-Assessment-Group-Phase](https://github.com/gamebasics/assessment/wiki/Development-Assessment-Group-Phase).

Since I don't have that much expertise in the UI/frontend, I have focused myself in the backend part, trying to have a scalable and testable code.

## Project composition
Since it's a bit of a test, I wanted it to be as modular as possible. Not being a big project, it could have been done with less "style", less overengineered, but here we go:
- We have two microservices, **LeagueClassification.MatchEngine**, and **LeagueClassification.Teams**. The second one is to manage teams, although right now it only *gets* teams, it doesn't store anything since it's out of the scope of the test.
The first one manages the creation of the match schedule for the different teams, the match simulation and getting the classification. Ideally, I would have put the classification related methods in another microservice, but for testing purposes, I have some sort of inmemory persistence that is no more than a static class, and I didn't want to put a lot of effort to it.
- Both microservices have different services injected to it. Again, I wanted it to be modular, and we are injecting a MockRepository, for instance, but we could we injecting a repository that went to the database.
- Apart from the webservices themselves, there is the **LeagueClassification.Entities** project for the entities themselves, the **LeagueClassification.Core** that would have some shared classes (right now, only the repository), the **LeagueClassification.SimEngine** and **LeagueClassification.Teams** as some service provider libraries, and the **LeagueClassification.Tests** for unit tests

## Testing
One way of testing the project would be following the different path:
- Select **LeagueClassification.MatchEngine** and **LeagueClassification.Teams** as startup projects.
- Build and run the solution. The Swagger UI should appear.
- In the MatchEngine, follow the following sequence:
	- Execute the /MatchEngine/GenerateMatchSchedule post request. This will generate 6 different matches for the 4 default teams, 3 rounds.
	- Here it's possible to simulate all matches directly using the /MatchEngine/SimulateMatches endpoint, or it's possible to simulate a particular round using /MatchEngine/SimulateMatches/round/{round}, where round is an integer.
	- With these steps, we will have all matches simulated. After this, the endpoint for getting the classification table is /MatchEngine/GetClassification.

With that, it's possible to do a minimal or starting test of the program. Another way of testing it would be executing the **unit tests** from the **LeagueClassification.Tests** project.

Some things missing due to lack of time may be a couple of more unit tests, like checking the different comparisons between the teamclassificationstats (There is only one superficial test for that), adding a TeamClassificationStats microservice, refactoring the leagueRepository to separate it in a couple of classes, or more error checking (I have created (and thrown) a exception -NoExistingTeamsException- as a example). 