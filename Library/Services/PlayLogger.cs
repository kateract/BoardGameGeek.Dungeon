using System;
using System.Threading.Tasks;
using BoardGameGeek.Library.Models;
using Microsoft.Extensions.Logging;

namespace BoardGameGeek.Library.Services
{
    public class PlayLogger
    {
        private readonly ILogger logger;

        public PlayLogger(IBggService bggService, ILogger logger)
        {
            BggService = bggService;
            this.logger = logger;
        }

        public Task LogPlay(DateTime date, string location, int quantity, int gameId, int length, bool isIncomplete, bool noWinStats, string comments)
        {
            var play = new Play
            {
                Date = date,
                Location = location,
                Quantity = Math.Max(1, quantity),
                GameId = gameId,
                Length = Math.Max(0, length),
                IsIncomplete = isIncomplete,
                NoWinStats = noWinStats,
                Comments = comments
            };
            logger.LogInformation($"Logging play {play}");
            return BggService.LogUserPlayAsync(play);
        }

        private IBggService BggService { get; }
    }
}
