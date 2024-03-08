using DSharpPlus.SlashCommands;
using DSharpPlus;
using Mafia_Bot.RoleDeckComponents;

namespace Mafia_Bot
{
    internal class Program
    {
        public static DiscordClient Discord = new DiscordClient(new DiscordConfiguration()
        {
            Token = File.ReadAllLines("BotKey.txt")[0],
            TokenType = TokenType.Bot,
            Intents = DiscordIntents.AllUnprivileged
        });

        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }
        static async Task MainAsync()
        {
            SlashCommandsExtension slash = Discord.UseSlashCommands();

            slash.RegisterCommands<RoledeckSlashCommands>();

            await Discord.ConnectAsync();
            Console.WriteLine("Connected");
            while (Console.ReadLine() != "stop") ;
        }
    }
}
