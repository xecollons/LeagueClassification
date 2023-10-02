using LeagueClassification.Entities.Entities;

namespace LeagueClassification.Core.Interfaces
{
    public interface ILeagueRepository
    {
        Task<Match?> GetMatch(int id);
        Task<List<Match>> GetMatches();
        Task<List<Match>> GetMatchesForTeam(int teamId);
        Task<List<TeamClassificationStats>> GetLeagueClassification();
        Task<List<Match>> SaveMatches(List<Match> matches);
        Task<Match> SaveMatch(Match match);
        Task<List<Team>> GetTeams();
        Task<Team?> GetTeam(int teamId);
        Task<TeamClassificationStats> GetTeamClassificationEntry(int id);
        Task<List<Match>> GetMatchesForRound(int round);
        void ClearMatches();
    }
}
