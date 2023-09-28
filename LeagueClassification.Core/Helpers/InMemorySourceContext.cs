using LeagueClassification.Entities.Entities;

namespace LeagueClassification.Core.Helpers
{
    public static class InMemorySourceContext
    {
        public static Dictionary<int, Match> Matches { get; set; } = new Dictionary<int, Match>();
        public static Dictionary<int, Team> TeamEntries { get; set; }

        public static Dictionary<int, TeamClassificationStats> TeamClassificationEntries { get; set; }

        public static int LastMatchId { get; set; } = 0;
        public static int LastTeamId { get; set; } = 0;

        static InMemorySourceContext()
        {
            TeamEntries = new Dictionary<int, Team>()
            {
                {1, new Team() {Id = 1, Name = "Valencia", Strength = 4}},
                {2, new Team() {Id = 2, Name = "Atletico", Strength = 2}},
                {3, new Team() {Id = 3, Name = "Real Madrid", Strength = 3}},
                {4, new Team() {Id = 4, Name = "Barcelona", Strength = 3 }}
            };

            TeamClassificationEntries = new Dictionary<int, TeamClassificationStats>();
            foreach (var team in TeamEntries.Values)
            {
                TeamClassificationEntries.Add(team.Id, new TeamClassificationStats(team));
            }
        }
    }
}
