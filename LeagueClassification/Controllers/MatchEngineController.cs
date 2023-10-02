using LeagueClassification.Entities;
using LeagueClassification.Entities.Entities;
using LeagueClassification.Entities.Mappers;
using LeagueClassification.SimEngine.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LeagueClassification.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MatchEngineController :  ControllerBase
    {
        IMatchEngineService _simEngineService;
        ITeamClassificationStatsService _teamClassificationStatsService;
        public MatchEngineController(IMatchEngineService matchEngineService, ITeamClassificationStatsService teamClassificationStatsService)
        {
            _simEngineService = matchEngineService;
            _teamClassificationStatsService = teamClassificationStatsService;
        }

        [HttpGet("Health")]
        public IActionResult Health()
        {
            return Ok();
        }

        [HttpPost("GenerateMatchSchedule")]
        public async Task<IActionResult> GenerateMatchSchedule()
        {
            var matches = await _simEngineService.GenerateMatchSchedule();
            return new JsonResult(matches.Select(m => m.ToDTO()));
        }


        [HttpPost("SimulateMatches")]
        public async Task<IActionResult> SimulateAll()
        {
            var matches = await _simEngineService.SimulateAll();
            return new JsonResult(matches.Select(m => m.ToDTO()));
        }

        [HttpPost("SimulateMatches/round/{round}")]
        public async Task<IActionResult> SimulateRound(int round)
        {
            var matches = await _simEngineService.SimulateRound(round);
            return new JsonResult(matches.Select(m => m.ToDTO()));
        }

        [HttpPost("SimulateMatches/match/{matchId}")]
        public async Task<IActionResult> SimulateMatch(int matchId)
        {
            var match = await _simEngineService.SimulateMatch(matchId);
            if (match == null)
            {
                return NotFound();
            }
            return new JsonResult(match.ToDTO());
        }

        [HttpGet("GetMatches")]
        public async Task<IActionResult> GetMatches()
        {
            var matches = await _simEngineService.GetMatches();
            return new JsonResult(matches.Select(m => m.ToDTO()));
        }

        [HttpGet("GetMatch/{id}")]
        public async Task<IActionResult> GetMatch(int id)
        {
            var match = await _simEngineService.GetMatch(id);
            if (match == null)
            {
                return NotFound();
            }
            return new JsonResult(match.ToDTO());
        }

        [HttpGet("GetMatches/team/{teamId}")]
        public async Task<IActionResult> GetMatchesPerTeam(int teamId)
        {
            var matches = await _simEngineService.GetMatchesPerTeam(teamId);
            return new JsonResult(matches.Select(m => m.ToDTO()));
        }

        [HttpGet("GetMatches/round/{roundNumber}")]
        public async Task<IActionResult> GetMatchesPerRound(int roundNumber)
        {
            var matches = await _simEngineService.GetMatchesPerRound(roundNumber);
            return new JsonResult(matches.Select(m => m.ToDTO()));
        }

        //We could add a pagination here, passing a pagenumber and pageSize argument, but for this scope is unnecessary
        [HttpGet("GetClassification")]
        public async Task<IActionResult> GetClassification()
        {
            var teamClassificationStats = await _teamClassificationStatsService.GetClassificationEntries();
            return new JsonResult(teamClassificationStats.Select(m => m.ToDTO()));
        }
    }
}
