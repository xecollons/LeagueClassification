using LeagueClassification.Entities.Entities;

namespace LeagueClassification.SimEngine.Interfaces
{
    //If we can inject the simulatorengine itself, we can have multiple simulations to choose
    //In this case, we will use in the tests a simulator engine that only takes in account the strengths of the team for the simulation
    //And a one with certain randomness to the rest api
    public interface IMatchSimulatorEngine
    {
        void SimulateMatch(Match match);
    }
}
