using LeagueClassification.Core.Implementations;
using LeagueClassification.Core.Interfaces;
using LeagueClassification.Entities.Entities;
using LeagueClassification.TeamsServices.Implementations;
using LeagueClassification.TeamsServices.Interfaces;

namespace LeagueClassification.Tests
{
    public class TeamTests
    {
        ITeamsService teamsService;
        ILeagueRepository leagueRepository;
        Dictionary<int, Team> teamEntries = new Dictionary<int, Team>()
            {
                {1, new Team() {Id = 1, Name = "Valencia", Strength = 4}},
                {2, new Team() {Id = 2, Name = "Atletico", Strength = 2}},
                {3, new Team() {Id = 3, Name = "Real Madrid", Strength = 3}},
                {4, new Team() {Id = 4, Name = "Barcelona", Strength = 3 }}
            };

        [SetUp]
        public void Setup()
        {
            leagueRepository = new MockLeagueRepository(teamEntries.Values.ToList());
            teamsService = new TeamsService(leagueRepository);
        }

        [Test]
        public async Task Assert_ReturningAnExistingTeam_ReturnsTeam()
        {
            var team = await teamsService.GetTeam(1);
            Assert.IsNotNull(team);
        }

        [Test]
        public async Task Assert_ReturningNonExistingTeam_ReturnsNull()
        {
            var team = await teamsService.GetTeam(5);
            Assert.IsNull(team);
        }

        [Test]
        public async Task Assert_GetTeams_ReturnExistingTeams()
        {
            var teams = await teamsService.GetTeams();
            Assert.IsNotNull(teams);
            Assert.That(teams.Count, Is.EqualTo(4));
        }

        [Test]
        public async Task Assert_WithNoTeams_ReturnsEmptyList()
        {
            leagueRepository = new MockLeagueRepository(new List<Team>());
            teamsService = new TeamsService(leagueRepository);
            var teams = await teamsService.GetTeams();
            Assert.IsNotNull(teams);
            Assert.That(teams.Count, Is.EqualTo(0));
        }
    }
}
