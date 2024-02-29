using DSharpPlus.SlashCommands;
using DSharpPlus;

namespace Mafia_Bot
{
    internal class Program
    {
        public static DiscordClient discord = new DiscordClient(new DiscordConfiguration()
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
            SlashCommandsExtension slash = discord.UseSlashCommands();

            slash.RegisterCommands<RolelistSlashCommands>(724358800517365851);

            await discord.ConnectAsync();
            Console.WriteLine("Connected");
            while (Console.ReadLine() != "stop") ;
        }
    }
}
