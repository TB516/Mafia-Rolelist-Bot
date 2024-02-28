using DSharpPlus.SlashCommands;
using DSharpPlus;

namespace Mafia_Game_Rolelist_Bot
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

            await discord.ConnectAsync();
            Console.WriteLine("Connected");
            while (Console.ReadLine() != "stop") ;
        }
    }
}
