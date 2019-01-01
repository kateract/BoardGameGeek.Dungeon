using Pocket;
using System;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace BoardGameGeek.Dungeon
{
    public class Program
    {
        private static Task<int> Main(string[] args)
        {
            LogEvents.Subscribe(entry =>
            {
                var (message, _) = entry.Evaluate();
                Console.WriteLine($"{entry.TimestampUtc.ToLocalTime():HH:mm:ss} {message}");
            });

            return CommandLine.Parser.InvokeAsync(args);
        }
    }
}
