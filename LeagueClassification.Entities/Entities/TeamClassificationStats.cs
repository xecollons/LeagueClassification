using LeagueClassification.Entities.Helpers;
using System.Collections;
using System.Text.RegularExpressions;

namespace LeagueClassification.Entities.Entities
{
    //I doubted about putting all the stats in the Team entity itself,
    //but I think that conceptually is another thing even if the relation is 1:1
    //Team is... well, a team, and the team stats for a season are a different thing.
    public class TeamClassificationStats : IComparable<TeamClassificationStats>
    {
        public Team Team { get; set; }
        public int MatchesWon { get; set; }
        public int MatchesLost { get; set; }
        public int MatchesDrawn { get; set; }
        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }
        public int Points { get; set; }
        public int GoalDifference => GoalsFor - GoalsAgainst;
        public List<Match> TeamMatches { get; set; } = new List<Match>();


        public TeamClassificationStats(Team team)
        {
            Team = team;
        }

        public int CompareTo(TeamClassificationStats? other)
        {
            if (other == null) return -1;
            return TeamClassificationStatsComparer.Compare(this, other);
        }
    }
}