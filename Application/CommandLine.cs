using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Threading.Tasks;

namespace BoardGameGeek.Dungeon
{
    public class CommandLine
    {
        static CommandLine()
        {
            var userArgument = new Argument<string> { Name = "userName", Description = "Geek user name." };

            var allOption = new Option(new[] { "-a", "--all" }, "Analyze all override.") { Argument = new Argument<bool>() };
            var yearOption = new Option(new[] { "-y", "--year" }, "Year to analyze. Defaults to current year.") { Argument = new Argument<int?>(() => DateTime.Now.Year) };

            var playsCommand = new CommandBuilder(new Command("plays", "Get user plays.") { Handler = CommandHandler.Create<string, bool, int?>(PlaysAsync) })
                .AddArgument(userArgument)
                .AddOption(allOption)
                .AddOption(yearOption)
                .Command;

            var statsCommand = new CommandBuilder(new Command("stats", "Get user stats.") { Handler = CommandHandler.Create<string, bool, int?>(StatsAsync) })
                .AddArgument(userArgument)
                .AddOption(allOption)
                .AddOption(yearOption)
                .Command;

            Parser = new CommandLineBuilder(new RootCommand("A command line tool for interacting with the BoardGameGeek API"))
                .AddCommand(playsCommand)
                .AddCommand(statsCommand)
                .UseDefaults()
                .Build();
        }

        public static async Task PlaysAsync(string userName, bool all, int? year)
        {
            if (all)
            {
                year = null;
            }
            var processor = new Processor(new BggService());
            var renderer = new Renderer();
            await renderer.RenderPlays(userName, year, await processor.ProcessPlays(userName, year));
        }

        public static async Task StatsAsync(string userName, bool all, int? year)
        {
            if (all)
            {
                year = null;
            }
            var processor = new Processor(new BggService());
            var renderer = new Renderer();
            await renderer.RenderStats(userName, year, await processor.ProcessStats(userName, year));
        }

        public static Parser Parser { get; }
    }
}
