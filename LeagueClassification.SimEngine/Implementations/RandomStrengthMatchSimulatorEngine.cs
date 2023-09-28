using LeagueClassification.Entities.Entities;
using LeagueClassification.SimEngine.Interfaces;

namespace LeagueClassification.SimEngine.Implementations
{
    public class RandomStrengthMatchSimulatorEngine : IMatchSimulatorEngine
    {
        public void SimulateMatch(Match match)
        {
            Random random = new Random();
            match.GoalsForHomeTeam = random.Next(0, match.HomeTeam.Strength);
            match.GoalsForAwayTeam = random.Next(0, match.AwayTeam.Strength);
            match.Played = true;
        }
    }
}
