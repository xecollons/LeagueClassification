using LeagueClassification.Core.Interfaces;
using LeagueClassification.Entities.Entities;
using LeagueClassification.SimEngine.Implementations;
using LeagueClassification.SimEngine.Interfaces;

namespace LeagueClassification.Tests.MockImplementations
{
    public class StaticStrengthMatchSimulatorEngine : IMatchSimulatorEngine
    {
        public void SimulateMatch(Match match)
        {
            //Simple random match simulation criteria
            match.GoalsForHomeTeam = match.HomeTeam.Strength;
            match.GoalsForAwayTeam = match.AwayTeam.Strength;
            match.Played = true;
        }
    }

}