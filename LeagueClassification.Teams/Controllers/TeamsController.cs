using LeagueClassification.Entities.DTOs;
using LeagueClassification.Entities.Entities;
using LeagueClassification.Entities.Mappers;
using LeagueClassification.TeamsServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LeagueClassification.Teams.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TeamsController : ControllerBase
    {
        ITeamsService _teamsService;

        public TeamsController(ITeamsService teamsService)
        {
            _teamsService = teamsService;
        }

        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var teams = await _teamsService.GetTeams();
            return new JsonResult(teams.Select(t => t.ToDTO()));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Team))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTeamPerId(int id)
        {
            var team = await _teamsService.GetTeam(id);
            if (team == null)
            {
                return NotFound();
            }
            return new JsonResult(team.ToDTO());
        }
    }
}
