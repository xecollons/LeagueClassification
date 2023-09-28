using LeagueClassification.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueClassification.Entities.DTOs
{
    public class MatchDTO
    {
        public int Id { get; set; }
        public int HomeTeamId { get; set; }
        public string HomeTeamName { get; set; } = string.Empty;
        public int GoalsForHomeTeam { get; set; }
        public int AwayTeamId { get; set; }
        public string AwayTeamName { get; set; } = string.Empty;
        public int GoalsForAwayTeam { get; set; }
        public bool Played { get; set; }
        public int Round { get; set; }
    }
}
