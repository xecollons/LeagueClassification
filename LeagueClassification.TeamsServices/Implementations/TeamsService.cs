using LeagueClassification.Entities.Entities;
using LeagueClassification.Core.Interfaces;
using LeagueClassification.TeamsServices.Interfaces;

namespace LeagueClassification.TeamsServices.Implementations
{
    public class TeamsService : ITeamsService
    {
        ILeagueRepository _leagueRepository;
        public TeamsService(ILeagueRepository leagueRepository)
        {
            _leagueRepository = leagueRepository;
        }
        public async Task<Team?> GetTeam(int id)
        {
            return await _leagueRepository.GetTeam(id);
        }

        public async Task<List<Team>> GetTeams()
        {
            return await _leagueRepository.GetTeams();
        }
    }
}
