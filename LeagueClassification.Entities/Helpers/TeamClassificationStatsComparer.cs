using LeagueClassification.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueClassification.Entities.Helpers
{
    public static class TeamClassificationStatsComparer
    {
        public static int Compare(TeamClassificationStats t1, TeamClassificationStats other)
        {
            //TODO: Add this dynamically
            Func<TeamClassificationStats, TeamClassificationStats, int>[] comparisonRules = new Func<TeamClassificationStats, TeamClassificationStats, int>[]
        {
            (team1, team2) => ComparePoints(team1, team2),
            (team1, team2) => CompareGoalDifference(team1, team2),
            (team1, team2) => CompareGoalsFor(team1, team2),
            (team1, team2) => CompareGoalsAgainst(team1, team2),
            (team1, team2) => CompareHeadToHead(team1, team2),
            //should we add an alphabetical comparison as the last one?
        };

            int result = comparisonRules.Select(method => method(t1, other))
                                //Added the ToList here since without it it works but it can't be debugged, related to https://github.com/dotnet/runtime/issues/62232
                                .ToList() 
                                .FirstOrDefault(result => result != 0);


            return result;
        }

        static int ComparePoints(TeamClassificationStats t1, TeamClassificationStats other)
        {
            if (t1.Points > other.Points)
            {
                return -1;
            }
            else if (t1.Points < other.Points)
            {
                return 1;
            }
            return 0;
        }

        static int CompareGoalDifference(TeamClassificationStats t1, TeamClassificationStats other)
        {
            if (t1.GoalDifference > other.GoalDifference)
            {
                return -1;
            }
            else if (t1.GoalDifference < other.GoalDifference)
            {
                return 1;
            }

            return 0;
        }

        static int CompareGoalsFor(TeamClassificationStats t1, TeamClassificationStats other)
        {
            if (t1.GoalsFor > other.GoalsFor)
            {
                return -1;
            }
            else if (t1.GoalsFor < other.GoalsFor)
            {
                return 1;
            }

            return 0;
        }

        static int CompareGoalsAgainst(TeamClassificationStats t1, TeamClassificationStats other)
        {
            if (t1.GoalsAgainst < other.GoalsAgainst)
            {
                return -1;
            }
            else if (t1.GoalsAgainst > other.GoalsAgainst)
            {
                return 1;
            }

            return 0;
        }

        static int CompareHeadToHead(TeamClassificationStats t1, TeamClassificationStats other)
        {
            int res = 0;

            foreach (Match? match in t1.TeamMatches.Where(m => m.HomeTeam.Id == other.Team.Id || m.AwayTeam.Id == other.Team.Id))
            {
                //t1 is home team
                if (match.HomeTeam.Id == t1.Team.Id)
                {
                    if (match.GoalsForHomeTeam > match.GoalsForAwayTeam)
                    {
                        res--;
                    }
                    else if (match.GoalsForHomeTeam < match.GoalsForAwayTeam)
                    {
                        res++;
                    }
                }
                else
                {
                    if (match.GoalsForHomeTeam > match.GoalsForAwayTeam)
                    {
                        res++;
                    }
                    else if (match.GoalsForHomeTeam < match.GoalsForAwayTeam)
                    {
                        res--;
                    }
                }
            }

            return res > 0 ? 1 : res < 0 ? -1 : 0;
        }
    }
}
