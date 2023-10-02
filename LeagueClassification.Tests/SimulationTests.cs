using LeagueClassification.Core.Interfaces;
using LeagueClassification.Core.Implementations;
using LeagueClassification.Entities.Entities;
using LeagueClassification.SimEngine.Implementations;
using LeagueClassification.SimEngine.Interfaces;
using LeagueClassification.Tests.MockImplementations;
using LeagueClassification.SimEngine.Exceptions;

namespace LeagueClassification.Tests
{
    public class SimulationTests
    {
        IMatchEngineService matchEngineService;
        ITeamClassificationStatsService teamClassificationStatsService;
        ILeagueRepository leagueRepository;
        IMatchSimulatorEngine matchSimulatorEngine;

        Dictionary<int, Team> teamEntries = new Dictionary<int, Team>()
            {
                {1, new Team() {Id = 1, Name = "Valencia", Strength = 4}},
                {2, new Team() {Id = 2, Name = "Atletico", Strength = 2}},
                {3, new Team() {Id = 3, Name = "Real Madrid", Strength = 3}},
                {4, new Team() {Id = 4, Name = "Barcelona", Strength = 3 }}
            };

        List<Match> mockMatches;

        [OneTimeSetUp] 
        public void OneTimeSetUp()
        {
            mockMatches = new List<Match>()
            {
                new Match() {Id = 1, HomeTeam = teamEntries[1], AwayTeam = teamEntries[2], GoalsForHomeTeam = 2, GoalsForAwayTeam = 1, Round = 1, Played = true}, //Valencia 2 - 1 Atletico
                new Match() {Id = 2, HomeTeam = teamEntries[3], AwayTeam = teamEntries[4], GoalsForHomeTeam = 2, GoalsForAwayTeam = 2, Round = 1, Played = true}, //Real Madrid 2 - 2 Barcelona
                new Match() {Id = 3, HomeTeam = teamEntries[1], AwayTeam = teamEntries[3], GoalsForHomeTeam = 1, GoalsForAwayTeam = 2, Round = 2, Played = true}, //Valencia 1 - Real Madrid 2
                new Match() {Id = 4, HomeTeam = teamEntries[4], AwayTeam = teamEntries[2], GoalsForHomeTeam = 1, GoalsForAwayTeam = 2, Round = 2, Played = true}, //Barcelona 1 - 2 Atletico
                new Match() {Id = 5, HomeTeam = teamEntries[1], AwayTeam = teamEntries[4], GoalsForHomeTeam = 1, GoalsForAwayTeam = 1, Round = 3, Played = true}, //Valencia 1 - 1 Barcelona
                new Match() {Id = 6, HomeTeam = teamEntries[3], AwayTeam = teamEntries[2], GoalsForHomeTeam = 1, GoalsForAwayTeam = 1, Round = 3, Played = true}, //Real Madrid 1 - 1 Atletico
            };
        }

        [SetUp]
        public void Setup()
        {
            leagueRepository = new MockLeagueRepository(teamEntries.Values.ToList());
            matchSimulatorEngine = new StaticStrengthMatchSimulatorEngine();
            matchEngineService = new MatchEngineService(leagueRepository, matchSimulatorEngine);
            teamClassificationStatsService = new TeamClassificationStatsService(leagueRepository);
        }

        [Test]
        public async Task Assert_CreatingMatchSchedule_CreatesCorrectSchedule()
        {
            HashSet<string> matchesChecked = new HashSet<string>();
            var matches = await matchEngineService.GenerateMatchSchedule();

            foreach (var match in matches)
            {
                //We create a matchkey to check that one combination of teams for a match exists only once between the different rounds
                //We sort the Ids so the matchkeys are unique since we don't have a home/away schedule, and Barça - Madrid would have the same key as Madrid - Barça, hence, repeated, and the test would fail.
                //In total we should have 6 matches (or different combinations) for 4 teams
                int[] teamNames = new int[] { match.HomeTeam.Id, match.AwayTeam.Id };
                Array.Sort(teamNames);
                var matchKey = string.Join("|", teamNames);
                matchesChecked.Add(matchKey);
            }

            Assert.That(matchesChecked.Count, Is.EqualTo(matches.Count));
        }

        [Test]
        public void Assert_CreatingMatchScheduleWithoutExistingTeams_ThrowsNoTeamsException()
        {
            leagueRepository = new MockLeagueRepository(new List<Team>());
            matchEngineService = new MatchEngineService(leagueRepository, matchSimulatorEngine);
            HashSet<string> matchesChecked = new HashSet<string>();
            Assert.ThrowsAsync<NoExistingTeamsException>(async () => await matchEngineService.GenerateMatchSchedule());
        }

        [Test]
        public async Task Assert_Simulating_Match_Simulates_Match()
        {
            var matches = await matchEngineService.GenerateMatchSchedule();
            var match = await matchEngineService.SimulateMatch(matches.First().Id);

            Assert.IsTrue(match.Played);
        }

