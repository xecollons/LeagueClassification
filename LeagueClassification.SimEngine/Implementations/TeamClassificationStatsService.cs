using LeagueClassification.Core.Interfaces;
using LeagueClassification.Entities.Entities;
using LeagueClassification.SimEngine.Helpers;
using LeagueClassification.SimEngine.Interfaces;

namespace LeagueClassification.SimEngine.Implementations
{
    public class TeamClassificationStatsService : ITeamClassificationStatsService
    {
        ILeagueRepository _leagueRepository;
        public TeamClassificationStatsService(ILeagueRepository leagueRepository)
        {
            _leagueRepository = leagueRepository;
        }

        public async Task<List<TeamClassificationStats>> GetClassificationEntries()
        {
            var teamClassificationStats = await _leagueRepository.GetLeagueClassification();
            return teamClassificationStats.OrderBy(t => t).ToList();
        }

        public async Task UpdateClassificationEntries()
        {
            var matches = await _leagueRepository.GetMatches();
            foreach (var match in matches)
            {
                await UpdateClassificationEntries(match);
            }
        }

        async Task UpdateClassificationEntries(Match match)
        {
            if (!match.Played) return;

            var homeTeam = await _leagueRepository.GetTeamClassificationEntry(match.HomeTeam.Id);
            var awayTeam = await _leagueRepository.GetTeamClassificationEntry(match.AwayTeam.Id);

            LeagueClassificationUtils.UpdateClassificationEntries(match, homeTeam, awayTeam);
        }
    }
}
