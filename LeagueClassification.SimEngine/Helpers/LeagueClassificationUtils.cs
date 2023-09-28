using LeagueClassification.Entities.Entities;
using LeagueClassification.SimEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueClassification.SimEngine.Helpers
{
    public static class LeagueClassificationUtils
    {
        //Instead of a helper class with the round robin schedule generation, this could had been an interface to generate a general schedule
        //and have a "RoundRobinScheduler" class injected that implements the round robin one, or some other scheduling algorithm
        //but I wanted to leave this this way for simplicity 
        public static List<Match> GenerateRoundRobinSchedule(List<Team> teams)
        {
            List<Match> matches = new List<Match>();
            for (int round = 1; round <= 3; round++)
            {
                for (int i = 0; i < teams.Count / 2; i++)
                {

                    Match match = new Match();
                    match.HomeTeam = teams[i];
                    match.AwayTeam = teams[teams.Count - i - 1];
                    match.Round = round;
                    matches.Add(match);
                }
                // We rotate the teams for the next round
                Team lastTeam = teams[teams.Count - 1];
                for (int i = teams.Count - 1; i > 1; i--)
                {
                    teams[i] = teams[i - 1];
                }
                teams[1] = lastTeam;
            }

            return matches;
        }

        public static void UpdateClassificationEntries(Match match, TeamClassificationStats homeTeam, TeamClassificationStats awayTeam)
        {
            if (!match.Played) return;

            homeTeam.GoalsFor += match.GoalsForHomeTeam;
            homeTeam.GoalsAgainst += match.GoalsForAwayTeam;
            awayTeam.GoalsFor += match.GoalsForAwayTeam;
            awayTeam.GoalsAgainst += match.GoalsForHomeTeam;

            if (match.GoalsForHomeTeam > match.GoalsForAwayTeam)
            {
                homeTeam.MatchesWon++;
                awayTeam.MatchesLost++;
                homeTeam.Points += 3;
            }
            else if (match.GoalsForHomeTeam < match.GoalsForAwayTeam)
            {
                homeTeam.MatchesLost++;
                awayTeam.MatchesWon++;

                awayTeam.Points += 3;
            }
            else
            {
                homeTeam.MatchesDrawn++;
                awayTeam.MatchesDrawn++;

                homeTeam.Points += 1;
                awayTeam.Points += 1;
            }

        }


    }
}
