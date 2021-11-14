using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using BoardGameGeek.Library.Converters;
using BoardGameGeek.Library.Services;
using Flurl.Http;
using Microsoft.Extensions.Logging;

namespace BoardGameGeek.Library.Services
{
    public class Authenticator
    {
        private readonly ILogger logger;

        public Authenticator(IBggService bggService, ILogger logger)
        {
            BggService = bggService;
            this.logger = logger;
        }

        public async Task AuthenticateUser(string userName, string password)
        {
            var fileName = $"BGG-{userName}-Auth.json"; // auth cache
            var options = new JsonSerializerOptions { Converters = { new CookieConverter() }, WriteIndented = true };
            if (password != null)
            {
                logger.LogInformation("Authenticating user");
                var cookies = await BggService.LoginUserAsync(userName, password);
                var json = JsonSerializer.Serialize(cookies, options);
                await File.WriteAllTextAsync(fileName, json);
            }
            else
            {
                var json = await File.ReadAllTextAsync(fileName);
                var cookies = JsonSerializer.Deserialize<IEnumerable<FlurlCookie>>(json, options);
                if (cookies is null)
                {
                    throw new InvalidDataException($"Unable to parse {fileName}");
                }
                BggService.LoginUser(cookies);
            }
        }

        private IBggService BggService { get; }
    }
}
