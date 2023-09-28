using LeagueClassification.Entities.DTOs;
using LeagueClassification.Entities.Entities;
using System.Runtime.CompilerServices;

namespace LeagueClassification.Entities.Mappers
{
    //We could use Automapper for this kind of stuff, but I don't like it too much, I prefer it this way
    public static class EntityDTOMapper
    {
        public static MatchDTO ToDTO(this Match match)
        {
            return new MatchDTO()
            {
                Id = match.Id,
                Played = match.Played,
                HomeTeamId = match.HomeTeam.Id,
                HomeTeamName = match.HomeTeam.Name,
                AwayTeamId = match.AwayTeam.Id,
                AwayTeamName = match.AwayTeam.Name,
                GoalsForHomeTeam = match.GoalsForHomeTeam,
                GoalsForAwayTeam = match.GoalsForAwayTeam,
                Round = match.Round,
            };
        }

        public static TeamDTO ToDTO(this Team team)
        {
            return new TeamDTO()
            {
                Id = team.Id,
                Name = team.Name,
                Strength = team.Strength
            };
        }

        public static TeamClassificationStatsDTO ToDTO(this TeamClassificationStats stats)
        {
            return new TeamClassificationStatsDTO()
            {
                Id = stats.Team.Id,
                TeamName = stats.Team.Name,
                Points = stats.Points,
                MatchesWon = stats.MatchesWon,
                MatchesDrawn = stats.MatchesDrawn,
                MatchesLost = stats.MatchesLost,
                GoalsFor = stats.GoalsFor,
                GoalsAgainst = stats.GoalsAgainst
            };
        }
    }
}