        [Test]
        public async Task Assert_Simulating_NonExistingMatch_Returns_Null()
        {
            var match = await matchEngineService.SimulateMatch(222);

            Assert.IsNull(match);
        }

        [Test]
        public async Task Assert_SimulatingAll_SimulatesAllMatches()
        {
            await matchEngineService.GenerateMatchSchedule();
            var matches = await matchEngineService.SimulateAll();

            Assert.IsFalse(matches.Any(m => !m.Played));
        }

        [Test]
        public async Task Assert_UpdatingTeamClassificationStats_WorksOKForWinningTeam()
        {
            await matchEngineService.SaveMatchSchedule(mockMatches.Take(2).ToList());
            await teamClassificationStatsService.UpdateClassificationEntries();
            var classificationEntries = await teamClassificationStatsService.GetClassificationEntries();
            Assert.Multiple(() =>
            {
                Assert.That(classificationEntries[0].Points, Is.EqualTo(3));
                Assert.That(classificationEntries[0].GoalsFor, Is.EqualTo(2));
                Assert.That(classificationEntries[0].GoalsAgainst, Is.EqualTo(1));
                Assert.That(classificationEntries[0].MatchesWon, Is.EqualTo(1));
                Assert.That(classificationEntries[0].MatchesDrawn, Is.EqualTo(0));
                Assert.That(classificationEntries[0].MatchesLost, Is.EqualTo(0));
            });
        }

        [Test]
        public async Task Assert_UpdatingTeamClassificationStats_WorksOKForLosingTeam()
        {
            await matchEngineService.SaveMatchSchedule(mockMatches.Take(2).ToList());
            await teamClassificationStatsService.UpdateClassificationEntries();
            var classificationEntries = await teamClassificationStatsService.GetClassificationEntries();
            Assert.Multiple(() =>
            {
                Assert.That(classificationEntries[3].Points, Is.EqualTo(0));
                Assert.That(classificationEntries[3].GoalsFor, Is.EqualTo(1));
                Assert.That(classificationEntries[3].GoalsAgainst, Is.EqualTo(2));
                Assert.That(classificationEntries[3].MatchesWon, Is.EqualTo(0));
                Assert.That(classificationEntries[3].MatchesDrawn, Is.EqualTo(0));
                Assert.That(classificationEntries[3].MatchesLost, Is.EqualTo(1));
            });
        }

        [Test]
        public async Task Assert_UpdatingTeamClassificationStats_WorksOKForDrawingTeams()
        {
            await matchEngineService.SaveMatchSchedule(mockMatches.Take(2).ToList());
            await teamClassificationStatsService.UpdateClassificationEntries();
            var classificationEntries = await teamClassificationStatsService.GetClassificationEntries();
            Assert.Multiple(() =>
            {
                Assert.That(classificationEntries[1].Points, Is.EqualTo(1));
                Assert.That(classificationEntries[1].GoalsFor, Is.EqualTo(2));
                Assert.That(classificationEntries[1].GoalsAgainst, Is.EqualTo(2));
                Assert.That(classificationEntries[1].MatchesWon, Is.EqualTo(0));
                Assert.That(classificationEntries[1].MatchesDrawn, Is.EqualTo(1));
                Assert.That(classificationEntries[1].MatchesLost, Is.EqualTo(0));
            });
            Assert.Multiple(() =>
            {
                Assert.That(classificationEntries[2].Points, Is.EqualTo(1));
                Assert.That(classificationEntries[2].GoalsFor, Is.EqualTo(2));
                Assert.That(classificationEntries[2].GoalsAgainst, Is.EqualTo(2));
                Assert.That(classificationEntries[2].MatchesWon, Is.EqualTo(0));
                Assert.That(classificationEntries[2].MatchesDrawn, Is.EqualTo(1));
                Assert.That(classificationEntries[2].MatchesLost, Is.EqualTo(0));
            });
        }



        [Test]
        public async Task Assert_ClassificationOrderIsOKAfterSimulatingMatches()
        {
            await matchEngineService.SaveMatchSchedule(mockMatches);
            await teamClassificationStatsService.UpdateClassificationEntries();
            var classificationEntries = await teamClassificationStatsService.GetClassificationEntries();
            Assert.That(classificationEntries.Count, Is.EqualTo(4));
            Assert.Multiple(() =>
            {
                Assert.That(classificationEntries[0].Team.Id, Is.EqualTo(3)); //Real Madrid with points
                Assert.That(classificationEntries[1].Team.Id, Is.EqualTo(1)); //Valencia above Atletico because of head to head
                Assert.That(classificationEntries[2].Team.Id, Is.EqualTo(2)); //Atletico
                Assert.That(classificationEntries[3].Team.Id, Is.EqualTo(4)); //Barcelona with less points
            });
        }
    }
}