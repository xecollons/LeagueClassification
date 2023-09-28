using LeagueClassification.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueClassification.SimEngine.Interfaces
{
    public interface ITeamClassificationStatsService
    {
        Task<List<TeamClassificationStats>> GetClassificationEntries();
        Task UpdateClassificationEntries();
    }
}
