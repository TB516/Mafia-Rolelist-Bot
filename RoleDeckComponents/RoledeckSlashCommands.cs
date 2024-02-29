using DSharpPlus.SlashCommands;

namespace Mafia_Bot.RoleDeckComponents
{
    internal class RoledeckSlashCommands : ApplicationCommandModule
    {
        [SlashCommand("post", "Stores a role deck for future use")]
        public async Task PostList(InteractionContext ctx,
            [Option("Name", "Name of the role deck")] string name,
            [Option("JSON", "JSON data of the role deck")] string json)
        {
            RoledeckMessage message = new(name, json);
        }
    }
}
