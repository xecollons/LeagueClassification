using LeagueClassification.Entities.Entities;
using LeagueClassification.Core.Helpers;
using LeagueClassification.Core.Interfaces;

namespace LeagueClassification.Core.Implementations
{
    //This is an inmemory implementation with some static class for persistence within the program execution, but could be connected to a database, for example
    //That is why the methods are async even if in this implementation, they are "fake" async
    public class MockLeagueRepository : ILeagueRepository
    {
        public MockLeagueRepository()
        {
        }

        public MockLeagueRepository(List<Team> teamEntries)
        {

            InMemorySourceContext.TeamEntries = new Dictionary<int, Team>();
            InMemorySourceContext.TeamClassificationEntries = new Dictionary<int, TeamClassificationStats>();
            InMemorySourceContext.Matches = new Dictionary<int, Match>();

            foreach (var team in teamEntries)
            {
                InMemorySourceContext.TeamEntries.Add(team.Id, team);
                InMemorySourceContext.TeamClassificationEntries.Add(team.Id, new TeamClassificationStats(team));
            }
        }

        public async void SaveTeams(List<Team> teams)
        {
            foreach (var team in teams)
            {
                if (team.Id == 0)
                {
                    team.Id = ++InMemorySourceContext.LastTeamId;
                }
                InMemorySourceContext.TeamEntries[team.Id] = team;
            }
        }

        public async void ClearMatches()
        {
            InMemorySourceContext.Matches.Clear();
        }

        public async Task<List<Match>> SaveMatches(List<Match> matches)
        {
            foreach (Match match in matches)
            {
                if (match.Id == 0)
                {
                    match.Id = ++InMemorySourceContext.LastMatchId;
                    InMemorySourceContext.TeamClassificationEntries[match.HomeTeam.Id].TeamMatches.Add(match);
                    InMemorySourceContext.TeamClassificationEntries[match.AwayTeam.Id].TeamMatches.Add(match);
                }
                InMemorySourceContext.Matches[match.Id] = match;
            }

            return matches;
        }

        public async Task<List<TeamClassificationStats>> GetLeagueClassification()
        {
            return InMemorySourceContext.TeamClassificationEntries.Values.ToList();
        }

        public async Task<TeamClassificationStats> GetTeamClassificationEntry(int id)
        {
            return InMemorySourceContext.TeamClassificationEntries[id];
        }

        public async Task<List<Match>> GetMatches()
        {
            return InMemorySourceContext.Matches.Values.ToList();
        }

        public async Task<List<Match>> GetMatchesForTeam(int teamId)
        {
            return InMemorySourceContext.Matches.Values.Where(m => m.HomeTeam.Id == teamId || m.AwayTeam.Id == teamId).ToList();
        }

        public async Task<List<Team>> GetTeams()
        {
            return InMemorySourceContext.TeamEntries.Values.ToList();
        }

        public async Task<Match?> GetMatch(int id)
        {
            try
            {
                return InMemorySourceContext.Matches[id];
            }
            catch (KeyNotFoundException) { return null; }
        }

        public async Task<List<Match>> GetMatchesForRound(int round)
        {
            return InMemorySourceContext.Matches.Values.Where(m => m.Round == round).ToList();
        }

        public async Task<Team?> GetTeam(int teamId)
        {
            try
            {
                return InMemorySourceContext.TeamEntries[teamId];
            }
            catch (KeyNotFoundException) { return null; }
        }

        public async Task<Match> SaveMatch(Match match)
        {
            if (match.Id == 0)
            {
                match.Id = ++InMemorySourceContext.LastMatchId;
                InMemorySourceContext.TeamClassificationEntries[match.HomeTeam.Id].TeamMatches.Add(match);
                InMemorySourceContext.TeamClassificationEntries[match.AwayTeam.Id].TeamMatches.Add(match);
            }
            InMemorySourceContext.Matches[match.Id] = match;
            return match;
        }
    }
}
