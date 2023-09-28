using LeagueClassification.Entities.Entities;

namespace LeagueClassification.TeamsServices.Interfaces
{
    public interface ITeamsService
    {
        Task<List<Team>> GetTeams();
        Task<Team?> GetTeam(int id);
    }
}
