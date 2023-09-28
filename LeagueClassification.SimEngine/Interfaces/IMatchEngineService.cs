using LeagueClassification.Entities.Entities;

namespace LeagueClassification.SimEngine.Interfaces
{
    public interface IMatchEngineService
    {
        Task<List<Match>> SimulateAll();
        Task<List<Match>> GenerateMatchSchedule();
        Task<List<Match>> GetMatches();
        Task<List<Match>> GetMatchesPerRound(int round);
        Task<List<Match>> GetMatchesPerTeam(int teamId);
        Task<List<Match>> SimulateRound(int round);
        Task<List<Match>> SaveMatchSchedule(List<Match> matches);
        Task<Match?> GetMatch(int id);
    }
}
