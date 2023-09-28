using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueClassification.Entities.Entities
{
    public class Match
    {
        public int Id { get; set; }
        public Team HomeTeam { get; set; }
        public int GoalsForHomeTeam { get; set; }
        public Team AwayTeam { get; set; }
        public int GoalsForAwayTeam { get; set; }
        public bool Played { get; set; }
        public int Round { get; set; }
    }
}
