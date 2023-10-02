using LeagueClassification.Entities.Entities;
using LeagueClassification.Core.Interfaces;
using LeagueClassification.SimEngine.Helpers;
using LeagueClassification.SimEngine.Interfaces;
using LeagueClassification.SimEngine.Exceptions;

namespace LeagueClassification.SimEngine.Implementations
{
    public class MatchEngineService : IMatchEngineService
    {
        private ILeagueRepository _leagueRepository;
        private IMatchSimulatorEngine _matchSimulator;
        public MatchEngineService(ILeagueRepository leagueRepository, IMatchSimulatorEngine matchSimulator)
        {
            _leagueRepository = leagueRepository;
            _matchSimulator = matchSimulator;
        }

        public async Task<List<Match>> GenerateMatchSchedule()
        {
            var teams = await _leagueRepository.GetTeams();
            if (teams == null || teams.Count == 0)
            {
                throw new NoExistingTeamsException("There are no teams created. Please create some teams before proceeding with this step.");
            }
            _leagueRepository.ClearMatches();
            var matches = LeagueClassificationUtils.GenerateRoundRobinSchedule(teams);
            await SaveMatchSchedule(matches);

            return matches;
        }

        public async Task<List<Match>> SaveMatchSchedule(List<Match> matches)
        {
            return await _leagueRepository.SaveMatches(matches);
        }

        public async Task<List<Match>> SimulateAll()
        {
            var matches = await _leagueRepository.GetMatches();
            foreach (var match in matches)
            {
                SimulateMatch(match);
            }

            return matches;
        }

        protected virtual void SimulateMatch(Match match)
        {
            if (match.Played) return;

            _matchSimulator.SimulateMatch(match);

            UpdateClassificationEntries(match);
        }

        protected async void UpdateClassificationEntries(Match match)
        {
            if (!match.Played) return;

            var homeTeam = await _leagueRepository.GetTeamClassificationEntry(match.HomeTeam.Id);
            var awayTeam = await _leagueRepository.GetTeamClassificationEntry(match.AwayTeam.Id);

            LeagueClassificationUtils.UpdateClassificationEntries(match, homeTeam, awayTeam);
        }

        public async Task<List<Match>> SimulateRound(int round)
        {
            var matches = await _leagueRepository.GetMatchesForRound(round);
            foreach (var match in matches)
            {
                SimulateMatch(match);
            }
            return matches;
        }

        public async Task<List<Match>> GetMatches()
        {
            return await _leagueRepository.GetMatches();
        }

        public async Task<List<Match>> GetMatchesPerRound(int round)
        {
            return await _leagueRepository.GetMatchesForRound(round);
        }

        public async Task<List<Match>> GetMatchesPerTeam(int teamId)
        {
            return await _leagueRepository.GetMatchesForTeam(teamId);
        }

        public async Task<Match?> GetMatch(int id)
        {
            return await _leagueRepository.GetMatch(id);
        }

        public async Task<Match?> SimulateMatch(int matchId)
        {
            var match = await _leagueRepository.GetMatch(matchId);
            if (match == null) return null;

            SimulateMatch(match);
            match = await _leagueRepository.SaveMatch(match);
            return match;
        }
    }
}