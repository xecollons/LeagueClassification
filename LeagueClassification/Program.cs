using LeagueClassification.Core.Implementations;
using LeagueClassification.Core.Interfaces;
using LeagueClassification.SimEngine.Implementations;
using LeagueClassification.SimEngine.Interfaces;

namespace LeagueClassification
{
    public class Program
    {


        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddScoped<ILeagueRepository, MockLeagueRepository>();
            builder.Services.AddScoped<IMatchSimulatorEngine, RandomStrengthMatchSimulatorEngine>();
            builder.Services.AddScoped<IMatchEngineService, MatchEngineService>();
            builder.Services.AddScoped<ITeamClassificationStatsService, TeamClassificationStatsService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}